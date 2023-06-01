using System;
using System.Collections;
using System.Collections.Generic;
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

    private float boundRight;
    private float boundLeft;
    private float boundTop;
    private float boundBottom;

    private ENEMY_MOVE_STATE moveStatus = 0;
    private float moveHorizontalAmount = 0.5f;
    private float moveVerticalAmount;
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
        spriteRenderer.sprite = UFOEnemyScriptable.UFOEnemySprite;

        Health = UFOEnemyScriptable.UFOEnemyHealth;
        Damage = UFOEnemyScriptable.UFOEnemyDamage;
        numberOfBullets = UFOEnemyScriptable.NumberOfBullets;
        shotMinAngle = UFOEnemyScriptable.MinShotAngle;
        shotMaxAngle = UFOEnemyScriptable.MaxShotAngle;
        timeBetweenShots = UFOEnemyScriptable.TimeBetweenShots;

        EventManager.Instance.OnUFOEnemyDamage += TakeDamage;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUFOEnemyDamage -= TakeDamage;
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
    public void TakeDamage(GameObject damagedObject, int damage)
    {
        if (gameObject == damagedObject)
        {
            Health -= damage;

            spriteRenderer.color = Color.black;

            if (Health <= 0)
            {
                EventManager.Instance.StartUFOEnemyDefeatEvent(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void Shoot()
    {
        shootTimer = timeBetweenShots;

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
        moveTimer = 0.4f;

        if (transform.position.x >= boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3(boundRight - (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveStatus = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = false;
        }
        else if (transform.position.x <= boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3(boundLeft + (0.1f + halfSpriteSize.x), transform.position.y, transform.position.z);
            moveStatus = ENEMY_MOVE_STATE.MOVE_DOWN;
            moveRight = true;
        }

        if (transform.position.y >= boundTop - halfSpriteSize.y)
        {
            transform.position = new Vector3(transform.position.x, halfSpriteSize.y, transform.position.z);
        }

        if (transform.position.y < boundBottom + halfSpriteSize.y)
        {
            player.Lives--;
        }

        switch (moveStatus)
        {
            case ENEMY_MOVE_STATE.NO_MOVE:
                break;

            case ENEMY_MOVE_STATE.MOVE_RIGHT:
                transform.Translate(new Vector3(moveHorizontalAmount, 0, 0));
                break;

            case ENEMY_MOVE_STATE.MOVE_LEFT:
                transform.Translate(new Vector3(-moveHorizontalAmount, 0, 0));
                break;
                   
            case ENEMY_MOVE_STATE.MOVE_DOWN:
                transform.Translate(new Vector3(0, -moveVerticalAmount, 0));

                if (moveRight)
                {
                    moveStatus = ENEMY_MOVE_STATE.MOVE_RIGHT;
                }
                else if (!moveRight)
                {
                    moveStatus = ENEMY_MOVE_STATE.MOVE_LEFT;
                }

                break;
        }
    }

}
