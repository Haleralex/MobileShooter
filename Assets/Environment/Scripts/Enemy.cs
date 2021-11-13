using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyAI _enemyAI;

    [SerializeField]
    private int _maxHealth = 2;

    public EnemyAI EnemyAI => _enemyAI;

    private int _currentHealt;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        _currentHealt = _maxHealth;
    }

    
}
