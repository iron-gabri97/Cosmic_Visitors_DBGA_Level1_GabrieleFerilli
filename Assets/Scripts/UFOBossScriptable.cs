using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UFOBoss", menuName = "UFOEnemies/UFOBoss")]
public class UFOBossScriptable : ScriptableObject
{
    public int UFOBossHealth;
    public int UFOBossDamage;
    public Sprite UFOBossSprite;
    public GameObject UFOBossBulletPrefab;
    public Sprite UFOBossBulletSprite;
    public float TimeBetweenShots;
    public int NumberOfBullets;
    public int MinShotAngle;
    public int MaxShotAngle;
}

