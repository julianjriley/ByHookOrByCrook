using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaLeechProjectile : Projectile
{
    [SerializeField, Tooltip("How many times the poision repeats after hitting the boss")]
    private float _repeatTime;
    [SerializeField, Tooltip("Start time for second InvokeRepeating() call")]
    private float _startTime;
    private float _damageDealt = 0;
    private float _maxDamage = 10;
    private Collision _bossCollision;
    private Collider _bossCollider;
    //[SerializeField] GameObject _poisonEffect;
    [SerializeField] EffectData _effectData;

    [SerializeField, Tooltip("Sprite renderer to be disabled as if projectile was destroyed.")]
    private SpriteRenderer _renderer;

    //The two collision functions and apply poision functions are exactly the same
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(_damage);
            component.PassEffect(_effectData);
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player") || collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
                Destroy(gameObject);
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
        }

    }
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(_damage);
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
                Destroy(gameObject);
        }

        if (_health <= 0)
            Destroy(gameObject);

    }
    void ApplyPoison()
     {
        if (_damageDealt <= _maxDamage)
        {
            _damageDealt += _damage;
            _bossCollider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            InvokeRepeating("ApplyPoison", _startTime, _repeatTime);
        }
        else
        {
            Destroy(gameObject);
        }
        
     }
     void ApplyPoisonCollision()
     {
        if (_damageDealt <= _maxDamage)
        {
            _damageDealt += _damage;
            _bossCollision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            InvokeRepeating("ApplyPoisonCollision", _startTime, _repeatTime);
        }
        else
        {
            Destroy(gameObject);
        } 
    }
}
