using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCleanerfish : PassiveItemInstance
{
    LayerMask enemyProjectileMask;
    ParticleSystem effect;
    public override void ItemEffect()
    {
        base.ItemEffect();
        effect = GetComponent<ParticleSystem>();
        enemyProjectileMask = LayerMask.GetMask("BreakableBossProjectile", "BossProjectile");
        WeaponInstance.WeaponOverheated += DeleteProjectiles;
    }


    
    void DeleteProjectiles()
    {
        effect.Play();
        Collider[] colliders;
        colliders = Physics.OverlapSphere(gameObject.transform.position, 7, enemyProjectileMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }

    private void OnDisable()
    {
        WeaponInstance.WeaponOverheated -= DeleteProjectiles;
    }
}
