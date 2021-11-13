using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private EnemyСustomizer _enemyCustomizer;

    [SerializeField]
    private EnemyAIController _enemyAIController;

    [SerializeField]
    private GameObject _enemyPrefab;

    public event Action<EnemyAI> EnemyWasSpawned;
    public event Action<EnemyAI> EnemyWasReturned;


    private void OnEnable()
    {
        _enemySpawner.CreatePool(_enemyPrefab);

        InvokeRepeating(nameof(MakeEnemySpawnRequest), 0, 3);
    }

    private void MakeEnemySpawnRequest()
    {
        if (!_enemySpawner.IsPossibleToSpawn)
            return;

        var enemy = _enemySpawner.SpawnEnemyFromPool();
        Vector3 v3Pos = Vector3.zero;
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(1.5f, 3f), 0.5f, UnityEngine.Random.Range(5f, 10f)));

        }
        else
        {
            v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(-2f, -0.5f), 0.5f, UnityEngine.Random.Range(5f, 10f)));
        }
        enemy.transform.position = v3Pos;

        enemy.WasKilled += ReturnUsedEnemyToSpawner;

        _enemyAIController.AddEnemy(enemy);

        EnemyWasSpawned?.Invoke(enemy);
    }

    private void ReturnUsedEnemyToSpawner(EnemyAI enemy)
    {
        enemy.WasKilled -= ReturnUsedEnemyToSpawner;

        _enemyAIController.RemoveEnemy(enemy);

        _enemySpawner.ReturnEnemyToPool(enemy);

        EnemyWasReturned?.Invoke(enemy);

        _enemyCustomizer.MakeTreasureSpawnRequest(enemy);
    }
}
