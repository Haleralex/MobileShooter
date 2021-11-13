using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _meshRenderer;

    public LayerMask collisionMask;
    public Color Color
    {
        get
        {
            return _meshRenderer.material.color;
        }
        set
        {
            _meshRenderer.material.color = value;
        }
    }
    public event Action<Treasure> TreasureWasRaised;

    private bool _isColliding = false;

    private void Update()
    {
        _isColliding = Physics.CheckSphere(transform.position, 1, collisionMask);
        if (_isColliding)
        {
            TreasureWasRaised?.Invoke(this);
        }
    }
}
