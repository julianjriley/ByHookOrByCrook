using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class FishingControls : MonoBehaviour
{
    // State management
    private bool _isReeling = false;

    // Called on first frame
    private void Start()
    {
        // inputs
        _inputAction = InputSystem.actions.FindAction(FISHING_INPUT_ACTION);

        // casting vars
        _initFillScaleX = _fillBar.transform.localScale.x;
        _castingIndicator.SetActive(false); // hidden by default
    }

    // Update is called once per frame
    void Update()
    {
        // update inputs every frame
        ReadInputs();

        // handle EITHER casting or reeling
        if (!_isReeling)
            HandleCastingControls();
        else
            HandleReelingControls();
    }

    #region INPUTS
    private const string FISHING_INPUT_ACTION = "Fishing";
    private InputAction _inputAction;
    private bool _fishingClick = false;

    private void ReadInputs()
    {
        _fishingClick = false;
        // read input to determine if fishing click held during this frame
        float input = _inputAction.ReadValue<float>();
        if (input > InputSystem.settings.defaultDeadzoneMin)
            _fishingClick = true;
    }
    #endregion

    #region CASTING
    [Header("Casting")]
    [SerializeField, Tooltip("enabled/disabled appropriately during casting segment.")]
    private GameObject _castingIndicator;
    [SerializeField, Tooltip("Game object that will be dynamically scaled to show the charge of the cast.")]
    private GameObject _fillBar;
    [SerializeField, Tooltip("Rate at which casting bar charges up and down.")]
    private float _chargeRate;

    private float _initFillScaleX;
    private float _currCharge = 0; // 0 is minimum charge; 1 is maximum charge
    private bool _isIncreasing = true;
    private bool _prevClick = false;

    private void HandleCastingControls()
    {
        // CONTROLS LOGIC
        // start displaying charging bar
        if(_fishingClick && !_prevClick)
        {
            // reset bar
            _currCharge = 0;
            _isIncreasing = true;

            _castingIndicator.SetActive(true);
        }
        // holding down
        else if(_fishingClick)
        {
            if (_isIncreasing) // bar increasing
            {
                _currCharge += _chargeRate * Time.deltaTime;

                // flip direction, if necessary
                if (_currCharge > 1)
                {
                    _currCharge = 1;
                    _isIncreasing = false;
                }
            }
            else // bar decreasing
            {
                _currCharge -= _chargeRate * Time.deltaTime;

                // flip direction, if necessary
                if (_currCharge < 0)
                {
                    _currCharge = 0;
                    _isIncreasing = true;
                }
            }
        }
        else if(!_fishingClick && _prevClick) // transition to reeling mode
        {
            _isReeling = true;
        }

        // UPDATING FILL BAR
        if(_fishingClick)
        {
            // determine new fill bar scale
            Vector3 newBarScale = _fillBar.transform.localScale;
            newBarScale.x = math.remap(0, 1, 0, _initFillScaleX, _currCharge);

            // determine new fill bar pos (to keep indicator left-aligned)
            Vector3 newBarPos = _fillBar.transform.localPosition;
            float scaleDiff = _fillBar.transform.localScale.x - newBarScale.x;
            newBarPos.x = newBarPos.x - (scaleDiff/2f);

            // set new fill bar values
            _fillBar.transform.localScale = newBarScale;
            _fillBar.transform.localPosition = newBarPos;
        }

        _prevClick = _fishingClick;
    }
    #endregion

    #region REELING
    [Header("Bobber Behavior")]
    [SerializeField, Tooltip("Bobber rigidbody, needed to simulate its movement")]
    private Rigidbody2D _bobber;
    [SerializeField, Tooltip("Gravity value of the launching bobber")]
    private float _launchGravity;
    [SerializeField, Tooltip("Initial vertical cast speed of the bobber")]
    private float _verticalCastSpeed;
    [SerializeField, Tooltip("Smallest charge initial horizontal cast speed of the bobber")]
    private float _minHorizontalCastSpeed;
    [SerializeField, Tooltip("Largest charge initial horizontal cast speed of the bobber")]
    private float _maxHorizontalCastSpeed;
    [SerializeField, Tooltip("Height value of the water line (relative to bobber transform). Needed to determine when bobber should bob")]
    private float _waterLevel;
    [SerializeField, Tooltip("factor by which the vertical velocity is reduced on impact with the water; should be between 0 and 1")]
    private float _verticalDampen;
    [SerializeField, Tooltip("Gravity Value of the bobbing bobber")]
    private float _bobbingGravity;

    // states
    private bool _firstReelingFrame = true;
    private bool _inWater = false;

    private void HandleReelingControls()
    {
        // launch bobber
        if (_firstReelingFrame)
        {
            // set launch speed and enable gravity
            float xSpeed = math.remap(0, 1, _minHorizontalCastSpeed, _maxHorizontalCastSpeed, _currCharge);
            Vector2 launchVelocity = new Vector2(xSpeed, _verticalCastSpeed);
            _bobber.velocity = launchVelocity;
            _bobber.gravityScale = _launchGravity;

            _firstReelingFrame = false;
        }
        // hits water
        else if (!_inWater && _bobber.transform.localPosition.y < _waterLevel)
        {
            _inWater = true;

            // stop horizontal momentum; dampen vertical momentum
            Vector2 newVelocity = _bobber.velocity;
            newVelocity.x = 0;
            newVelocity.y = _bobber.velocity.y * _verticalDampen;
            _bobber.velocity = newVelocity;

            // no longer needed, the bobber has hit the water
            _castingIndicator.SetActive(false);
        }

        if(_inWater)
        {
            // Bobbing behavior
            if (_bobber.transform.localPosition.y < _waterLevel)
                _bobber.gravityScale = -_bobbingGravity;
            else
                _bobber.gravityScale = _bobbingGravity;
        }
    }
    #endregion
}
