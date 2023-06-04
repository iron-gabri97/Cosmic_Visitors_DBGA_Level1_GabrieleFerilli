using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBoss : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }

    public float speed;

    public GameObject UFOBossPrefab;
    public UFOBossScriptable UFOBossScriptable;

    private enum UFOBOSS_MOVE_STATE
    {
        NO_MOVE = -1,
        MOVE_RIGHT = 0,
        MOVE_LEFT = 1
    }

    private UFOBOSS_MOVE_STATE moveState = 0;

    private Camera camera;
    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private Vector2 halfSpriteSize;

    private float shootTimer;
    private GameObject bulletPrefab;
    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private string ufoEnemyBulletTag = "UFOEnemyBullet";

    private void Awake()
    {
        camera = Camera.main;

        boundRight = Helper.GetScreenBoundRight(camera);
        boundLeft = Helper.GetScreenBoundLeft(camera);
        boundTop = Helper.GetScreenBoundTop(camera);

        Health = UFOBossScriptable.UFOBossHealth;
        Damage = UFOBossScriptable.UFOBossDamage;
    }

    private void Start()
    {

        SpriteRenderer ufoBossRenderer = UFOBossPrefab.GetComponent<SpriteRenderer>();
        ufoBossRenderer.sprite = UFOBossScriptable.UFOBossSprite;

        halfSpriteSize = new Vector2((ufoBossRenderer.bounds.size.x / 2), (ufoBossRenderer.bounds.size.y / 2));

        shootTimer = UnityEngine.Random.Range(0f, 1.0f);
        shotMinAngle = UFOBossScriptable.MinShotAngle;
        shotMaxAngle = UFOBossScriptable.MaxShotAngle;
        numberOfBullets = UFOBossScriptable.NumberOfBullets;
        bulletPrefab = UFOBossScriptable.UFOBossBulletPrefab;

        EventManager.Instance.StartInitUFOBossHealthIntEvent(Health);
    }

    private void OnDisable()
    {
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        Move();

        if (shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        shootTimer = 1.0f;

        float angleStep = (shotMaxAngle - shotMinAngle) / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);

            bulletGO.tag = ufoEnemyBulletTag;

            Bullet bullet = bulletGO.GetComponent<Bullet>();

            EventManager.Instance.StartBulletSpawnEvent(bulletGO, UFOBossScriptable.UFOBossBulletSprite, Vector3.down, angleStep - (i * angleStep));

            EventManager.Instance.StartBulletSpawnIntEvent(UFOBossScriptable.UFOBossDamage);

            EventManager.Instance.StartBulletSpawnGOEvent(bulletGO);
        }
    }

    public void TakeDamage(int damage)
    {
            Health -= damage;
            EventManager.Instance.StartUpdateUFOBossHealthIntEvent(Health);

            if (Health <= 0)
            {
                EventManager.Instance.StartUFOEnemyDefeatEvent(gameObject);
                Destroy(gameObject);
            }
    }

    private void Move()
    {
        if (transform.position.y >= boundTop - halfSpriteSize.y)
        {
            transform.position = new Vector3(transform.position.x, boundTop - halfSpriteSize.y, transform.position.z);
        }

        switch (moveState)
        {
            case UFOBOSS_MOVE_STATE.NO_MOVE:
                break;

            case UFOBOSS_MOVE_STATE.MOVE_RIGHT:
                transform.position += speed * Time.deltaTime * Vector3.right;
               
                if (transform.position.x >= boundRight - halfSpriteSize.x)
                    {
                        transform.position = new Vector3(boundRight - (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
                        moveState = UFOBOSS_MOVE_STATE.MOVE_LEFT;
                    }

                    break;

            case UFOBOSS_MOVE_STATE.MOVE_LEFT:
                transform.position += speed * Time.deltaTime * Vector3.left;

                if (transform.position.x <= boundLeft + halfSpriteSize.x)
                     {
                         transform.position = new Vector3(boundLeft + (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
                         moveState = UFOBOSS_MOVE_STATE.MOVE_RIGHT;
                     }

                    break;
        }
    }
}
