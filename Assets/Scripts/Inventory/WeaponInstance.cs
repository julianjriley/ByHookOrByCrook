using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool overHeated;
    [SerializeField] protected bool canFire = true;


    protected float _heatLevel;
    

    public virtual void Fire(Vector3 direction)
    {

    }

    protected IEnumerator FireRate()
    {
        canFire = false;
        yield return new WaitForSeconds(_weapon.FireRate);
        canFire = true;
    }

    protected virtual void FixedUpdate()
    {
        _heatLevel = Mathf.Clamp(_heatLevel - _weapon.CoolingTime * Time.deltaTime, 0, 100);
        
        if (_heatLevel <= 0)
            overHeated = false;
 
    }

    public void UpdateRotation(Vector2 lookAt)
    {
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, lookAt);
    }


}
