using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam : Projectile
{
    //private float _zForce = 360f;
    private Animator _animator;
    private Collider _collider;
    [Header("Beam Specifics")]
    [SerializeField] private float _rotSpeed = .7f;
    private float _dir = 1;

    override protected void Start()
    {
        base.Start();
        //Color color = GetComponent<SpriteRenderer>().color;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        // Lasers turn "red" and damage the player. Faded lasers with no core are safe
        StartCoroutine(StartGimmick());
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        this.transform.Rotate(0, 0, _rotSpeed * _dir);
    }

    public void SetDirAndSpeed(float dir, float speed=.7f)
    {
        _dir = dir;
        _rotSpeed = speed;
    }

    private IEnumerator StartGimmick()
    {
        InvokeRepeating("Gimmick", 0f, 5f);
        yield return new WaitForSeconds(0f);
    }

    void Gimmick()
    {
        StartCoroutine(DoLaserAction());
    }

    private IEnumerator DoLaserAction()
    {
        _collider.enabled = true;
        _animator.Play("LaserAppear");

        yield return new WaitForSeconds(2.5f);

        _collider.enabled = false;
        _animator.Play("LaserFade");
    }

    override protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerCombat>().TakeDamage(1, false);
            // Colliding with the player does not destroy the laser
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
            Destroy(gameObject);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collider.gameObject.GetComponent<Projectile>().TakeDamage(_damage, false);
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
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(1, false);
            // Colliding with the player does not destroy the laser
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage, false);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            collision.gameObject.GetComponent<Projectile>().TakeDamage(_damage, false);
        }

        if (_health <= 0)
            Destroy(gameObject);
    }
}
