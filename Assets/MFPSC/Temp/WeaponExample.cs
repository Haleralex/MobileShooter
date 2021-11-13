using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This is not a functional weapon script. It just shows how to implement shooting and reloading with buttons system.
/// </summary>
public class WeaponExample : MonoBehaviour
{
    public FP_Input playerInput;

    public float shootRate = 0.15F;
    public float reloadTime = 1.0F;
    public int ammoCount = 15;

    private int ammo;
    private float delay;
    private bool reloading;

    public event Action<Queue<Bullet>> PoolWasCreated;
    private bool _alreadyAttacked;
    public float TimeBetweenAttacks;
    public event Action Attacked = delegate { };
    public event Action WasKilled = delegate { };

    private Color _currentColor;

    private Queue<Bullet> _bulletQ = new Queue<Bullet>();

    [SerializeField]
    private GameObject AmoHolder;
    [SerializeField]
    private GameObject SpawnPlace;

    [SerializeField]
    private Bullet _bulletPrefab;

    void Start()
    {
        ammo = ammoCount;
        CreatePool();
    }

    void Update()
    {
        if (playerInput.Shoot() || Input.GetKey(KeyCode.LeftControl))                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
            if (Time.time > delay)
                Shoot();

        if (playerInput.Reload())                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
            if (!reloading && ammoCount < ammo)
                StartCoroutine("Reload");
    }

    void Shoot()
    {
        if (ammoCount > 0)
        {
            Attack();
            ammoCount--;
        }
        else
            Debug.Log("Empty");

        delay = Time.time + shootRate;
    }

    IEnumerator Reload()
    {
        reloading = true;
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammo;
        Debug.Log("Reloading Complete");
        reloading = false;
    }

    void OnGUI()
    {
        GUILayout.Label("AMMO: " + ammoCount);
    }

    public void Attack()
    {
        if (!_alreadyAttacked)
        {
            Attacked?.Invoke();

            var bullet = _bulletQ.Dequeue();
            bullet.transform.position = SpawnPlace.transform.position;
            bullet.gameObject.SetActive(true);
            bullet.Color = _currentColor;

            bullet.BulletRigidbody.AddForce(transform.forward * 100, ForceMode.Impulse);

            StartCoroutine("ReturnBulletToPoolCoroutine", bullet);

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    IEnumerator ReturnBulletToPoolCoroutine(Bullet bullelt)
    {
        yield return new WaitForSeconds(2);
        ReturnBulletToPool(bullelt);
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.BulletRigidbody.velocity = Vector3.zero;
        _bulletQ.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void SetBulletColor(Color newColor)
    {
        _currentColor = newColor;
    }
    private void CreatePool()
    {
        for (int i = 0; i < ammoCount; i++)
        {
            var bullet = GameObject.Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.transform.parent = AmoHolder.transform;
            bullet.gameObject.SetActive(false);
            _bulletQ.Enqueue(bullet);
        }
        foreach (var k in _bulletQ)
        {
            k.SmthWasHitted += ReturnBulletToPool;
        }

        PoolWasCreated?.Invoke(_bulletQ);
    }
}
