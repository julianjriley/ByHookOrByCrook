using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes: love letter explodes within x distance to the player but also after y seconds

public class LoveLetter : Projectile
{
    [SerializeField, Tooltip("Max distance from the player's position that the love letter explodes from.")]
    private float _maxDistance;
    [SerializeField, Tooltip("Constant seeking speed towards player.")]
    private float _seekingSpeed;
    [SerializeField, Tooltip("Kaboom prefab")]
    private GameObject _explosion;

    private GameObject _player;

    protected override void Start()
    {
        base.Start();
        
        // find player ref used to hone projectile in on player
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create a LoveLetter Projectile.");

        // set velocity
        _rb.velocity = Vector3.zero;      
    }

    override protected void FixedUpdate()
    {
        // proximity explosion logic
        float distanceBetween = Vector3.Distance(_player.transform.position, transform.position);
        if (Mathf.Abs(distanceBetween) <= _maxDistance)
        {
            // when the projectile is close to the player, explode
            Explode();
        }

        // seek the player
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _rb.velocity = dir * _seekingSpeed;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerCombat player))
            Explode();
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerCombat player))
            Explode();
    }
    private void Explode()
    {
        Instantiate(Resources.Load("Explosion"), this.transform.position, this.transform.rotation);
        Destroy(gameObject);
        
    }
}
