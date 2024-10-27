using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaLeechProjectile : Projectile
{
    [SerializeField, Tooltip("Poison repeat time")]
    private float _repeatTime;
    private float _damage;
    private float _health;
    private float _damageDealt;
    private float _maxDamage;

    //The two collision functions are exactly the same its just safety honestly (some projectiles are triggers and others aren't)

    protected virtual new void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            StartCoroutine(ApplyPoison(collider));
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
            //Debug.Log("gotHere");
        }

    }
    protected virtual new void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            StartCoroutine(ApplyPoison(collision));
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
            Destroy(gameObject);

    }

    IEnumerator ApplyPoison(Collider collision)
    {
        while (_damageDealt < _maxDamage)
        {
            _damageDealt += _damage;
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            yield return new WaitForSeconds(_repeatTime);
        }
    }
    IEnumerator ApplyPoison(Collision collision)
    {
        while (_damageDealt < _maxDamage)
        {
            _damageDealt += _damage;
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            yield return new WaitForSeconds(_repeatTime);
        }
    }
}
