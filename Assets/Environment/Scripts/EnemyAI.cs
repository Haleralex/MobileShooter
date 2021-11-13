using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _hp1, _hp2;
    public NavMeshAgent agent;

    private Transform _playerTransform;

    public LayerMask WhatIsGround;

    public Vector3 WalkPoint;
    private bool _walkPointSetted;
    public float WalkPointRange;

    public float TimeBetweenAttacks;
    private bool _alreadyAttacked;

    public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;

    public event Action Attacked = delegate { };
    public event Action<EnemyAI> WasKilled = delegate { };

    [SerializeField]
    private GameObject drop;

    public void SetAim(Transform aimTransform)
    {
        _playerTransform = aimTransform;

        Reset();
    }

    public void Reset()
    {
        _currentHealth = _maxHealth;
        _hp1.SetActive(true);
        _hp2.SetActive(true);
    }

    public void Patroling()
    {
        if (!_walkPointSetted) SearchWalkPoint();

        if (_walkPointSetted)
        {
            agent.SetDestination(WalkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSetted = false;
    }

    private void SearchWalkPoint()
    {
        Debug.Log("here");
        float randomZ = UnityEngine.Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = UnityEngine.Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
            _walkPointSetted = true;
    }

    public void ChasePlayer()
    {
        if (!agent.isActiveAndEnabled)
            return;

        agent.SetDestination(_playerTransform.position);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(_playerTransform);

        if (!_alreadyAttacked)
        {
            Attacked?.Invoke();

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void Die()
    {
        WasKilled?.Invoke(this);

        
    }
    private int _maxHealth = 2;
    public int _currentHealth;
    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        _hp1.SetActive(false);

        if (_currentHealth == 0)
        {
            _hp2.SetActive(false);
            Die();
        }
    }

    
    
}
