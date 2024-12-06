using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class AnimeBossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    private float _majorPhaseThreshold;

    private AnimeBoss _boss;

    private void Start()
    {
        _boss = GameObject.FindWithTag("Boss").GetComponent<AnimeBoss>();

        // set initial level
        _healthBar.value = 1; // max

        _majorPhaseThreshold = _boss.GetMajorPhaseThreshold();
    }

    void Update()
    {
        if (_boss.IsInMajorPhaseTwo()) // final phase
        {
            _healthBar.value = math.remap(0, _majorPhaseThreshold, 0, 1, _boss.BossHealth);
        }
        else // first false health bar
        {
            _healthBar.value = math.remap(_majorPhaseThreshold, _boss.MaxBossHealth, 0, 1, _boss.BossHealth);
        }
    }
}
