using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _size;
    private float _speed;
    private float _lifetime;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    public void AssignStats(Weapon weapon)
    {
        _damage = weapon.Damage;
        _size = weapon.Size;
        _speed = weapon.Speed; 
        _lifetime = weapon.Lifetime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        Destroy(gameObject);
    }
}
