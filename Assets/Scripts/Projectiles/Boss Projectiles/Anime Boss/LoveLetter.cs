using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes: love letter explodes within x distance to the player but also after y seconds

public class LoveLetter : Projectile
{
    [SerializeField, Tooltip("Time until love letter explodes")]
    private float _timeTillExplosion;
    [SerializeField, Tooltip("Max distance from the player's position that the love letter explodes from.")]
    private float _maxDistance;
    [SerializeField, Tooltip("Kaboom prefab")]
    private GameObject _explosion;

    private Vector3 _direction;
    private GameObject _player;

    protected override void Start()
    {
        base.Start();
        
        // find player ref used to hone projectile in on player
        _player = GameObject.FindWithTag("Player");
        if (_player is null)
            throw new System.Exception("No player is present in the scene, but you are trying to create a LoveLetter Projectile.");

        // set velocity
        _direction.x = Vector3.right.x;
        _rb.velocity = _direction * _speed;
       
        // explodes after a given time
        Invoke("Explode", _timeTillExplosion);        
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
    private void FixedUpdate()
    {
        float distanceBetween = Vector3.Distance(_player.transform.position, transform.position);

        if (_player.transform.position.x < Screen.width / 2)
        {
            // switch letter direction if player is on the left side of the screen
            _direction.x *= -1;
            _rb.velocity = _direction * _speed;
        }
        else
        {
            // switch letter direction if player is on the right side of the screen
            _direction.x *= -1;
            _rb.velocity = _direction * _speed;
        }

        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        //Debug.Log(distanceBetween);
        if (Mathf.Abs(distanceBetween) <= _maxDistance)
        {
            // when the projectile is close to the player, explode
            Explode();
        }

    }
}
