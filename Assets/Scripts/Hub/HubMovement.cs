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
    private bool soundOn = true;

    public float MoveSpeed = 7f;
    public bool IsIdle = false; // A toggle for forcing the bear to stand still
    private bool _isTransitionDone = false;

    [SerializeField] EventReference footstepsSound;
    private EventInstance footsteps;

    [SerializeField, Tooltip("Used to disable movement controls while scene transition is not completed.")]
    private SceneTransitionsHandler _transitionsHandler;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move Top-Down");
        _moveAction.Enable();
        InputSystem.actions.FindAction("Mouse Position").Enable();
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

        // no motion if still loading scene
        if (!_transitionsHandler.IsDoneLoading())
            IsIdle = true; // prevents motion AND animation when trying to move
        else if (!_isTransitionDone)
        {
            IsIdle = false;
            _isTransitionDone = true;
        }
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
            if (_velocity.magnitude != 0 && !footstepsState.Equals(PLAYBACK_STATE.PLAYING) && soundOn)
            {
                footsteps.start();
                //Debug.Log("START");
            }
            if (_velocity.magnitude == 0 && footstepsState.Equals(PLAYBACK_STATE.PLAYING))
            {
                footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                //Debug.Log("STOP");
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
        
    }

    public void StopFootsteps()
    {
        footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    private void AnimatePlayer2D()
    {
        if ((_moveValues.x != 0) && !IsIdle)
        {
            _anim.SetBool("IsMovingSide", true);
            _anim.SetBool("FacingSide", true);
            _anim.SetBool("FacingUp", false);
            _anim.SetBool("FacingDown", false);
        }
        else
        {
            _anim.SetBool("IsMovingSide", false);
        }


        if ((_moveValues.y > 0 && _moveValues.x == 0) && !IsIdle)
        {
            _anim.SetBool("IsMovingUp", true);
            _anim.SetBool("FacingUp", true);
            _anim.SetBool("FacingSide", false);
            _anim.SetBool("FacingDown", false);
        }
        else
        {
            _anim.SetBool("IsMovingUp", false);
        }


        if ((_moveValues.y < 0 && _moveValues.x == 0) && !IsIdle) 
        {
            _anim.SetBool("IsMovingDown", true);
            _anim.SetBool("FacingDown", true);
            _anim.SetBool("FacingUp", false);
            _anim.SetBool("FacingSide", false);
        }
        else 
        {
            _anim.SetBool("IsMovingDown", false);
        }
        

        if (!IsIdle)
        {
            if (_moveValues.x > 0)
            {
                _sr.flipX = false;
            }
            else if (_moveValues.x < 0)
            {
                _sr.flipX = true;
            }
        }
        
    }
}
