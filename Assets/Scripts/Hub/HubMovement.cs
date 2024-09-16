/*
 * Script for player movement in the hub. 
 * Up-down-left-right, classic top-down movement.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HubMovement : MonoBehaviour
{
    private InputAction _moveAction;
    private Vector2 _moveValues;
    private Vector3 _velocity;

    private Rigidbody _rb;
    private Animator _anim;
    private SpriteRenderer _sr;

    public float MoveSpeed = 7f;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move Top-Down");
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveValues = _moveAction.ReadValue<Vector2>();

        AnimatePlayer2D();

        _velocity = new Vector3(_moveValues.x * MoveSpeed, _rb.velocity.y, _moveValues.y * MoveSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer2D();
    }

    private void MovePlayer2D()
    {
        _rb.velocity = _velocity;
    }

    private void AnimatePlayer2D()
    {
        if (_moveValues.x != 0 || _moveValues.y != 0)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if(_moveValues.x > 0)
        {
            _sr.flipX = true;
        }
        else if(_moveValues.x < 0)
        {
            _sr.flipX = false;
        }
    }
}
