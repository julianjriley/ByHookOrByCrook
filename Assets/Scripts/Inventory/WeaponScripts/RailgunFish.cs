using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunFish : WeaponInstance
{
    [SerializeField] GameObject _projectile;

    protected override void Start()
    {
        base.Start();
    }

    public override void Fire(Vector3 direction)
    {
        if (!_canFire)
            return;
        if (_overHeated)
        {
            // Modify later
            return;
        }
        if (_weapon.ProjectileCount < 2)
        {
            for (int i = 0; i < _weapon.ProjectileCount; i++)
            {
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
                projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
                RailgunFishProjectile railgunFishProjectile = projectile.GetComponent<RailgunFishProjectile>();
                railgunFishProjectile.AssignStats(_weapon);
                _heatLevel += _weapon.HeatBuildup;
            }
        }
        else
        {
            for (int i = -1; i < _weapon.ProjectileCount - 1; i++)
            {
                Vector3 aimingDir = Quaternion.Euler(0, 0, 8 * i) * _direction;
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, aimingDir));
                projectile.GetComponent<Rigidbody>().AddForce(aimingDir * _weapon.Speed, ForceMode.Impulse);
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
                RailgunFishProjectile railgunFishProjectile = projectile.GetComponent<RailgunFishProjectile>();
                railgunFishProjectile.AssignStats(_weapon);
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
