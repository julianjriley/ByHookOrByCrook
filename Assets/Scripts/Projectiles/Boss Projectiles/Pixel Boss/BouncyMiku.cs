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
    public GameObject _topright;
    public GameObject _bottomleft;
    private RaycastHit[] _hitArray;
    private Ray ray;
    
    private LayerMask _layer = LayerMask.GetMask("MikuWall");
    // TODO: Get boundaries of screen
    // TODO: Make projectile bounce within those boundaries

    override protected void Start()
    {
        base.Start();

        //SetBoundaries();

        direction = Vector3.right;
        _rb.velocity = transform.right * _speed;
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        _hitArray = Physics.RaycastAll(ray, direction, _layer);

        foreach (RaycastHit hit in _hitArray)
        {
            if (hit.transform.CompareTag("MikuWall"))
            {
                direction = Vector3.Reflect(_rb.velocity * _speed, direction); // change direction
            }
        }
    }


    //// Update is called once per frame
    //private void FixedUpdate()
    //{
    //    if (currentPosition.x < 0 || currentPosition.x > Screen.width || currentPosition.y < 0 || currentPosition.y > Screen.height)
    //    {
    //       
    //        _rb.AddForce(Vector3.Reflect(_rb.velocity * _speed, direction), ForceMode.Force);
    //    }
    //    _rb.AddForce(Vector3.down, ForceMode.Force);
    //}

    //void SetBoundaries()
    //{
    //    Vector3 point = new Vector3();

    //    point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height, Camera.main.nearClipPlane));
    //    _topright.transform.position = point;

    //    point = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
    //    _topright.transform.position = point;
    //}
}
