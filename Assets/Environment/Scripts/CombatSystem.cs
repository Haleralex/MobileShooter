using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private WeaponExample _weaponExample;


    [SerializeField]
    private Text _hpDisplayer;

    private const string UI_TEXT = "Player HP = ";

    public int PlayerAttackValue = 1;
    public int EnemyAttackValue = 5;

    private void OnEnable()
    {
        _player.OnHealthChanged += UpdateHPDisplayer;
        _weaponExample.PoolWasCreated += AddListenerToBullets;
    }

    public void AttackPlayer()
    {
        _player.ModifyHealth(-EnemyAttackValue);

        if (_player.CurrentHelth == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnAttackEnemy(EnemyAI enemy)
    {
        enemy.ModifyHealth(-PlayerAttackValue);
    }

    private void UpdateHPDisplayer(float curHP)
    {
        _hpDisplayer.text = UI_TEXT + curHP.ToString();
    }

    private void AddListenerToBullets(Queue<Bullet> pool)
    {
        foreach (var k in pool)
        {
            k.EnemyWasHitted += OnAttackEnemy;
        }
    }
}
