using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles setting constant speed of PixelInk projectile.
/// </summary>
public class PixelInk : Projectile
{
    override protected void Start()
    {
        base.Start();

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
