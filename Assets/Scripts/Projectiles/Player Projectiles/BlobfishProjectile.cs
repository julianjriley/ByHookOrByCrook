using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobfishProjectile : Projectile
{
    private float _damageMod = 0.6f;
    [SerializeField] private GameObject _flashEffect;
    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BossPrototype>(out BossPrototype component))
        {
            _damage = component.MaxBossHealth * _damageMod;
            component.TakeDamage(_damage, false);
            InstantiateDeathEffect(0.7f);
            FlashEffect();
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<BossPrototype>(out BossPrototype component))
        {
            _damage = component.MaxBossHealth * _damageMod;
            component.TakeDamage(_damage, false);
            InstantiateDeathEffect(0.7f);
            FlashEffect();
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

    void FlashEffect()
    {
        GameObject flashEffect = Instantiate(_flashEffect, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(flashEffect, 2f);
    }
}
