using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LLExplosion : Projectile
{
    override protected void Start()
    {
        base.Start();
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(_damage, false);


        }
    }

    override protected void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(_damage, false);


        }
    }
}
