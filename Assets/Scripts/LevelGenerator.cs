using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoSingleton<LevelGenerator>
{
    public GameObject UFOEnemyPrefab;
    public GameObject UFOBossPrefab;

    public int NumberOfUFOEnemies;

    private const char EMPTY = '-';
    private const char ENEMY = '+';

    private Camera camera;

    private Vector2 ufoEnemySpriteSize;

    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float halfScreenHeight;

    private int currentNumberOfUFOEnemies = 0;

    private Dictionary<Vector2, char> tiles = new Dictionary<Vector2, char>();

    protected override void Awake()
    {
        base.Awake();

        camera = Camera.main;

        boundRight = Helper.GetScreenBoundRight(camera);
        boundLeft = Helper.GetScreenBoundLeft(camera);
        boundTop = Helper.GetScreenBoundTop(camera);
        halfScreenHeight = Helper.GetScreenBoundTop(camera) / 2;

        SpriteRenderer ufoEnemySpriteRenderer = UFOEnemyPrefab.GetComponent<SpriteRenderer>();
        ufoEnemySpriteSize = new Vector2(ufoEnemySpriteRenderer.bounds.size.x, ufoEnemySpriteRenderer.bounds.size.y);
    }

    private void Start()
    {
        EventManager.Instance.OnNormalLevelStart += CreateLevel;
        EventManager.Instance.OnUFOBossLevelStart += CreateUFOBossLevel;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNormalLevelStart -= CreateLevel;
        EventManager.Instance.OnUFOBossLevelStart -= CreateUFOBossLevel;
    }

    private void CreateLevel()
    {
        currentNumberOfUFOEnemies = 0;

        InitTiles();

        for (int i = 0; i < tiles.Count; i++)
        {
            int tileIndexToCheck = Random.Range(0, tiles.Count - 1);

            if (GetTile(tileIndexToCheck) != EMPTY)
                continue;
            else
            {
                if (currentNumberOfUFOEnemies >= NumberOfUFOEnemies)
                {
                    break;
                }
                else
                {
                    SetTile(tileIndexToCheck, ENEMY);
                    Vector2 tilePosition = tiles.ElementAt(tileIndexToCheck).Key;
                    SpawnUFOEnemy(UFOEnemyPrefab, tilePosition);
                    currentNumberOfUFOEnemies++;
                }
            }
        }
    }

    private void InitTiles()
    {
        tiles.Clear();

        for (float y = halfScreenHeight + ufoEnemySpriteSize.y; y < boundTop - ufoEnemySpriteSize.y; y += ufoEnemySpriteSize.y)
        {
            for (float x = boundLeft + ufoEnemySpriteSize.x; x < boundRight - ufoEnemySpriteSize.x; x += ufoEnemySpriteSize.x)
            {
                tiles.Add(new Vector2(x, y), EMPTY);
            }
        }
    }

    private void SetTile(int index, char tile)
    {
        Vector2 tileKey = tiles.ElementAt(index).Key;

        tiles[tileKey] = tile;
    }

    private char GetTile(int index)
    {
        char tile = tiles.ElementAt(index).Value;

        return tile;
    }

    private void CreateUFOBossLevel()
    {
        Vector3 spawnPosition = new Vector3((boundRight / 2), boundTop, 0);
        SpawnUFOEnemy(UFOBossPrefab, spawnPosition);
    }

    private void SpawnUFOEnemy(GameObject prefab, Vector3 position)
    {
        GameObject ufoEnemyGO = Instantiate(prefab, position, Quaternion.identity);

        EventManager.Instance.StartUFOEnemySpawnGOEvent(ufoEnemyGO);

        SpriteRenderer ufoEnemyRenderer = ufoEnemyGO.GetComponent<SpriteRenderer>();
        Sprite ufoEnemySprite = ufoEnemyRenderer.sprite;

        Helper.UpdateColliderShapeToSprite(ufoEnemyGO, ufoEnemySprite);
    }
}
