using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

public class Sunfish : PassiveItemInstance
{

    WeaponInstance[] tempArray;
    LayerMask enemyProjectileMask;
    [SerializeField] GameObject hitEffect;

    bool shortRangeDamage = false;

    float mult = 1f;

    private void OnEnable()
    {
        EclipseStarfish.SunfishInteraction += EclipseStarfishDamageBonus;
        Piranha.SunfishInteraction += PiranhaDamageBonus;

    }
    private void OnDisable()
    {
        EclipseStarfish.SunfishInteraction -= EclipseStarfishDamageBonus;
        Piranha.SunfishInteraction -= PiranhaDamageBonus;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach (Item item in _player.GetInventory().items)
        {
            if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                weapon.CoolingTime = 0;
            }
        }
        enemyProjectileMask = LayerMask.GetMask("BreakableBossProjectile", "Boss");
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(HeatLevelOverride());
        InvokeRepeating("KillThings", 0f, 0.4f);
    }

    protected void FixedUpdate()
    {
        
    }

    void KillThings()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(gameObject.transform.position, 5, enemyProjectileMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
            {
                float distanceToPlayer;
                float closeRangeDamageMultiplier = 1;
                if (shortRangeDamage)
                {
                    distanceToPlayer = Mathf.Abs((_player.gameObject.transform.position - gameObject.transform.position).magnitude);
                    closeRangeDamageMultiplier = 1 * Mathf.Clamp(math.remap(0, 8, 2, 1, distanceToPlayer), 1, 2);
                }
                float damage = 2f * mult * closeRangeDamageMultiplier;
                
                if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile"))
                {
                    damage = 0.5f * mult * closeRangeDamageMultiplier;
                }
                target.TakeDamage(damage);
                GameObject effect = Instantiate(hitEffect, collider.transform.position, Quaternion.identity);
                Destroy(effect, 0.2f);
            }
        }
    }

    IEnumerator HeatLevelOverride()
    {
        yield return new WaitForSeconds(0.3f);
        tempArray = _player.GetWeaponsTransform().GetComponentsInChildren<WeaponInstance>();
        foreach (WeaponInstance weapon in tempArray)
        {
            weapon.SetOverHeatedState(true);
            weapon.SetHeatLevel(150);
        }
    }

    void EclipseStarfishDamageBonus()
    {
        int numGuns = 0;
        foreach (Item item in _player.GetInventory().items)
        {
            if (item is Weapon)
            {
                numGuns++;
            }
        }
        mult = 1 + (numGuns * 0.2f);
    }

    void PiranhaDamageBonus()
    {
        shortRangeDamage = true;
    }
}
