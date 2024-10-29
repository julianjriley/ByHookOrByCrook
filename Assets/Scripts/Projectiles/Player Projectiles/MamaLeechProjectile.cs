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
    
    //The two collision functions and apply poision functions are exactly the same

    protected virtual new void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            _bossCollider = collider;
            InvokeRepeating("ApplyPoison", 0, _repeatTime);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
        }

    }
    protected virtual new void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            _bossCollision = collision;
            InvokeRepeating("ApplyPoisonCollision", 0, _repeatTime);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
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
