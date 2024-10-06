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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        // TODO: handle damaging of boss projectiles (when contacting player projectiles / shields)
        /*  if (collision.layer == BossProjectile)
         *  {
         *      if (collision.CompareTag("PlayerProjectile")
         *      {
         *          _health -= collision.GetComponent<Projectile>.damage;
         *          Destroy(collision);
         *      
         *          if(_health < 0)
         *             Destroy(gameObject);
         *      }
         *      if (collision.CompareTag("Player")
         *      {
         *          collision.GetComponent<PlayerHealth>().ApplyDamage(_damage);
         *      
         *          Destroy(gameObject);
         *      }
         *  }
         */
    }
}
