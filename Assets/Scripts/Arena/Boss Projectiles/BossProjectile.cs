using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossProjectile : MonoBehaviour
{
    [SerializeField, Tooltip("Projectile's rigidbody used for motion scripting.")]
    protected Rigidbody _rb;
    [SerializeField, Tooltip("maximum duration that the projectile can exist.")]
    private float _lifeTime;
    [SerializeField, Tooltip("Remaining amount of damage that the projectile can take from player projectiles before it is destroyed.")]
    private float _health;

    virtual protected void Awake()
    {
        // destroy after lifetime expires
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: handle damaging of boss projectiles (when contacting player projectiles / shields)
        /*  if (collision.CompareTag("PlayerProjectile")
         *  {
         *      _health -= collision.GetComponent<Projectile>.damage;
         *      Destroy(collision);
         *      
         *      if(_health < 0)
         *          Destroy(gameObject);
         *  }
         */

        // TODO: handle destroying of boss projectile and application of damage when it hits the player
        /*
         * if (collision.CompareTag("Player")
         * {
         *      collision.GetComponent<PlayerHealth>().ApplyDamage(_damage);
         *      
         *      Destroy(gameObject);
         * }
         */
    }
}
