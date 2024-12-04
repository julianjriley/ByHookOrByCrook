using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : Projectile
{
    private Vector3 _direction;

    protected override void Start()
    {
        base.Start();

        _direction.x = Vector3.right.x;
        _rb.velocity = _direction * _speed;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
