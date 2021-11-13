using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    private Queue<Treasure> _pool = new Queue<Treasure>();

    public Transform Parent;

    public int PoolAmount = 2;

    public bool IsPossibleToSpawn => _pool.Count > 0;


    public Treasure SpawnTreasureFromPool()
    {
        if (_pool.Count == 0)
            return null;

        var enemy = _pool.Dequeue();

        enemy.gameObject.SetActive(true);
        return enemy;
    }

    public void ReturnTreasureToPool(Treasure returnedd)
    {
        returnedd.gameObject.SetActive(false);
        _pool.Enqueue(returnedd);
    }

    public void CreatePool(GameObject treasurePrefab)
    {
        if (!treasurePrefab.TryGetComponent<Treasure>(out var foo))
        {
            return;
        }

        for (int i = 0; i < PoolAmount; i++)
        {
            var enemyGO = GameObject.Instantiate(treasurePrefab);
            var enemy = enemyGO.GetComponent<Treasure>();
            enemyGO.SetActive(false);
            enemyGO.transform.parent = Parent;
            _pool.Enqueue(enemy);
        }
    }

    public void SubscribeEvents(Action<Treasure> action)
    {
        foreach (var k in _pool)
        {
            k.TreasureWasRaised += action;
        }
    }
}
