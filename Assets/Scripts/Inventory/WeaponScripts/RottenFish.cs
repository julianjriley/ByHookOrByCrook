using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenFish : WeaponInstance 
{
    [SerializeField] GameObject _projectile;
    [SerializeField] EventReference Cheating;

    public override void Fire(Vector3 direction)
    {
        if(!_canFire) 
            return;
        if(_overHeated)
        {
            //Modify later if we wanna do cool stuff to the gun while overheated idk
            return;
        }
        if(_weapon.ProjectileCount < 2)
        {
            for (int i = 0; i < _weapon.ProjectileCount; i++)
            {
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
                projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
                RottenFishProjectile rottenFishProjectile = projectile.GetComponent<RottenFishProjectile>();
                rottenFishProjectile.AssignStats(_weapon);
                _heatLevel += _weapon.HeatBuildup;
            }
        }
        else
        {
            
            for (int i = -1; i < _weapon.ProjectileCount - 1; i++)
            {
                Vector3 aimingDir = Quaternion.Euler(0,0, 8 * i) * _direction;
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, aimingDir));
                projectile.GetComponent<Rigidbody>().AddForce(aimingDir * _weapon.Speed, ForceMode.Impulse);
                RottenFishProjectile rottenFishProjectile = projectile.GetComponent<RottenFishProjectile>();
                rottenFishProjectile.AssignStats(_weapon);
            }
            _heatLevel += _weapon.HeatBuildup;
        }

        if (_heatLevel >= 100)
            _overHeated = true;
        SoundManager.Instance.PlayOneShot(_weapon.FireSound, gameObject.transform.position);
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }


}
