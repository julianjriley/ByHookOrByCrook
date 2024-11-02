using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

// Notes: love letter explodes within x distance to the player but also after y seconds
// Suggestion: use Unity animator to deal with projectile movement? it sounds easier.

public class LoveLetter : Projectile
{
    [SerializeField, Tooltip("Time until love letter explodes")]
    private float _timeTillExplosion;
    [SerializeField, Tooltip("Damage multiplier is applied when the love letter is VERY close to the player.")]
    private float _damageMultiplier;
    [SerializeField, Tooltip("Max distance from the player's x-pos that the love letter explodes from.")]
    private float _xDistance;

    private bool _hasExploded;
    private GameObject _player;

    protected override void Start()
    {
        base.Start();
        
        // ensures bool starts false
        _hasExploded = false;
        
        // find player ref used to hone projectile in on player
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create a LoveLetter Projectile.");
        
        // set velocity
        _rb.velocity = transform.right * _speed;
        
        // explodes after a given time
        Invoke("Explode()", _timeTillExplosion);
        
    }

    private void Explode()
    {
        if (_hasExploded)
        {
            // shouldnt reach this because projectile should have been destroyed already
            return;
        }
        else
        {
            // cause damage
            _player.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy();
            _hasExploded = true;
            // play animation here

            new WaitForSeconds(3);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        float xDiff = _player.transform.position.x - transform.position.x;

        if (Mathf.Abs(xDiff) < _xDistance)
        {
            // do we want player to take more damage when explosions are closer?
            _damage = _damage * _damageMultiplier;
            Explode();
        }
        else if (Mathf.Abs(xDiff) == _xDistance)
        {
            // do less damage?
            Explode();
        }
    }
}
