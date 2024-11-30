using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobfishProjectile : Projectile
{
    private float _damageMod = 0.6f;
    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BossPrototype>(out BossPrototype component))
        {
            _damage = component.MaxBossHealth * _damageMod;
            component.TakeDamage(_damage, false);
            PlayDeathEffect();
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<BossPrototype>(out BossPrototype component))
        {
            _damage = component.MaxBossHealth * _damageMod;
            component.TakeDamage(_damage, false);
            PlayDeathEffect();
            Destroy(gameObject);
        }
    }

    public void UseTripleShotDamage()
    {
        _damageMod = 0.267f;
    }

    public override void AssignStats(Weapon weapon)
    {
        _size = weapon.Size;
        _speed = weapon.Speed;
        _lifetime = weapon.Lifetime;
        _playerCombat = weapon.GetPlayer();
    }
}
