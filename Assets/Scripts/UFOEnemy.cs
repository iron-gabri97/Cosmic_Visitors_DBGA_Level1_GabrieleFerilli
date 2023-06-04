using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UFOEnemy : MonoBehaviour, IDamageable, IShooter
{
    public int Health { get; set; }
    public int Damage { get; set; }

    public List<UFOEnemyScriptable> UFOEnemies = new List<UFOEnemyScriptable>();
    public GameObject bulletPrefab;
    public UFOEnemyScriptable UFOEnemyScriptable;

    private Camera camera;
    private Player player;

    private float boundRight= 8.5f;
    private float boundLeft = -8.5f;
    private float boundBottom = -3.0f;

    private ENEMY_MOVE_STATE moveStatus = 0;
    private float moveHorizontalAmount = 0.1f;
    private float moveVerticalAmount = 0.5f;
    private float moveTimer = 0.5f;
    private bool moveRight = true;

    private Vector2 halfSpriteSize;

    private int numberOfBullets;
    private int shotMinAngle, shotMaxAngle;
    private Vector3 bulletSpawnOffset = Vector3.down;
    private float timeBetweenShots;
    private float shootTimer;
    private string ufoEnemyBulletTag = "UFOEnemyBullet";

    private SpriteRenderer spriteRenderer;

    private enum ENEMY_MOVE_STATE
    {
        NO_MOVE = -1,
        MOVE_RIGHT = 0,
        MOVE_DOWN = 1,
        MOVE_LEFT = 2
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>();

        camera = Camera.main;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Health = UFOEnemyScriptable.UFOEnemyHealth;
        Damage = UFOEnemyScriptable.UFOEnemyDamage;
        shootTimer = UnityEngine.Random.Range(0f, 1.5f);
        numberOfBullets = UFOEnemyScriptable.NumberOfBullets;
        shotMinAngle = UFOEnemyScriptable.MinShotAngle;
        shotMaxAngle = UFOEnemyScriptable.MaxShotAngle;

    }

    private void OnDisable()
    {
      
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            Move();
        }

        if (shootTimer <= 0)
        {
            Shoot();
        }
    }
    public void TakeDamage(int damage)
    {
            Health -= damage;

            spriteRenderer.color = Color.magenta;

            if (Health <= 0)
            {
                EventManager.Instance.StartUFOEnemyDefeatEvent(gameObject);
                Destroy(gameObject);
            }
    }

    public void Shoot()
    {
        shootTimer = 1.5f;

        float angleStep = (shotMaxAngle - shotMinAngle) / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position + bulletSpawnOffset, Quaternion.identity);

            bulletGO.tag = ufoEnemyBulletTag;

            Bullet bullet = bulletGO.GetComponent<Bullet>();

            EventManager.Instance.StartBulletSpawnIntEvent(UFOEnemyScriptable.UFOEnemyDamage);

            EventManager.Instance.StartBulletSpawnEvent(bulletGO, UFOEnemyScriptable.UFOEnemyBulletSprite, Vector3.down, angleStep - (i * angleStep));

            EventManager.Instance.StartBulletSpawnGOEvent(bulletGO);
        }
    }

    private void Move()
    {
        moveTimer = 0.5f;

        if (transform.position.y <= boundBottom + halfSpriteSize.y)
        {
            player.Lives = 0;
        }

        switch (moveStatus)
        {
            case ENEMY_MOVE_STATE.NO_MOVE:
                break;

            case ENEMY_MOVE_STATE.MOVE_RIGHT:
                transform.Translate(new Vector3(moveHorizontalAmount, 0, 0));
               
                if (transform.position.x >= boundRight - halfSpriteSize.x)
                {
                    transform.position = new Vector3(boundRight - (halfSpriteSize.x), transform.position.y, transform.position.z);
                    moveStatus = ENEMY_MOVE_STATE.MOVE_DOWN;
                    moveRight = false;
                }
                break;

            case ENEMY_MOVE_STATE.MOVE_LEFT:
                transform.Translate(new Vector3(-moveHorizontalAmount, 0, 0));
                
                if (transform.position.x <= boundLeft + halfSpriteSize.x)
                {
                    transform.position = new Vector3(boundLeft + (halfSpriteSize.x), transform.position.y, transform.position.z);
                    moveStatus = ENEMY_MOVE_STATE.MOVE_DOWN;
                    moveRight = true;
                }
                break;
                   
            case ENEMY_MOVE_STATE.MOVE_DOWN:
                transform.Translate(new Vector3(0, -moveVerticalAmount, 0));

                if (moveRight)
                {
                    moveStatus = ENEMY_MOVE_STATE.MOVE_RIGHT;
                }
                else
                {
                    moveStatus = ENEMY_MOVE_STATE.MOVE_LEFT;
                }

                break;
        }
    }

}
