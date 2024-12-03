using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool _overHeated;
    [SerializeField] protected bool _canFire = true;
    protected SpriteRenderer spriteRenderer;
    protected Animator _animator;
    PlayerCombat _player;

    [SerializeField] protected GameObject _projectile;

    public float mult;

    public static event Action WeaponOverheated;
    public static event Action WeaponCooledOff;

    //Buff Specific Variables
    float lookAngle;

    //Constantly retrieved from the player
    protected Vector3 _direction;

    //You know
    protected float _heatLevel;

    //Used for auto firing weapons
    protected Coroutine _autoFireCoroutine = null;

    protected virtual void Start()
    {

        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _player = _weapon.GetPlayer();
        mult = 1.0f;
        WeaponCooledOff += SubtractOverheatMult;
        WeaponOverheated += AddOverheatMult;

    }

    void OnDisable()
    {
        WeaponCooledOff -= SubtractOverheatMult;
        WeaponOverheated -= AddOverheatMult;
    }

    public virtual void Fire(Vector3 direction)
    {

        BulletHandling(direction);
        _animator.Play("Fire");
        OverheatAndResetting();

    }

    protected bool FireBoolChecks()
    {
        if (!_canFire)
        {
            return false;
        }

        if (_overHeated)
        {
            return false;
        }
        else
            return true;
    }

    protected void BulletHandling(Vector3 direction)
    {
        if (_weapon.ProjectileCount < 2)
        {
            BulletLogic(direction);
            _heatLevel += _weapon.HeatBuildup;
        }
        else
        {
            TripleShot();
        }
    }

    protected virtual void BulletLogic(Vector3 direction)
    {
        for (int i = 0; i < _weapon.ProjectileCount; i++)
        {
            GameObject projectile = Instantiate(_projectile, _firePoint.position, Quaternion.FromToRotation(Vector3.up, _direction));
            projectile.transform.localScale = new Vector3(projectile.transform.localScale.x * _weapon.Size, projectile.transform.localScale.y * _weapon.Size, 1);
            projectile.GetComponent<Rigidbody>().AddForce(_direction * _weapon.Speed, ForceMode.Impulse);
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.AssignStats(_weapon);
            projectileComponent.ReassignDamage(CheckOverheat() * _weapon.Damage * mult);
        }
    }

    protected virtual void TripleShot()
    {
        for (int i = -1; i < _weapon.ProjectileCount - 1; i++)
        {
            Vector3 aimingDir = Quaternion.Euler(0, 0, 8 * i) * _direction;
            BulletLogic(aimingDir);
        }
        _heatLevel += _weapon.HeatBuildup;
    }

    protected virtual void OverheatAndResetting()
    {
        TryApplyRecoil();
        if (_heatLevel >= 100)
        {
            _overHeated = true;
        }
        SoundManager.Instance.PlayOneShot(_weapon.FireSound, gameObject.transform.position);
        StartCoroutine(FireRate());
        _autoFireCoroutine = StartCoroutine(FireAuto(_direction));
    }

    public virtual void CeaseFire()
    {
        if(_autoFireCoroutine != null)
            StopCoroutine(_autoFireCoroutine);
    }

    protected virtual IEnumerator FireRate()
    {
        _canFire = false;
        yield return new WaitForSeconds(1/_weapon.FireRate);
        _canFire = true;
    }

    protected IEnumerator FireAuto(Vector3 direction)
    {
        while (!_canFire || _overHeated)
        {
            yield return null;
        }

        Fire(direction);
    }

    protected virtual void Update()
    {
        if (_overHeated == true && _heatLevel >= 100)
        {
            WeaponOverheated?.Invoke();
            _weapon.CoolingTime *= _weapon.OverheatCoolingSpeedMultiplier;
            _heatLevel = 99;
        }
        _heatLevel = Mathf.Clamp(_heatLevel - _weapon.CoolingTime * Time.deltaTime, 0, 100);

        if (_heatLevel <= 0)
        {
            if (_overHeated == true)
            {
                WeaponCooledOff?.Invoke();
                _weapon.CoolingTime /= _weapon.OverheatCoolingSpeedMultiplier;
            }
            _overHeated = false;
        }
    }


    protected virtual void FixedUpdate()
    {
        
        if (_direction != null && _direction != Vector3.zero)
            lookAngle = Vector3.Angle(Vector3.up, _direction);
        
    }

    public void UpdateRotation(Vector2 lookAt)
    {

        Debug.Log(lookAngle);
    }

    public void SetAim(Vector3 direction)
    {
        _direction = direction;
    }

    public virtual void DisableRendering()
    {
        spriteRenderer.enabled = false;
    }

    public virtual void EnableRendering()
    {
        spriteRenderer.enabled = true;
    }

    public float GetHeatLevel()
    {
        return _heatLevel;
    }

    public void SetHeatLevel(float heatLevel)
    {
        _heatLevel = heatLevel;
    }

    public Weapon GetWeapon()
    {
        return _weapon;
    }

    public bool GetOverHeatedState()
    {
        return _overHeated;
    }

    public void SetOverHeatedState(bool overHeated)
    {
        _overHeated = overHeated;
    }


    protected void TryApplyRecoil()
    {
        if (!_weapon.canRecoil)
            return;
        if(lookAngle > 150 && lookAngle < 210)
        {
            _player.ApplyRecoil(_weapon.RecoilAmount);
        }
    }

    protected float CheckOverheat()
    {
        if (_weapon.overheatShot && (_heatLevel + _weapon.HeatBuildup) >= 100)
            return 4f;
        else
            return 1f;

    }

    void AddOverheatMult()
    {
        if (_weapon.overheatDamageBonus)
            mult += 0.2f;
    }

    void SubtractOverheatMult()
    {
        if (_weapon.overheatDamageBonus)
            mult -= 0.2f;
    }

}
