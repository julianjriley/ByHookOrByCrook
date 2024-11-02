using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncyMiku : Projectile
{
    private Vector3 _direction;

    override protected void Start()
    {
        base.Start();

        _direction.x = Vector3.right.x;
        _direction.y = Vector3.down.y;
        _rb.velocity = _direction * _speed;
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.transform.gameObject.layer == 11)
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
        }
    } 
}
