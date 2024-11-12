using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam : Projectile
{
    private float _zForce = 360f;
    private Animator _animator;
    private Collider _collider;

    override protected void Start()
    {
        base.Start();
        Color color = GetComponent<SpriteRenderer>().color;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        // Lasers turn "red" and damage the player. Yellow lasers are safe. Animation color change might be a little off.
        // TODO: Replace laser sprite 
        StartCoroutine(StartGimmick());
    }
    IEnumerator StartGimmick()
    {
        InvokeRepeating("Gimmick", 0f, 5f);
        yield return new WaitForSeconds(0f);
    }

    void Gimmick()
    {
        _collider.enabled = true;
        _animator.Play("ChangeLaserColor");

        StartCoroutine(ResetColliders());
    }

    IEnumerator ResetColliders()
    {
        yield return new WaitForSeconds(2.5f);
        _collider.enabled = false;
        _animator.Play("DefaultLaser");
    }
    override protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            // Colliding with the player does not destroy the laser
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
        }

    }

    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            // Colliding with the player does not destroy the laser
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
