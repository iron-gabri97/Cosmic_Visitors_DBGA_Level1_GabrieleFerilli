using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UFOEnemy", menuName = "UFOEnemies/UFOEnemy")]
public class UFOEnemyScriptable : ScriptableObject
{
    public int UFOEnemyHealth;
    public int UFOEnemyDamage;
    public Sprite UFOEnemySprite;
    public GameObject UFOEnemyBulletPrefab;
    public Sprite UFOEnemyBulletSprite;
    public int NumberOfBullets;
    public int MinShotAngle;
    public int MaxShotAngle;
}
