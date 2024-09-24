using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

/// <summary>
/// Handles input gathering (using new input system), processing of casting inputs, and processing of reeling inputs.
/// Calls are made to the bobber and random fish selector as needed.
/// </summary>
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

            // launch bobber
            _bobber.LaunchBobber(_currCharge);

            // TODO: make this fade out instead
            _castingIndicator.SetActive(false);
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
    [Header("Reeling")]
    [SerializeField, Tooltip("Handles bobber movement and is called within controls.")]
    private BobberBehavior _bobber;
    [SerializeField, Tooltip("Smallest possible wait before a fish bites.")]
    private float _minFishBiteTime;
    [SerializeField, Tooltip("Larges possible wait before a fish bites.")]
    private float _maxFishBiteTime;

    private bool _firstReelingFrame = true;
    private float _fishBiteTimer = 0;

    private void HandleReelingControls()
    {
        // randomize fish bite time
        if (_firstReelingFrame)
        {
            _fishBiteTimer = UnityEngine.Random.Range(_minFishBiteTime, _maxFishBiteTime);
            _firstReelingFrame = false;
        }

        // Wait for fish to bite
        if (_bobber.State == BobberBehavior.BobberState.Bobbing)
        {
            if (_fishBiteTimer < 0)
                _bobber.StartTugging();
            else
                _fishBiteTimer -= Time.deltaTime;
        }

        // Timing click controls
        if(_bobber.State == BobberBehavior.BobberState.Tugging)
        {
            // TODO: handle timing click
        }
    }
    #endregion
}
