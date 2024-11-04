using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles behavior of bird 'enemy' movement back and forth across screen.
/// Spawns bird dropping attacks both when near player and on random intervals.
/// </summary>
public class Bird : Projectile
{
    [Header("Bird Movement")]
    [SerializeField, Tooltip("horizontal distance from the center of the screen at which the bird will turn around")]
    private float _turnAroundDistance;
    [SerializeField, Tooltip("whether the bird is flying right by default.")]
    private bool _isFlyingRight;
    [SerializeField, Tooltip("Used for flipping sprite on turn around")]
    private SpriteRenderer _renderer;

    [Header("Droppings")]
    [SerializeField, Tooltip("Dropping projectile object to spawn.")]
    private GameObject _droppingPrefab;
    [SerializeField, Tooltip("Place where droppings will spawn from the bird.")]
    private GameObject _defecationLocation;
    [SerializeField, Tooltip("Initial downward speed of spawned dropping.")]
    private float _droppingSpeed;
    [SerializeField, Tooltip("min to max delay between bird droppings.")]
    private Vector2 _spawnDelayInterval;
    [SerializeField, Tooltip("horizontal distance from player at which bird will attempt to release its bowels.")]
    private float _attackDistance;

    // droppings
    private GameObject _player;
    private float _spawnDelayTimer;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        // find reference to player - used for seeking
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create PinkMan.");

        // invoke first repeating dropping call
        Invoke("DropDropping", Random.Range(_spawnDelayInterval.x, _spawnDelayInterval.y));

        // ensure gravity is disabled
        _rb.useGravity = false;

        // set initial velocity and sprite direction
        _renderer.flipX = !_isFlyingRight;
        _rb.velocity = (_isFlyingRight ? 1 : -1) * transform.right * _speed;

        // no delay on initial dropping
        _spawnDelayTimer = _spawnDelayInterval.y;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        // check for closeness to player AND ensure minimum time has passed
        float horDist = Mathf.Abs(_player.transform.position.x - _rb.position.x);
        if(_spawnDelayTimer > _spawnDelayInterval.x && horDist < _attackDistance)
        {
            DropDropping();
        }

        _spawnDelayTimer += Time.fixedDeltaTime;

        // turn left at right edge
        if (_rb.position.x > _turnAroundDistance)
        {
            // set sprite scale and velocity
            _renderer.flipX = true;
            _rb.velocity = -transform.right * _speed;
            _isFlyingRight = false;
        }
        // turn right at left edge
        else if (_rb.position.x < -_turnAroundDistance)
        {
            // set sprite scale and velocity
            _renderer.flipX = false;
            _rb.velocity = transform.right * _speed;
            _isFlyingRight = true;
        }
    }

    /// <summary>
    /// Spawn bird dropping at appropriate location.
    /// </summary>
    private void DropDropping()
    {
        // resets next invoked dropping call in case it was triggered by player proximity instead of invoke call
        CancelInvoke();

        // create dropping
        GameObject newDropping = Instantiate(_droppingPrefab, _defecationLocation.transform.position, _droppingPrefab.transform.rotation);
        newDropping.GetComponent<Rigidbody>().velocity = new Vector3(0, -_droppingSpeed, 0);

        // ensure waiting at least minimum delay before spawning again
        _spawnDelayTimer = 0;

        // invoke next dropping call
        Invoke("DropDropping", Random.Range(_spawnDelayInterval.x, _spawnDelayInterval.y));
    }
}
