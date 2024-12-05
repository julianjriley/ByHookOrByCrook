using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Note: Ground needs a CHILD gameobject with a box collider with a Layer set to MikuWall.
/// </summary>
public class BouncyMiku : Projectile
{
    private Vector3 _direction;
    [SerializeField] private GameObject _sprite;
    [SerializeField] private EventReference mikuSound;

    override protected void Start()
    {
        base.Start();

        // randomize initial direction
        _direction.x = Random.Range(0, 2) == 0 ? -1 : 1;
        _direction.y = Random.Range(0, 2) == 0 ? -1 : 1;

        _rb.velocity = _direction * _speed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //_sprite.transform.Rotate(0, 0, _direction.x * 5);
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Ground") || collision.transform.gameObject.layer == 11)
        {
            if(collision.gameObject.name == "Wall")
            {
                _direction.x *= -1;
            }
            else
            {
                _direction.y *= -1;
            }
            _rb.velocity = _direction * _speed;
            SoundManager.Instance.PlayOneShot(mikuSound, gameObject.transform.position);
        }
    }

    override protected void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        /*if (collider.gameObject.CompareTag("Ground") || collider.transform.gameObject.layer == 11)
        {
            if (collider.gameObject.name == "Wall")
            {
                _direction.x *= -1;
            }
            else
            {
                _direction.y *= -1;
            }
            _rb.velocity = _direction * _speed;
        }*/
    }
}
