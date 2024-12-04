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
        GameManager.Instance.GamePersistent.IsInvulnerable = true;
    }
}
