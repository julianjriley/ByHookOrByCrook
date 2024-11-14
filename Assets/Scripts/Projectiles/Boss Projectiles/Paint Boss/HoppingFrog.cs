using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles hopping motion of frog 'enemy' on random intervals
/// </summary>
public class HoppingFrog : Projectile
{
    [Header("Components")]
    [SerializeField, Tooltip("To trigger hop/idle animations.")]
    private Animator _anim;
    [SerializeField, Tooltip("To flip sprite when turning.")]
    private SpriteRenderer _sprite;

    [Header("Behavior")]
    [SerializeField, Tooltip("Velocity at which frog hops each step at.")]
    private Vector2 _hopSpeed;
    [SerializeField, Tooltip("min and max value between frog hops. Randomly chosen")]
    private Vector2 _hopDelayInterval;
    [SerializeField, Tooltip("horizontal distance from center at which frog will turn around")]
    private int _turnAroundDistance;
    [SerializeField, Tooltip("whether the frog is facing right by default.")]
    private bool _isHoppingRight;

    private float _initY;
    private bool _isHopping = false;

    // for ensuring the frog is not instantly grounded when they start hopping
    private float _hopStartTimer = 0f;
    private const float HOP_START_DELAY = 0.1f;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        _initY = _rb.position.y;

        _sprite.flipX = !_isHoppingRight;

        // first hop
        Invoke("Hop", Random.Range(_hopDelayInterval.x, _hopDelayInterval.y));
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        // check for turn around
        if (_rb.position.x >= _turnAroundDistance)
        {
            _isHoppingRight = false;
            _sprite.flipX = true;
        }
        else if (_rb.position.x <= -_turnAroundDistance)
        {
            _isHoppingRight = true;
            _sprite.flipX = false;
        }

        // landing check
        if (_isHopping && _hopStartTimer > HOP_START_DELAY && _rb.position.y <= _initY)
        {
            // stop motion
            _rb.velocity = Vector3.zero;
            // snap to floor
            _rb.MovePosition(new Vector3(Mathf.RoundToInt(_rb.position.x), _initY, 0));

            // ensure stays stopped
            _rb.useGravity = false;

            _isHopping = false;

            // hop again after random delay
            Invoke("Hop", Random.Range(_hopDelayInterval.x, _hopDelayInterval.y));
        }
        else
            _hopStartTimer += Time.fixedDeltaTime; // to make sure frog is not instantly grounded when it starts jumping
    }

    private void Hop()
    {
        // set initial hop speed
        _rb.velocity = new Vector3((_isHoppingRight ? 1 : -1) * _hopSpeed.x, _hopSpeed.y, 0);
        
        // use gravity for trajectory
        _rb.useGravity = true;

        _isHopping = true;
        _hopStartTimer = 0f;

        _anim.SetTrigger("Hop");
    }
}
