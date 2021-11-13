using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private Rigidbody _bulletRigidbody;

    public Rigidbody BulletRigidbody => _bulletRigidbody;

    public event Action<EnemyAI> EnemyWasHitted;

    public event Action<Bullet> SmthWasHitted;

    [SerializeField]
    private MeshRenderer _meshRenderer;

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

    void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.tag == "Enemy"))
        {
            return;
        }


        SmthWasHitted?.Invoke(this);

        if (collision.transform.parent.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            EnemyWasHitted?.Invoke(enemy);
        }
    }
}
