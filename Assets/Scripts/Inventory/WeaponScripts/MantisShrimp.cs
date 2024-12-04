using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisShrimp : WeaponInstance
{
    [SerializeField] GameObject _projectile;

    protected override void Start()
    {
        base.Start();
        _weapon.FireRate *= _weapon.ProjectileCount;
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


        // Fire function is the same for both triple shot and regular shots
        //CheckOverheat();
        int shotDirectionMod = Random.Range(0, 5);
        Vector3 aimingDir = Quaternion.Euler(0, 0, 12 * (2 - shotDirectionMod)) * _direction;
        GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, aimingDir));
        projectile.GetComponent<Rigidbody>().AddForce(aimingDir * _weapon.Speed, ForceMode.Impulse);
        projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
        MantisShrimpProjectile mantisShrimpProjectile = projectile.GetComponent<MantisShrimpProjectile>();
        mantisShrimpProjectile.AssignStats(_weapon);
        mantisShrimpProjectile.ReassignDamage(CheckOverheat() * _weapon.Damage * mult);
        _heatLevel += _weapon.HeatBuildup;
        //CheckOverheat();
        _animator.Play("Fire");
        if (_heatLevel >= 100)
            _overHeated = true;
        SoundManager.Instance.PlayOneShot(_weapon.FireSound, gameObject.transform.position);
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }
}
