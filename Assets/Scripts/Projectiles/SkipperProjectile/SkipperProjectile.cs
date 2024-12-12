using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipperProjectile : Projectile
{
    public override void AssignStats(Weapon weapon)
    {
        // skipper damage must account for assist mode multiplier
        _damage = weapon.Damage * GameManager.Instance.GamePersistent.SkipperMultiplier;
        _baseDamage = _damage;
        _size = weapon.Size;
        _speed = weapon.Speed;
        _lifetime = weapon.Lifetime;
    }
}
