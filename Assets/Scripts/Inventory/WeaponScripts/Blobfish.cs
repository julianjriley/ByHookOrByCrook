using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class Blobfish : WeaponInstance
{
    [SerializeField] private GameObject _projectile;
    private float _tickTimer = 90f;
    private bool _transformed = false;
    private bool _tickTimerEnabled = true;
    [SerializeField] private EventReference _blobSound;
    private EventInstance blobSound;
    private float soundOn;

    protected override void Start()
    {
        base.Start();
        //_animator = GetComponent<Animator>();
        blobSound = SoundManager.Instance.CreateInstance(_blobSound);
        blobSound.start();
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
        _animator.SetBool("Fire", true);
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
        SoundManager.Instance.SetParameter(blobSound, "BlobLevel", 0);
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }

    protected override void Update()
    {
        base.Update();
        if (!_tickTimerEnabled || !spriteRenderer.enabled)
        {
            blobSound.getParameterByName("IsActive", out soundOn);
            if (soundOn == 1 && _tickTimerEnabled)
            {
                SoundManager.Instance.SetParameter(blobSound, "IsActive", 0);
            }
            return;
        }
        if (_tickTimer < 0)
        {
            _tickTimerEnabled = false;
            StartCoroutine(BecomeTheAbsoluteGigaMegaNuke3000());
        }
        else
        {
            _tickTimer -= Time.deltaTime;
            _animator.SetFloat("TimeLeft", _tickTimer);
        }
        //sound stuff
        if (_tickTimer <= 60 && _tickTimer > 30)
        {
            SoundManager.Instance.SetParameter(blobSound, "BlobLevel", 1);
        }
        if (_tickTimer <= 30 && _tickTimer > 0)
        {
            SoundManager.Instance.SetParameter(blobSound, "BlobLevel", 2);
        }
        blobSound.getParameterByName("IsActive", out soundOn);
        if (soundOn == 0)
        {
            SoundManager.Instance.SetParameter(blobSound, "IsActive", 1);
        }
    }

    

    private IEnumerator BecomeTheAbsoluteGigaMegaNuke3000()
    {
        _animator.SetBool("Transform", true);
        SoundManager.Instance.SetParameter(blobSound, "BlobLevel", 3);
        yield return new WaitForSeconds(0.5f);
        _transformed = true;
    }


}
