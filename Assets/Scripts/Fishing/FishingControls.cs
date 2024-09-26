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
    [Header("Adjacent Components")]
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
    [SerializeField, Tooltip("Casting goal is used to determine proficiency score for casting portion of fishing.")]
    private CastingGoalMover _castingGoal;

    private float _initFillScaleX;
    private float _currCharge = 0; // 0 is minimum charge; 1 is maximum charge
    private bool _isIncreasing = true;
    private bool _prevClick = false;
    private bool _isWaiting = true;
    private float _castingScore = 0; // min 0; max 1

    private void HandleCastingControls()
    {
        // CONTROLS LOGIC
        if(_bobber.State == BobberBehavior.BobberState.Waiting) // ensure you can only cast once bobber has fully returned
        {
            // start showing indicator and reset charge
            if (_fishingClick && _isWaiting)
            {
                // reset bar
                _currCharge = 0;
                _isIncreasing = true;
                _isWaiting = false;

                _castingIndicator.SetActive(true);
            }
            // holding down
            else if (_fishingClick)
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

                // UPDATE FILLING BAR
                // determine new fill bar scale
                Vector3 newBarScale = _fillBar.transform.localScale;
                newBarScale.x = math.remap(0, 1, 0, _initFillScaleX, _currCharge);

                // determine new fill bar pos (to keep indicator left-aligned)
                Vector3 newBarPos = _fillBar.transform.localPosition;
                float scaleDiff = _fillBar.transform.localScale.x - newBarScale.x;
                newBarPos.x = newBarPos.x - (scaleDiff / 2f);

                // set new fill bar values
                _fillBar.transform.localScale = newBarScale;
                _fillBar.transform.localPosition = newBarPos;
            }
            // mouse released (launch bobber)
            else if (!_fishingClick && _prevClick)
            {
                // launch bobber
                _bobber.LaunchBobber(_currCharge);

                // TODO: make this fade out instead
                _castingIndicator.SetActive(false);
            }
        }
        
        // transition to reeling controls once bobber hits the water (enters bobbing)
        if (_bobber.State == BobberBehavior.BobberState.Bobbing)
        {
            _castingScore = _castingGoal.GetCastingScore();

            _isReeling = true;
            _fishBiteTimer = UnityEngine.Random.Range(_minFishBiteTime, _maxFishBiteTime);
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
    [SerializeField, Tooltip("Game Object of reeling UI. Used to enable/disable.")]
    private GameObject _reelIndicator;
    [SerializeField, Tooltip("Game object that will have its scale scripted to make circle shrink visually.")]
    private GameObject _shrinkingRing;
    [SerializeField, Tooltip("Total time it takes from the ring to appear to when it fully shrinks.")]
    private float _totalShrinkTime;
    [SerializeField, Tooltip("Scale of shrinking ring at which a click starts to become a perfect success.")]
    private float _maxPerfectCutoff;
    [SerializeField, Tooltip("Scale of shrinking ring at which the click is no longer a perfect success.")]
    private float _minPerfectCutoff;
    [SerializeField, Tooltip("Scale of shrinking ring at which an early failure will start contributing some points to reeling score.")]
    private float _maxFailureCutoff;

    private float _reelingTimer;
    private Vector3 _initialShrinkingRingScale;
    private float _currentShrinkingScale = 0;
    private float _reelingScore = 0; // min 0; max 1

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
                _currentShrinkingScale = _initialShrinkingRingScale.x;
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
                if(_currentShrinkingScale < _minPerfectCutoff) // late clicks
                {   
                    if (_reelingTimer <= 0)
                        // SUPER late (never clicked)
                        _reelingScore = 0;
                    else
                        // variable score based on how late the press was
                        _reelingScore = math.remap(_minPerfectCutoff, 0, 1, 0, _currentShrinkingScale);
                }
                else if(_currentShrinkingScale > _maxPerfectCutoff) // early click
                {
                    if (_currentShrinkingScale > _maxFailureCutoff)
                        // SUPER early
                        _reelingScore = 0;
                    else
                        // variable score based on how early the press was
                        _reelingScore = math.remap(_maxFailureCutoff, _maxPerfectCutoff, 0, 1, _currentShrinkingScale);
                }
                else
                {
                    // perfect! (between min and max perfect cutoffs)
                    _reelingScore = 1;
                }

                // determine combined fishing score
                float combinedScore = (_castingScore + _reelingScore) / 2.0f;
                // make catch with calculated odds
                // TEMP: Debug scores until catch randomizer can be used
                Debug.Log("Casting Score: " + _castingScore + "        Reeling Score: " + _reelingScore);
                // TODO: CatchRandomizer CANNOT actually be called until there is at least one item implemented from each category (null reference issue)
                // _catchRandomizer.CatchFish(combinedScore);

                // disable reeling UI
                _reelIndicator.SetActive(false);

                // return bobber
                _bobber.ReturnBobber();

                // pick new casting goal pos
                _castingGoal.RandomizeCastingGoal();

                // return back to casting controls
                _isReeling = false;
                _isWaiting = true;
            }

            // update reeling timer
            _reelingTimer -= Time.deltaTime;
            if (_reelingTimer < 0)
                _reelingTimer = 0;

            // update scale based on timer
            _currentShrinkingScale = math.remap(_totalShrinkTime, 0, _initialShrinkingRingScale.x, 0, _reelingTimer);
            // update scale based on calculated value
            _shrinkingRing.transform.localScale = _initialShrinkingRingScale * (_currentShrinkingScale / _initialShrinkingRingScale.x);
        }
    }
    #endregion
}
