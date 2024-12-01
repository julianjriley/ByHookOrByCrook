using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunFishProjectile : Projectile
{

    public override void TakeDamage(float damage, bool dontUseSound = false)
    {
        
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamage(2, false);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage, false);
        }
        InstantiateDeathEffect();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(4, false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage, false);
        }
        InstantiateDeathEffect();
    }
}
