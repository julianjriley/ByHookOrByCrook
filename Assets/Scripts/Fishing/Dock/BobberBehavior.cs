using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Handles visual behavior of the bobber itself accompanying the actions of FishingControls.
/// </summary>
public class BobberBehavior : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Bobber rigidbody, needed to simulate its movement.")]
    private Rigidbody2D _rigidBody;

    [Header("Launch")]
    [SerializeField, Tooltip("Gravity value of the launching bobber.")]
    private float _launchGravity;
    [SerializeField, Tooltip("Initial vertical cast speed of the bobber.")]
    private float _verticalCastSpeed;
    [SerializeField, Tooltip("Smallest charge initial horizontal cast speed of the bobber.")]
    private float _minHorizontalCastSpeed;
    [SerializeField, Tooltip("Largest charge initial horizontal cast speed of the bobber.")]
    private float _maxHorizontalCastSpeed;

    [Header("Bobbing")]
    [SerializeField, Tooltip("Height value of the water line (relative to bobber transform). Needed to determine when bobber should bob.")]
    private float _waterLevel;
    [SerializeField, Tooltip("factor by which the vertical velocity is reduced on impact with the water; should be between 0 and 1.")]
    private float _verticalDampen;
    [SerializeField, Tooltip("Gravity Value of the bobbing bobber.")]
    private float _bobbingGravity;

    [Header("Tugging")]
    [SerializeField, Tooltip("Smallest possible time between tugs on the line.")]
    private float _minTugInterval;
    [SerializeField, Tooltip("Largest possible time between tugs on the line.")]
    private float _maxTugInterval;
    [SerializeField, Tooltip("Largest possible distance (right or left) from starting point that tug may end up.")]
    private float _maxTugDistance;
    [SerializeField, Tooltip("'Snappiness' of bobber movements towards each new tug goal.")]
    private float _tuggingSharpness;
    [SerializeField, Tooltip("Vertical distance from water level that bobber will be pulled during tugging")]
    private float _tugVerticalDistance;

    [Header("SFX")]
    [SerializeField] EventReference fishing;

    private float _tugTimer = 0;
    private float _tugOrigin;
    private float _tugGoal;

    [Header("Returning")]
    [SerializeField, Tooltip("Game object whose position is the goal of the rod returning.")]
    private GameObject _returnGoal;
    [SerializeField, Tooltip("'Snapiness' of bobber returning to return goal (fishing rod)")]
    private float _returnSharpness;
    [SerializeField, Tooltip("Distance at which bobber will snap to return goal and transition to waiting state.")]
    private float _returnSnapThreshold;

    private void Start()
    {
        Debug.Log("Fishing called");
        //Starts the Fishing EventInstance, which makes all of the sounds.
        SoundManager.Instance.InitializeFishing(fishing);
    }

    public enum BobberState
    {
        Waiting,
        Launching,
        Bobbing,
        Tugging,
        Returning
    }

    [Header("Bobber State")]
    [Tooltip("Do not modify directly. Useful for viewing current state.")]
    public BobberState State = BobberState.Waiting;

    private void Update()
    {
        switch(State)
        {
            case BobberState.Waiting:
                // TODO: may need some system here to actually match the position of the bobber with the rod during the animation
                break;
            case BobberState.Launching:

                // impact with water
                if (transform.localPosition.y < _waterLevel)
                {
                    // stop horizontal momentum AND dampen vertical momentum
                    Vector2 newVelocity = _rigidBody.velocity;
                    newVelocity.x = 0;
                    newVelocity.y = _rigidBody.velocity.y * _verticalDampen;
                    _rigidBody.velocity = newVelocity;

                    // done with launching state
                    State = BobberState.Bobbing;
                    SoundManager.Instance.SetParameter(SoundManager.Instance.fishingEventInstance, "BobberState", 2);
                }

                break;
            case BobberState.Bobbing:

                // float up
                if (transform.localPosition.y < _waterLevel)
                    _rigidBody.gravityScale = -_bobbingGravity;
                // fall down
                else
                    _rigidBody.gravityScale = _bobbingGravity;

                break;
            case BobberState.Tugging:

                // Tug Timer
                if (_tugTimer <= 0)
                {
                    // pick new goal and reset timer
                    _tugGoal = _tugOrigin + UnityEngine.Random.Range(-_maxTugDistance, _maxTugDistance);
                    _tugTimer = UnityEngine.Random.Range(_minTugInterval, _maxTugInterval);
                }
                else
                    _tugTimer -= Time.deltaTime;

                // smooth tracking/lerping to goal
                Vector3 goalPos = new Vector3(_tugGoal, _waterLevel - _tugVerticalDistance, transform.localPosition.z);
                transform.localPosition = Vector3.Lerp(transform.localPosition, goalPos, 1f - Mathf.Exp(-_tuggingSharpness * Time.deltaTime));

                break;
            case BobberState.Returning:
                
                // snap if at return position
                if (Vector3.Distance(transform.position, _returnGoal.transform.position) < _returnSnapThreshold)
                {
                    transform.position = _returnGoal.transform.position;
                    State = BobberState.Waiting;
                    SoundManager.Instance.SetParameter(SoundManager.Instance.fishingEventInstance, "BobberState", 0);
                }
                // smooth lerp back to return position
                else
                    transform.position = Vector3.Lerp(transform.position, _returnGoal.transform.position, 1f - Mathf.Exp(-_returnSharpness * Time.deltaTime));

                break;
        }
    }

    /// <summary>
    /// Gives bobber initial launch speed and switches bobber to launching state.
    /// </summary>
    public void LaunchBobber(float chargeLevel)
    {
        // set launch speed and enable gravity
        float xSpeed = math.remap(0, 1, _minHorizontalCastSpeed, _maxHorizontalCastSpeed, chargeLevel);
        Vector2 launchVelocity = new Vector2(xSpeed, _verticalCastSpeed);
        _rigidBody.velocity = launchVelocity;
        _rigidBody.gravityScale = _launchGravity;

        // start bobbing algorithm
        State = BobberState.Launching;
        SoundManager.Instance.SetParameter(SoundManager.Instance.fishingEventInstance, "BobberState", 1);
    }

    /// <summary>
    /// Switches bobber to fishOnLine state
    /// </summary>
    public void StartTugging()
    {
        if (State == BobberState.Bobbing)
        {
            _tugOrigin = transform.localPosition.x;
            _rigidBody.gravityScale = 0; // stop gravity
            _rigidBody.velocity = Vector2.zero; // stop velocity-based motion

            State = BobberState.Tugging;
            SoundManager.Instance.SetParameter(SoundManager.Instance.fishingEventInstance, "BobberState", 3);
        }
        else
            throw new System.Exception("Incorrect usage of FishOnTheLine() function of BobberBehavior. Can only be called while bobber is currently in Bobbing State");
    }

    /// <summary>
    /// Switches bobber to Returning state (you just caught something).
    /// </summary>
    public void ReturnBobber()
    {
        if (State == BobberState.Tugging)
        {
            State = BobberState.Returning;
            SoundManager.Instance.SetParameter(SoundManager.Instance.fishingEventInstance, "BobberState", 4);
        }
        else
            throw new System.Exception("Incorrect usage of ReturnBobber() function of BobberBehavior. Can only be called while bobber is currently in Tugging State");
    }
}
