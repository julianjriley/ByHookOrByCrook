using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool _overHeated;
    [SerializeField] protected bool _canFire = true;
    protected SpriteRenderer spriteRenderer;

    PlayerCombat _player;

    

    public static event Action WeaponOverheated;
    public static event Action WeaponCooledOff;

    //Buff Specific Variables
    float lookAngle;

    //Constantly retrieved from the player
    protected Vector3 _direction;

    //You know
    protected float _heatLevel;

    //Used for auto firing weapons
    protected Coroutine _autoFireCoroutine;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _player = _weapon.GetPlayer();
    }

    public virtual void Fire(Vector3 direction)
    {
        
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
        while (!_canFire)
            yield return null;
        Fire(direction);
    }

    protected virtual void FixedUpdate()
    {
        if(_overHeated == true && _heatLevel >= 100)
        {
            WeaponOverheated?.Invoke();
        }
        if (_direction != null && _direction != Vector3.zero)
            lookAngle = Vector3.Angle(Vector3.up, _direction);
        _heatLevel = Mathf.Clamp(_heatLevel - _weapon.CoolingTime * Time.deltaTime, 0, 100);
        


        if (_heatLevel <= 0)
        {
            if(_overHeated == true)
            {
                WeaponCooledOff?.Invoke();
            }
            _overHeated = false;
        }
            
 
    }

    public void UpdateRotation(Vector2 lookAt)
    {

        Debug.Log(lookAngle);
    }

    public void SetAim(Vector3 direction)
    {
        _direction = direction;
    }

    public void DisableRendering()
    {
        spriteRenderer.enabled = false;
    }

    public void EnableRendering()
    {
        spriteRenderer.enabled = true;
    }

    public float GetHeatLevel()
    {
        return _heatLevel;
    }

    public Weapon GetWeapon()
    {
        return _weapon;
    }

    public bool GetOverHeatedState()
    {
        return _overHeated;
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
}
