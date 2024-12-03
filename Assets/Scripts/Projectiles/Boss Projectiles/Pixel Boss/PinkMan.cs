using FMODUnity;
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
    [SerializeField, Tooltip("Max distance from the player's other axis pos that it is acceptable to turn within.")]
    private float _detectDistance; // prevents auto turning if it spawned on wrong side of player (i.e. player near edge)
    [SerializeField, Tooltip("Direction of initial motion. Should be a unit vector in a cardinal direction.")]
    private Vector2 _initialDirection;
    [SerializeField, Tooltip("Used to set charging animation")]
    private Animator _anim;
    [SerializeField, Tooltip("Used to orient the sprite according to move directions")]
    private SpriteRenderer _renderer;

    private GameObject _player;
    private bool _hasTurned = false;
    [SerializeField] EventReference pacSound;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        // ensure gravity is disabled
        _rb.useGravity = false;

        // set initial speed (in appropriate direction)
        _rb.velocity = transform.right * _speed;

        // flipY to account for moving left variant and moving down variant coming from the right side of screen
        if (_rb.velocity.x < -0.01 || (_rb.velocity.y < -0.01 && _rb.position.x > 0))
        {
            _renderer.flipY = true;
        }

        // find reference to player - used for seeking
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create PinkMan.");
    }

    override protected void FixedUpdate()
    {
        // only rotate and increase speed once
        if (_hasTurned)
            return;

        // determine current direction of motion
        float posDiff;
        bool xDir;
        // moving in x-direction
        if (Mathf.Abs(_rb.velocity.x) > 0.01f)
        {
            posDiff = _player.transform.position.x - transform.position.x;
            xDir = true;
        }
        else // moving in y-direction
        {
            posDiff = _player.transform.position.y - transform.position.y;
            xDir = false;
        }

        // if PinkMan must switch direction now
        if(Mathf.Abs(posDiff) < _detectDistance)
        {
            if(xDir)
            {
                bool isRight = _rb.velocity.x > 0.01f;

                // right -> up, left -> down
                if ((_player.transform.position.y > transform.position.y && isRight) || (_player.transform.position.y < transform.position.y && !isRight)) 
                {
                    transform.Rotate(Vector3.forward, 90);
                    // increase speed
                    _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
                }
                // right -> down, left -> up
                else
                {
                    transform.Rotate(Vector3.forward, -90);
                    // increase speed
                    _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
                }
            }
            else
            {
                bool isUp = _rb.velocity.y > 0.01f;

                // up -> left, down -> right
                if ((_player.transform.position.x > transform.position.x && !isUp) || (_player.transform.position.x < transform.position.x && isUp))
                {
                    transform.Rotate(Vector3.forward, 90);
                    // increase speed
                    _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
                }
                // up -> right, down -> left
                else
                {
                    transform.Rotate(Vector3.forward, -90);
                    // increase speed
                    _rb.velocity = transform.right * _speed * _phaseTwoSpeedMultiplier;
                }
            }
            

            _hasTurned = true;

            // start charging animation
            _anim.SetTrigger("Charge");
            GetComponent<StudioParameterTrigger>().TriggerParameters();
        }
    }
}
