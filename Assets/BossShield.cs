using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour, IDamageable
{
    ParticleSystem _particleSystem;
    Collider _theCollider;
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _theCollider = GetComponent<Collider>();
    }

    public void PassEffect(EffectData effect)
    {
        
    }

    public void TakeDamage(float damage, bool dontUseSound = false)
    {

    }

    public void StartPlaying()
    {
        _particleSystem.Play();
        _theCollider.enabled = true;
    }

    public void StopPlaying()
    {
        _particleSystem.Stop();
        _theCollider.enabled = false;
    }


}
