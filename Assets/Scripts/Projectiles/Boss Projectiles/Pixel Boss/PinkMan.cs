using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement and player tracking behavior of PinkMan projectile.
/// </summary>
public class PinkMan : Projectile
{
    [SerializeField, Tooltip("Multiplier for speed after direction change")]
    private float _phaseTwoSpeedMultiplier;
    [SerializeField, Tooltip("Max distance from the player's x-pos that it is acceptable to turn within.")]
    private float _xDetectDistance; // prevents auto turning if it spawned on wrong side of player (i.e. player near edge)
    [SerializeField, Tooltip("Used to set charging animation")]
    private Animator _anim;

    private GameObject _player;
    private bool _movingRight;
    private bool _hasTurned = false;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        // ensure gravity is disabled
        _rb.useGravity = false;

        // set initial speed (in appropriate direction)
        _rb.velocity = transform.right * _speed;

        // moving right
        if (_rb.velocity.x > 0)
            _movingRight = true;
        // moving left
        else
            _movingRight = false;

        // find reference to player - used for seeking
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create PinkMan.");
    }

    override protected void FixedUpdate()
    {
        if (_hasTurned)
            return;

        // positive = player right of projectile
        // negative = projectile right of player
        float xDiff = _player.transform.position.x - transform.position.x;

        // if PinkMan must switch direction now
        if(Mathf.Abs(xDiff) < _xDetectDistance && ((_movingRight && xDiff < 0) || (!_movingRight && xDiff > 0)))
        {
            // rotate up
            if (_player.transform.position.y > transform.position.y)
            {
                transform.Rotate(Vector3.forward, 90);
                // increase speed
                _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
            }
            // rotate down
            else
            {
                transform.Rotate(Vector3.forward, -90);
                // increase speed
                _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
            }

            _hasTurned = true;

            // start charging animation
            _anim.SetTrigger("Charge");
        }
    }
}
