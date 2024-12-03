using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowFish : WeaponInstance
{

    float increasedFireRate;
    float fireRateCap;
    protected override void Start()
    {
        base.Start();
        fireRateCap = _weapon.FireRate * 3;
    }

    public override void Fire(Vector3 direction)
    {
        if (!_canFire)
        {
            increasedFireRate = 0;
            return;
        }
            
        if (_overHeated)
        {
            //Modify later if we wanna do cool stuff to the gun while overheated idk
            increasedFireRate = 0;
            return;
        }
        BulletHandling(direction);

        if (_heatLevel >= 100)
        {
            _overHeated = true;
            increasedFireRate = 0;
        }
        _animator.Play("Fire");
        OverheatAndResetting();
        increasedFireRate += 0.3f;
    }

    protected override IEnumerator FireRate()
    {
        _canFire = false;
        yield return new WaitForSeconds(1 / (_weapon.FireRate + increasedFireRate));
        _canFire = true;
    }

    public override void CeaseFire()
    {
        base.CeaseFire();
        increasedFireRate = 0;
    }
}
