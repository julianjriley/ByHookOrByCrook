using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool overHeated;
    [SerializeField] protected bool canFire;

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
    /*
    protected IEnumerator CoolingDown()
    {
        overHeated = true;
        yield return new WaitForSeconds(_weapon.
        overHeated = false;
    }
    */
}
