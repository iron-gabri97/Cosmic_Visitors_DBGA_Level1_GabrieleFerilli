using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoSingleton<EventManager>
{
    public delegate void OnGameEvent();
    public delegate void OnGameIntEvent(int value);
    public delegate void OnGameGameObjectEvent(GameObject GO);
    public delegate void OnGameDamageEvent(GameObject GO, int amount);

    public delegate void OnBulletSpawnEvent(GameObject GO, Sprite sprite, Vector3 generalDirection, float angleToShoot);

    public OnGameEvent OnGameStart;
    public OnGameEvent OnGamePlaying;
    public OnGameEvent OnNormalLevelStart;
    public OnGameEvent OnUFOBossLevelStart;
    public OnGameEvent OnGamePause;
    public OnGameEvent OnGameOver;
    public OnGameEvent OnGameOverWin;
    public OnGameEvent OnGameOverLose;
    public OnGameEvent OnGameReInit;

    public OnGameIntEvent OnInitPlayerHealth;
    public OnGameIntEvent OnInitPlayerLives;
    public OnGameIntEvent OnInitUFOBossHealth;
    public OnGameIntEvent OnUpdatePlayerLives;
    public OnGameIntEvent OnUpdatePlayerHealth;
    public OnGameIntEvent OnUpdateUFOBossHealth;
    public OnGameIntEvent OnBulletSpawnInt;

    public OnGameGameObjectEvent OnBulletSpawnGO;
    public OnGameGameObjectEvent OnBulletDestroyed;
    public OnGameGameObjectEvent OnUFOEnemySpawnedGO;
    public OnGameGameObjectEvent OnUFOEnemyDefeated;

    public OnGameDamageEvent OnUFOEnemyDamage;
    public OnGameDamageEvent OnPlayerDamage;

    public OnBulletSpawnEvent OnBulletSpawn;

    public void StartGameStartEvent()
    {
        OnGameStart?.Invoke();
    }

    public void StartGamePlayingEvent()
    {
        OnGamePlaying?.Invoke();
    }

    public void StartNormalLevelEvent()
    {
        OnNormalLevelStart?.Invoke();
    }

    public void StartUFOBossLevelEvent()
    {
        OnUFOBossLevelStart?.Invoke();
    }

    public void StartGamePauseEvent()
    {
        OnGamePause?.Invoke();
    }

    public void StartGameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public void StartGameOverWinEvent()
    {
        OnGameOverWin?.Invoke();
    }

    public void StartGameOverLoseEvent()
    {
        OnGameOverLose?.Invoke();
    }

    public void StartGameReInitEvent()
    {
        OnGameReInit?.Invoke();
    }

    public void StartBulletSpawnGOEvent(GameObject GO)
    {
        OnBulletSpawnGO?.Invoke(GO);
    }

    public void StartUFOEnemySpawnGOEvent(GameObject GO)
    {
        OnUFOEnemySpawnedGO?.Invoke(GO);
    }

    public void StartUFOEnemyDefeatEvent(GameObject GO)
    {
        OnUFOEnemyDefeated?.Invoke(GO);
    }

    public void StartBulletDestroyedEvent(GameObject GO)
    {
        OnBulletDestroyed?.Invoke(GO);
    }
    public void StartUpdatePlayerLivesEvent(int value)
    {
        OnUpdatePlayerLives?.Invoke(value);
    }

    public void StartUpdatePlayerHealthEvent(int value)
    {
        OnUpdatePlayerHealth?.Invoke(value);
    }

    public void StartUpdateUFOBossHealthIntEvent(int value)
    {
        OnUpdateUFOBossHealth?.Invoke(value);
    }

    public void StartBulletSpawnIntEvent(int value)
    {
        OnBulletSpawnInt?.Invoke(value);
    }

    public void StartInitPlayerHealthEvent(int value)
    {
        OnInitPlayerHealth?.Invoke(value);
    }

    public void StartInitPlayerLivesEvent(int value)
    {
        OnInitPlayerLives?.Invoke(value);
    }

    public void StartInitUFOBossHealthIntEvent(int value)
    {
        OnInitUFOBossHealth?.Invoke(value);
    }

    public void StartUFOEnemyDamageEvent(GameObject GO, int amount)
    {
        OnUFOEnemyDamage?.Invoke(GO, amount);
    }

    public void StartPlayerDamageEvent(GameObject GO, int amount)
    {
        OnPlayerDamage?.Invoke(GO, amount);
    }

    public void StartBulletSpawnEvent(GameObject GO, Sprite sprite, Vector3 generalDirection, float angleToShoot)
    {
        OnBulletSpawn?.Invoke(GO, sprite, generalDirection, angleToShoot);
    }
}
