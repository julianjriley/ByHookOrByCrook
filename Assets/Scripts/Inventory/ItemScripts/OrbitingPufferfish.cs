using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingPufferfish : PassiveItemInstance
{
    private float _damage;
    private float _waitTime;
    private float _speed;

    private GameObject _pivot;

    [SerializeField]
    GameObject prefab;
    public override void ItemEffect()
    {
        base.ItemEffect();

        _pivot = _player.gameObject;

        // Spawn another orbiting pufferfish
        Instantiate(prefab, _pivot.transform);  
    }

    private void Update()
    {
        // Orbiting around the player
        transform.RotateAround(_pivot.transform.position, new Vector3(0, 1, 0), _speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);

            // Trigger cooldown & play deflated sprite
            new WaitForSeconds(_waitTime);
            
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            // projectile should die?

            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);

            // Trigger cooldown & play deflated sprite
            new WaitForSeconds(_waitTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
           
            // Trigger cooldown & play deflated sprite
            new WaitForSeconds(_waitTime);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            // projectile should die?

            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);

            // Trigger cooldown & play deflated sprite
            new WaitForSeconds(_waitTime);
        }
    }
}
