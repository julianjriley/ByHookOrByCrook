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
        base.OnCollisionEnter(collision);
    }

    override protected void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }
}
