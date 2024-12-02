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
    [SerializeField] private float _dir = 1;
    [SerializeField] private float _activeDuration = 2.5f;
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

    public void SetDirAndSpeed(float dir, float duration, float speed=.7f)
    {
        _dir = dir;
        _activeDuration = duration;
        _rotSpeed = speed;
    }

    

    #region DESTROY BEHAVIOR
    private void TimeUp()
    {
        StartCoroutine(OnDissapate());
    }

    private IEnumerator OnDissapate()
    {
        _animator.Play("LaserDespawn", 0, 0);
        yield return new WaitForSeconds(.33f);
        Destroy(this);
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
        yield return new WaitForSeconds(_downDuration);

        
        _animator.Play("LaserAppear");
        yield return new WaitForSeconds(.5f);
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
