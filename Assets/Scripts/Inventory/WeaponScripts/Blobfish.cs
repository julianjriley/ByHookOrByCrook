using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class Blobfish : WeaponInstance
{
    [SerializeField] private GameObject _projectile;
    private Animator _animator;
    private float _tickTimer = 9f;
    private bool _transformed = false;
    

    protected override void Start()
    {
        base.Start();
        //_animator = GetComponent<Animator>();
    }
    public override void Fire(Vector3 direction)
    {
        if (!_canFire)
        {
            return;
        }

        if (_overHeated)
        {
            return;
        }

        if (!_transformed)
            return;
        if (_weapon.ProjectileCount < 2)
        {
            CheckOverheat();
            _weapon.Damage *= mult;
            for (int i = 0; i < _weapon.ProjectileCount; i++)
            {
                GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
                projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
                BlobfishProjectile blobfishProjectile = projectile.GetComponent<BlobfishProjectile>();
                blobfishProjectile.AssignStats(_weapon);
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
                BlobfishProjectile blobfishProjectile = projectile.GetComponent<BlobfishProjectile>();
                blobfishProjectile.AssignStats(_weapon);
                blobfishProjectile.UseTripleShotDamage();
            }
            _weapon.Damage /= mult;
            _heatLevel += _weapon.HeatBuildup;
        }
        TryApplyRecoil();
        if (_heatLevel >= 100)
        {
            if (_weapon.overheatShot)
                _weapon.Damage /= 10f;
            _overHeated = true;
        }
        SoundManager.Instance.PlayOneShot(_weapon.FireSound, gameObject.transform.position);
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }

    protected override void Update()
    {
        base.Update();
        if (_transformed || !spriteRenderer.enabled)
            return;
        if (_tickTimer < 0)
        {
            _transformed = true;
        }
        else
            _tickTimer -= Time.deltaTime;
        Debug.Log(_tickTimer);
    }

    

    private void BecomeTheAbsoluteGigaMegaNuke3000()
    {
        _animator.Play("Transform");
    }


}
