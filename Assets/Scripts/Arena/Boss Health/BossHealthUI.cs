using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    private BossPrototype _boss;

    private void Start()
    {
        _boss = GameObject.FindWithTag("Boss").GetComponent<BossPrototype>();
        _healthBar.value = _boss.BossHealth / _boss.MaxBossHealth;
        _boss.HealthChanged += UpdateBossHealth;
    }

    void UpdateBossHealth(float health)
    {
        _healthBar.value = _boss.BossHealth / _boss.MaxBossHealth;
    }
}
