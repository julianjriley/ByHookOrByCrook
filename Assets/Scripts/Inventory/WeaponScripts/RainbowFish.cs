using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowFish : WeaponInstance
{
    [SerializeField] GameObject _projectile;

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
        if (_weapon.ProjectileCount < 2)
        {
            CheckOverheat();
            _weapon.Damage *= mult;
            for (int i = 0; i < _weapon.ProjectileCount; i++)
            {
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
                projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
                RottenFishProjectile rottenFishProjectile = projectile.GetComponent<RottenFishProjectile>();
                rottenFishProjectile.AssignStats(_weapon);
                _heatLevel += _weapon.HeatBuildup;
            }
            _weapon.Damage /= mult;
        }
        else
        {
            CheckOverheat();
            _weapon.Damage *= mult;
            for (int i = -1; i < _weapon.ProjectileCount - 1; i++)
            {
                Vector3 aimingDir = Quaternion.Euler(0, 0, 8 * i) * _direction;
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, aimingDir));
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
                projectile.GetComponent<Rigidbody>().AddForce(aimingDir * _weapon.Speed, ForceMode.Impulse);
                RottenFishProjectile rottenFishProjectile = projectile.GetComponent<RottenFishProjectile>();
                rottenFishProjectile.AssignStats(_weapon);
            }
            _weapon.Damage /= mult;
            _heatLevel += _weapon.HeatBuildup;
        }

        if (_heatLevel >= 100)
        {
            if (_weapon.overheatShot)
                _weapon.Damage /= 10f;
            _overHeated = true;
            increasedFireRate = 0;
        }
        _animator.Play("Fire");
        TryApplyRecoil(); 
        SoundManager.Instance.PlayOneShot(_weapon.FireSound, gameObject.transform.position);
        increasedFireRate += 0.3f;
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
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
