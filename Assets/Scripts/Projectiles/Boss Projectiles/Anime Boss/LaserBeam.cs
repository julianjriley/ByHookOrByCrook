using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam : Projectile
{
    [SerializeField]
    private GameObject _pivotPoint;
    private float _zForce = 360f;
    override protected void Start()
    {
        base.Start();
        // TODO: When Spawned, set pivot to bosss and SPIN
        transform.position = _pivotPoint.GetComponent<Transform>().position;
        _rb.AddTorque(new Vector3(0, 0, _zForce), ForceMode.Impulse);
    }

    override protected void OnTriggerEnter(Collider collider)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
            //Debug.Log("gotHere");
        }

    }

    override protected void OnCollisionEnter(Collision collision)
    {
        //DO DAMAGE CODE HERE
        //Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage);
        }

        if (_health <= 0)
            Destroy(gameObject);

    }
}
