using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam : Projectile
{
    private Animator _animator;
    private Collider _collider;
    [Header("Beam Specifics")]
    [Tooltip("Rotation speed")]
    [SerializeField] private float _rotSpeed = .7f;
    [Tooltip("Rotation direction")]
    [SerializeField] private float _dir = 1;
    [Tooltip("Time portion of laser's cycle where it can damage player")]
    [SerializeField] private float _activeDuration = 2.5f;
    [Tooltip("Total time of laser cycle (nondamaging => damaging)")]
    [SerializeField] private float _cycleDuration = 5f;
    private float _downDuration;

    override protected void Start()
    {
        gameObject.AddComponent<EffectManager>();
        _rb = GetComponent<Rigidbody>();

        if (_lifetime > 0)
            Invoke("TimeUp", _lifetime);

        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        _downDuration = _cycleDuration - _activeDuration;

        _animator.Play("LaserSpawn");

        // Lasers turn "red" and damage the player. Faded lasers with no core are safe
        StartCoroutine(StartGimmick());
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        this.transform.Rotate(0, 0, _rotSpeed * _dir);
    }
    

    #region DESTROY BEHAVIOR
    private void TimeUp()
    {
        StopAllCoroutines(); // ensure proper despawn anim plays instead of override in next function
        StartCoroutine(OnDissapate());
    }

    private IEnumerator OnDissapate()
    {
        _animator.Play("LaserDespawn", 0, 0);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    #endregion
    private IEnumerator StartGimmick() //TODO
    {
        InvokeRepeating("Gimmick", 0f, _cycleDuration);
        yield return new WaitForSeconds(0f);
    }

    private void Gimmick()
    {
        StartCoroutine(DoLaserAction());
    }

    private IEnumerator DoLaserAction()
    {
        yield return new WaitForSeconds(_downDuration / 2);
        _animator.Play("LaserThreaten");
        yield return new WaitForSeconds(_downDuration / 4); // Speed up the animation
        _animator.speed = 1.5f;
        yield return new WaitForSeconds(_downDuration / 4);

        _animator.speed = 1;
        _animator.Play("LaserAppear");
        _collider.enabled = true;

        yield return new WaitForSeconds(_activeDuration);

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
