using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Note: Ground needs a CHILD gameobject with a box collider with a Layer set to MikuWall.
/// </summary>
public class BouncyMiku : Projectile
{
    private Vector3 _direction;
    [SerializeField] private GameObject _sprite;
    [SerializeField] private EventReference mikuSound;

    [Header("Miku Bounds")]
    [SerializeField, Tooltip("Horizontal distance from center that counts as bounds.")]
    private float _horBound;
    [SerializeField, Tooltip("Vertical distance up from center that counts as bounds.")]
    private float _topBound;
    [SerializeField, Tooltip("Vertical distance down from center that counts as bounds.")]
    private float _lowBound;

    private Transform _centerScreenPos;

    override protected void Start()
    {
        base.Start();

        // randomize initial direction
        _direction.x = Random.Range(0, 2) == 0 ? -1 : 1;
        _direction.y = Random.Range(0, 2) == 0 ? -1 : 1;

        _rb.velocity = _direction * _speed;

        _centerScreenPos = GameObject.Find("Center Screen").transform;
    }

    protected override void FixedUpdate()
    {
        // flip vertical
        if ((transform.position.y > _centerScreenPos.position.y + _topBound && _rb.velocity.y > 0)
            || (transform.position.y < _centerScreenPos.position.y - _lowBound && _rb.velocity.y < 0))
        {
            _direction.y *= -1;
            _rb.velocity = _direction * _speed;

            // bounce sound
            SoundManager.Instance.PlayOneShot(mikuSound, gameObject.transform.position);
        }

        // flip horizontal
        if ((transform.position.x > _centerScreenPos.position.x + _horBound && _rb.velocity.x > 0) 
            || (transform.position.x < _centerScreenPos.position.x - _horBound && _rb.velocity.x < 0))
        {
            _direction.x *= -1;
            _rb.velocity = _direction * _speed;

            // bounce sound
            SoundManager.Instance.PlayOneShot(mikuSound, gameObject.transform.position);
        }


        //_sprite.transform.Rotate(0, 0, _direction.x * 5);
    }
}
