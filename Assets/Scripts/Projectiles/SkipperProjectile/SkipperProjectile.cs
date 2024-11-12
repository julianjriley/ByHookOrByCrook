using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipperProjectile : Projectile
{
    public override void AssignStats(Weapon weapon)
    {
        _damage = weapon.Damage;
        _baseDamage = _damage;
        _size = weapon.Size;
        _speed = weapon.Speed;
        _lifetime = weapon.Lifetime;
    }
}
