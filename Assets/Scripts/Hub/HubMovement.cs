/*
 * Script for player movement in the hub. 
 * Up-down-left-right, classic top-down movement.
 */

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HubMovement : MonoBehaviour
{
    private InputAction _moveAction;
    private Vector2 _moveValues;
    private Vector3 _velocity;

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sr;

    public float MoveSpeed = 7f;
    public bool IsIdle = false; // A toggle for forcing the bear to stand still

    [SerializeField] EventReference footstepsSound;
    private EventInstance footsteps;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move Top-Down");
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        // TODO: Put this somewhere relevant to it
        Camera camera = Camera.main;
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0, 1, 1);
        footsteps = SoundManager.Instance.CreateInstance(footstepsSound);
    }

    // Update is called once per frame
    void Update()
    {
        _moveValues = _moveAction.ReadValue<Vector2>();

        AnimatePlayer2D();

        _velocity = new Vector2(_moveValues.x * MoveSpeed, _moveValues.y * MoveSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer2D();
    }

    private void MovePlayer2D()
    {
        PLAYBACK_STATE footstepsState;
        footsteps.getPlaybackState(out footstepsState);
        if (!IsIdle)
        {
            _rb.velocity = _velocity;
            if (!footstepsState.Equals(PLAYBACK_STATE.PLAYING))
            {
                footsteps.start();
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
            if (footstepsState.Equals(PLAYBACK_STATE.PLAYING))
            {
                footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
        
    }

    private void AnimatePlayer2D()
    {
        if ((_moveValues.x != 0 || _moveValues.y != 0 ) && !IsIdle)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if (!IsIdle)
        {
            if (_moveValues.x > 0)
            {
                _sr.flipX = true;
            }
            else if (_moveValues.x < 0)
            {
                _sr.flipX = false;
            }
        }
        
    }
}
