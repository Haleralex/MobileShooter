using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyСustomizer : MonoBehaviour
{
    [SerializeField]
    private TreasureSpawner _treasureSpawner;

    [SerializeField]
    private GameObject _treasurePrefab;

    [SerializeField]
    WeaponExample _weaponExample;

    public event Action<Treasure> TreasureWasSpawned;
    public event Action<Treasure> TreasureWasReturned;
    public List<Color> Colors = new List<Color>();

    private void OnEnable()
    {
        _treasureSpawner.CreatePool(_treasurePrefab);
    }

    public void MakeTreasureSpawnRequest(EnemyAI enemy)
    {
        if (!_treasureSpawner.IsPossibleToSpawn)
            return;

        var treasure = _treasureSpawner.SpawnTreasureFromPool();
        treasure.gameObject.SetActive(true);
        treasure.transform.position = enemy.transform.position;
        treasure.Color = Colors[UnityEngine.Random.Range(0, Colors.Count)];

        treasure.TreasureWasRaised += ReturnUsedTreasureToSpawner;
    }


    private void ReturnUsedTreasureToSpawner(Treasure treasure)
    {
        treasure.TreasureWasRaised -= ReturnUsedTreasureToSpawner;
        _treasureSpawner.ReturnTreasureToPool(treasure);
        treasure.gameObject.SetActive(false);
        _weaponExample.SetBulletColor(treasure.Color);
    }
}
