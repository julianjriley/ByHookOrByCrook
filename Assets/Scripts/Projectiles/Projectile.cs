using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of damage projectile deals on contact.")]
    protected float _damage;
    [SerializeField, Tooltip("Amount of damage projectile deals pre-buffs")]
    protected float _baseDamage;
    [SerializeField, Tooltip("Scale of projectile hitbox.")]
    protected float _size;
    [SerializeField, Tooltip("Move speed of projectile")]
    protected float _speed;
    [SerializeField, Tooltip("Time until projectile is automatically destroyed")]
    protected float _lifetime;
    [SerializeField, Tooltip("Amount of damage required to destroy the projectile")] 
    protected float _health;

    protected Rigidbody _rb;
    protected PlayerCombat _playerCombat;

    bool shortRangeDamage;
    float distanceToPlayer;


    virtual protected void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        // allows negative lifetime projectiles to have indefinite lifetime (useful on painterly boss)
        if(_lifetime > 0)
            Destroy(gameObject, _lifetime);
    }

    public  virtual void AssignStats(Weapon weapon)
    {
        _damage = weapon.Damage;
        _baseDamage = _damage;
        _size = weapon.Size;
        _speed = weapon.Speed; 
        _lifetime = weapon.Lifetime;
        _playerCombat = weapon.GetPlayer();
        shortRangeDamage = _playerCombat.useShortRangeDamage;
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    virtual protected void FixedUpdate()
    {
        if(_playerCombat != null)
        {
            if(shortRangeDamage)
            {
                distanceToPlayer = Mathf.Abs((_playerCombat.gameObject.transform.position - gameObject.transform.position).magnitude);
                _damage = _baseDamage * Mathf.Clamp(math.remap(0, 8, 2, 1, distanceToPlayer), 1, 2);
            }
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
