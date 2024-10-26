using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of damage projectile deals on contact.")]
    private float _damage;
    [SerializeField, Tooltip("Scale of projectile hitbox.")]
    private float _size;
    [SerializeField, Tooltip("Move speed of projectile")]
    protected float _speed;
    [SerializeField, Tooltip("Time until projectile is automatically destroyed")]
    private float _lifetime;
    [SerializeField, Tooltip("Amount of damage required to destroy the projectile")] 
    private float _health;

    protected Rigidbody _rb;

    virtual protected void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Destroy(gameObject, _lifetime);
    }

    public void AssignStats(Weapon weapon)
    {
        _damage = weapon.Damage;
        _size = weapon.Size;
        _speed = weapon.Speed; 
        _lifetime = weapon.Lifetime;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Destroy(gameObject);
        }
    }


    //The two collision functions are exactly the same its just safety honestly (some projectiles are triggers and others aren't)

    protected virtual void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
           Destroy(gameObject);
        }

        if(collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
           collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
           Destroy(gameObject);
        }

        if(collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
            //Debug.Log("gotHere");
        }
            
    }
    protected virtual void OnCollisionEnter(Collision collision)
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
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
            Destroy(gameObject);

    }
}
