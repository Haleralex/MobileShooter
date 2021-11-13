using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Queue<EnemyAI> _pool = new Queue<EnemyAI>();

    public Transform Parent;

    public int PoolAmount = 2;

    public bool IsPossibleToSpawn => _pool.Count > 0;


    public EnemyAI SpawnEnemyFromPool()
    {
        if (_pool.Count == 0)
            return null;
        var enemy = _pool.Dequeue();
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    public void ReturnEnemyToPool(EnemyAI returnedd)
    {
        returnedd.gameObject.SetActive(false);
        _pool.Enqueue(returnedd);
    }

    public void CreatePool(GameObject enemyPrefab)
    {
        if (!enemyPrefab.TryGetComponent<EnemyAI>(out var foo))
        {
            return;
        }

        for (int i = 0; i < PoolAmount; i++)
        {
            var enemyGO = GameObject.Instantiate(enemyPrefab);
            var enemy = enemyGO.GetComponent<EnemyAI>();
            enemyGO.SetActive(false);
            enemyGO.transform.parent = Parent;
            _pool.Enqueue(enemy);
        }
    }
}
