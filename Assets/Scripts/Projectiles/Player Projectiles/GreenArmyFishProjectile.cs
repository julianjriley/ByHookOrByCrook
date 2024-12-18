using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreenArmyFishProjectile : Projectile
{
    private SpriteRenderer _spriteRenderer;
    private Collider _collider;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject explosionEffect;
    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<Collider>();

    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile"))
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(_damage, false);
        }


        if (_health <= 0)
        {
            Destroy(gameObject, 1f);
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            //Debug.Log("gotHere");
        }
        Instantiate(explosionEffect,transform.position, Quaternion.identity); 
        Collider[] colliders;
        colliders = Physics.OverlapSphere(gameObject.transform.position, 4, enemyMask, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile"))
            {
                col.gameObject.GetComponent<IDamageable>().TakeDamage(_damage, false);
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                col.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
            }

        }

    }

}
