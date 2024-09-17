using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    [SerializeField] private Transform _firePoint;

    public virtual void Fire(Vector3 direction)
    {

    }
}
