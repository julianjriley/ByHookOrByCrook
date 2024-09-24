using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

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

    private float _tugTimer = 0;
    private float _tugOrigin;
    private float _tugGoal;

    public enum BobberState
    {
        Waiting,
        Launching,
        Bobbing,
        Tugging
    }

    [HideInInspector]
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
        }
        else
            throw new System.Exception("Incorrect usage of FishOnTheLine() function of BobberBehavior. Can only be called while bobber is currently in Bobbing State");
    }
}
