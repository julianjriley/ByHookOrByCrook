using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipperGlock : WeaponInstance
{
    [SerializeField] GameObject _projectile;
    public override void Fire(Vector3 direction)
    {
        for (int i = 0; i < _weapon.ProjectileCount; i++)
        {
            GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
            projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
            projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
            SkipperProjectile skipperProjectile = projectile.GetComponent<SkipperProjectile>();
            skipperProjectile.AssignStats(_weapon);
            _heatLevel += _weapon.HeatBuildup;
        }
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }
}
