using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnapesticTetra : WeaponInstance
{
    [SerializeField] GameObject[] _projectiles;
    [SerializeField] float[] _speedValues;
    private int _projectileIndex = 0;



    public override void Fire(Vector3 direction)
    {
        FireBoolChecks();
        BulletHandling(direction);
        _animator.Play("Fire");
        OverheatAndResetting();
        if (_projectileIndex >= _projectiles.Length - 1)
            _projectileIndex = 0;
        else
            _projectileIndex += 1;
    }

    protected override void BulletLogic(Vector3 direction)
    {
        GameObject projectile = Instantiate(_projectiles[_projectileIndex], _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
        projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
        projectile.GetComponent<Rigidbody>().AddForce(_direction * _speedValues[_projectileIndex], ForceMode.Impulse);
        AnapesticTetraProjectile anapesticTetraProjectile = projectile.GetComponent<AnapesticTetraProjectile>();
        anapesticTetraProjectile.AssignStats(_weapon);
        anapesticTetraProjectile.ReassignDamage(CheckOverheat() * _weapon.Damage * mult);
    }
}
