using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public event Action<EnemyAI> EnemyDowned;
    private List<EnemyAI> _enemies = new List<EnemyAI>();
    public LayerMask WhatIsPlayer;

    [SerializeField]
    private Transform _aimTransform;

    [SerializeField]
    private CombatSystem _combatSystem;

    public void AddEnemy(EnemyAI enemy)
    {
        _enemies.Add(enemy);
        enemy.SetAim(_aimTransform);

        enemy.Attacked += _combatSystem.AttackPlayer;
    }
    public void RemoveEnemy(EnemyAI enemy)
    {
        _enemies.Remove(enemy);

        enemy.Attacked -= _combatSystem.AttackPlayer;
    }

    private void Update()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].PlayerInSightRange = Physics.CheckSphere(_enemies[i].transform.position, _enemies[i].SightRange, WhatIsPlayer);
            _enemies[i].PlayerInAttackRange = Physics.CheckSphere(_enemies[i].transform.position, _enemies[i].AttackRange, WhatIsPlayer);

            if (!_enemies[i].PlayerInSightRange && !_enemies[i].PlayerInAttackRange) _enemies[i].Patroling();
            if (_enemies[i].PlayerInSightRange && !_enemies[i].PlayerInAttackRange) _enemies[i].ChasePlayer();
            if (_enemies[i].PlayerInSightRange && _enemies[i].PlayerInAttackRange) _enemies[i].AttackPlayer();
        }
    }
}
