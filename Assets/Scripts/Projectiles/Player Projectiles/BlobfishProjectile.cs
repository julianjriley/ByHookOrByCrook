using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobfishProjectile : Projectile
{
    private const float INDIRECT_DAMAGE = 100f;
    [SerializeField] LayerMask enemyProjectileMask;
    private float _damageMod = 0.6f;
    [SerializeField] private GameObject _flashEffect;
    protected override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BossPrototype>(out BossPrototype component))
        {
            _damage = component.MaxBossHealth * _damageMod;
            ScreenWipe();
            InstantiateDeathEffect(0.7f);
            FlashEffect();
            Destroy(gameObject);
        }

        // impact with breakable projectile
        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile"))
        {
            _damage = INDIRECT_DAMAGE; // constant damage instead of percent max health

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
            ScreenWipe();
            InstantiateDeathEffect(0.7f);
            FlashEffect();
            Destroy(gameObject);
        }

        // impact with breakable projectile
        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile"))
        {
            _damage = INDIRECT_DAMAGE; // constant damage instead of percent max health
            ScreenWipe();
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
        GameObject flashEffect = Instantiate(_flashEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(flashEffect, 2f);
    }

    void ScreenWipe()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(gameObject.transform.position, 60, enemyProjectileMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                collider.GetComponent<BossPrototype>().TakeDamage(_damage, false);
            }
            else
            {
                if (collider.TryGetComponent(out InvincibilityOrb orb))
                    orb.TakeDamage(1000);
                else
                    Destroy(collider.gameObject);
            }
        }
    }
}
