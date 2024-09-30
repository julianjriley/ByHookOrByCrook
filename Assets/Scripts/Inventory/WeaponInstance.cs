using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool _overHeated;
    [SerializeField] protected bool _canFire = true;
    protected SpriteRenderer spriteRenderer; 

    //Constantly retrieved from the player
    protected Vector3 _direction;

    //You know
    protected float _heatLevel;

    //Used for auto firing weapons
    protected Coroutine _autoFireCoroutine;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        _heatLevel = Mathf.Clamp(_heatLevel - _weapon.CoolingTime * Time.deltaTime, 0, 100);
        
        if (_heatLevel <= 0)
            _overHeated = false;
 
    }

    public void UpdateRotation(Vector2 lookAt)
    {
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, lookAt);
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

}
