using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam : Projectile
{
    [SerializeField]
    //private Transform _pivotPoint;
    private float _zForce = 360f;
    private Animator _animator;
    private Collider _collider;

    override protected void Start()
    {
        base.Start();
        Color color = GetComponent<SpriteRenderer>().color;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        StartCoroutine(StartGimmick());
    }

    IEnumerator StartGimmick()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(2f);
        _collider.enabled = true;
        _animator.Play("ChangeLaserColor");
        yield return new WaitForSeconds(2f);
        _collider.enabled = false;
        _animator.Play("DefaultLaser");

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
