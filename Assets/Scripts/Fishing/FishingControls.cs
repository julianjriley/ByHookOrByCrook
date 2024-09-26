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
    [Header("Other Components")]
    [SerializeField, Tooltip("Handles bobber movement and is called within controls.")]
    private BobberBehavior _bobber;
    [SerializeField, Tooltip("Handles randomization of caught fish based on relevant parameters.")]
    private CatchRandomizer _catchRandomizer;

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

        // reeling
        _initialShrinkingRingScale = _shrinkingRing.transform.localScale;
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
        // Ensure bobber has fully returned before we process casting inputs
        if (_bobber.State != BobberBehavior.BobberState.Waiting)
            return;

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
            _fishBiteTimer = UnityEngine.Random.Range(_minFishBiteTime, _maxFishBiteTime);

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
    [Header("Bobbing")]
    [SerializeField, Tooltip("Smallest possible wait before a fish bites.")]
    private float _minFishBiteTime;
    [SerializeField, Tooltip("Larges possible wait before a fish bites.")]
    private float _maxFishBiteTime;

    private float _fishBiteTimer = 0;

    [Header("Reeling")]
    [SerializeField, Tooltip("Game Object of reeling UI. Used to enable/disable")]
    private GameObject _reelIndicator;
    [SerializeField, Tooltip("Game object that will have its scale scripted to make circle shrink visually.")]
    private GameObject _shrinkingRing;
    [SerializeField, Tooltip("Rate at which the shrinking ring's scale decreases.")]
    private float _ringShrinkRate;
    [SerializeField, Tooltip("Total time it takes from the ring to appear to when it fully shrinks.")]
    private float _totalShrinkTime;
    [SerializeField, Tooltip("percent of ring shrinking at which goal becomes a success.")]
    private float _maxPercentGoal;
    [SerializeField, Tooltip("percent of ring shrinking at which goal is no longer a success.")]
    private float _minPercentGoal;

    private float _reelingTimer;
    private Vector3 _initialShrinkingRingScale;
    private float _currentShrinkPercent = 1; // decreases from 1 to 0 over process

    private void HandleReelingControls()
    {
        // Wait for fish to bite
        if (_bobber.State == BobberBehavior.BobberState.Bobbing)
        {
            if (_fishBiteTimer < 0)
            {
                // restart all reeling parameters
                _bobber.StartTugging();
                _reelingTimer = _totalShrinkTime;
                _shrinkingRing.transform.localScale = _initialShrinkingRingScale;
                _reelIndicator.SetActive(true);
                _currentShrinkPercent = 1;
            }
            else
                _fishBiteTimer -= Time.deltaTime;
        }

        // Timing click controls
        if(_bobber.State == BobberBehavior.BobberState.Tugging)
        {
            // if you click ANY time during tugging it will activate (early, perfect, late)
            if(_fishingClick || _reelingTimer <= 0)
            {
                if (_currentShrinkPercent < _maxPercentGoal && _currentShrinkPercent > _minPercentGoal)
                {
                    // SUCCESS
                    // max odds
                }
                else
                {
                    // FAILURE
                    // variable odds depending on distance from goal range (either max or min depending on if it was an early or a late miss)
                }

                // make catch with calculated odds
                // TODO: make odds calculations actually go in here
                _catchRandomizer.CatchFish(1, 1);

                _reelIndicator.SetActive(false);
                _bobber.ReturnBobber();
                // return back to casting controls
                _isReeling = false;
            }

            // update timer
            _reelingTimer -= Time.deltaTime;
            if (_reelingTimer < 0)
                _reelingTimer = 0;

            // update scale
            _currentShrinkPercent = _reelingTimer / _totalShrinkTime;
            _shrinkingRing.transform.localScale = _initialShrinkingRingScale * _currentShrinkPercent;
        }
    }
    #endregion
}
