using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenFish : WeaponInstance 
{
    [SerializeField] GameObject _projectile;

    public override void Fire(Vector3 direction)
    {
        if(overHeated)
        {
            //Modify later if we wanna do cool stuff to the gun while overheated idk
            return;
        }
        for (int i = 0; i < _weapon.ProjectileCount; i++)
        {
            GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, direction));
            projectile.GetComponent<Rigidbody>().AddForce(direction * _weapon.Speed, ForceMode.Impulse);
            RottenFishProjectile rottenFishProjectile = projectile.GetComponent<RottenFishProjectile>();
            rottenFishProjectile.AssignStats(_weapon);
            _heatLevel += _weapon.HeatBuildup;
        }
        if (_heatLevel >= 100)
            overHeated = true;

        StartCoroutine(FireRate());
    }
}
