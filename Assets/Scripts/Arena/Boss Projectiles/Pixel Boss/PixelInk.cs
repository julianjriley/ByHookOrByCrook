using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelInk : BossProjectile
{
    [SerializeField, Tooltip("Constant speed projectile travels at.")]
    private float _speed;

    override protected void Awake()
    {
        base.Awake();

        // ensure gravity is disabled
        _rb.useGravity = false;

        // set initial speed (in appropriate direction)
        _rb.velocity = transform.right * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        // no changes necessary in update
    }
}
