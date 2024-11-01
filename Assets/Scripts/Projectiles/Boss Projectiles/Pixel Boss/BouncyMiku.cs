using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyMiku : Projectile
{
    private Transform _rightTop;
    private Transform _leftTop;
    private Transform _rightBottom;
    private Transform _leftBottom;

    Vector3 direction;
    [SerializeField]
    private float _force;

    private Vector3 currentPosition;
    // TODO: Get boundaries of screen
    // TODO: Make projectile bounce within those boundaries

    void Start()
    {
        base.Start();

        //SetBoundaries();

        direction = Vector3.up * _force;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _rb.AddForce(direction, ForceMode.Force);

        if(currentPosition.x < 0 || currentPosition.x > Screen.width || currentPosition.y < 0 || currentPosition.y > Screen.height)
        {
            direction = Vector3.Reflect(_rb.velocity, direction);
        }

    }

    void SetBoundaries()
    {
        _rightTop.transform.position = Camera.main.WorldToViewportPoint(new Vector3(1,1,0));
        _leftTop.transform.position = Camera.main.WorldToViewportPoint(new Vector3(0, 1, 0));
        _rightBottom.transform.position = Camera.main.WorldToViewportPoint(new Vector3(1,0,0));
        _leftBottom.transform.position = Camera.main.WorldToViewportPoint(new Vector3(0,0,0));


    }
}
