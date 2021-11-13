using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 100;

    private int _currentHealth;

    public int CurrentHelth => _currentHealth;

    public event Action<float> OnHealthChanged = delegate { };
    

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    

    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;

        OnHealthChanged(_currentHealth);
    }
}
