using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaLeechProjectile : Projectile
{
    [SerializeField, Tooltip("Poison repeat time")]
    private float _repeatTime;
    private float _damageDealt = 0;
    private float _maxDamage = 10;
    private Collision _bossCollision;
    private Collider _bossCollider;

    //The two collision functions are exactly the same its just safety honestly (some projectiles are triggers and others aren't)

    protected virtual new void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            _bossCollider = collider;
            Debug.Log("invoke poison");
            InvokeRepeating("ApplyPoison", 0, _repeatTime);
            Debug.Log("repeat time - " + _repeatTime);
           // Destroy(gameObject);
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
            Debug.Log("invoke poison");
            InvokeRepeating("ApplyPoisonCollision", 0, _repeatTime);
            //Destroy(gameObject);
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
        Debug.Log("Applying poison");
        if (_damageDealt <= _maxDamage)
        {
            _damageDealt += _damage;
            Debug.Log("Damage Dealt" + _damageDealt);
            _bossCollider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
        }
    }
     void ApplyPoisonCollision()
    {
        Debug.Log("Applying poison");

        if (_damageDealt <= _maxDamage)
        {
            _damageDealt += _damage;
            Debug.Log("Damage Dealt" + _damageDealt);
            _bossCollision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
        }
    }
}
