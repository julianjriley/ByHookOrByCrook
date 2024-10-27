using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaLeech : WeaponInstance
{
    // damage the boss over time

    [SerializeField] GameObject _projectile;

    float waitTime = 0.5f;

    protected override void Start()
    {
        base.Start();
    }

    public override void Fire(Vector3 direction)
    {
        if (_canFire)
        {
            base.Fire(direction);
        }
    }

    protected override IEnumerator FireRate()
    {
        // slow rate of fire

        _canFire = false;
        yield return new WaitForSeconds(waitTime);
        _canFire = true;
    }
}
