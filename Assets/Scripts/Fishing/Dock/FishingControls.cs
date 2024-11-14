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
    [SerializeField, Tooltip("Used to lock controls until popup no longer on screen")]
    private CatchUI _catchUI;
    [SerializeField, Tooltip("Used to trigger player animations corresponding to actions.")]
    private Animator _anim;

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
        // Prevent ALL controls while popup still active
        if (_catchUI.IsPopupActive())
            return;

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
    [SerializeField, Tooltip("Used for indicating performance of casting task")]
    private PerformancePopup _castingPopup;

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

                _anim.SetTrigger("WindUp");
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

                _anim.SetTrigger("Cast");
            }
        }
        
        // transition to reeling controls once bobber hits the water (enters bobbing)
        if (_bobber.State == BobberBehavior.BobberState.Bobbing)
        {
            _castingScore = _castingGoal.GetCastingScore();

            // trigger casting popup
            if (_castingScore == 1)
                _castingPopup.PopUp(0);
            else if (_castingGoal.IsSuperFar())
                _castingPopup.PopUp(999);
            else
            {
                if (_bobber.transform.position.x < _castingGoal.transform.position.x)
                    _castingPopup.PopUp(-1);
                else
                    _castingPopup.PopUp(1);
            }

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
    [SerializeField, Tooltip("Ideal scale for shrinking ring to match the outer edge of the goal circle.")]
    private float _perfectScale;
    [SerializeField, Tooltip("Distance from perfect scale that still counts as a perfect score.")]
    private float _perfectThreshold;
    [SerializeField, Tooltip("Distance from perfect scale at which score becomes a max failure.")]
    private float _failureThreshold;
    [SerializeField, Tooltip("Used for indicating performance of reeling task")]
    private PerformancePopup _reelingPopup;

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

                _anim.SetTrigger("Reeling");
            }
            else
                _fishBiteTimer -= Time.deltaTime;
        }

        // Timing click controls
        if(_bobber.State == BobberBehavior.BobberState.Tugging)
        {
            // Assign reeling score (either when the player clicks OR waits too long)
            if(_fishingClick || _reelingTimer <= 0)
            {
                // no click was ever made - default to max fail
                if (_reelingTimer <= 0)
                {
                    _reelingScore = 0;
                    _reelingPopup.PopUp(999);
                }
                // max failure (either very  early or very late)
                else if (_currentShrinkingScale > _perfectScale + _failureThreshold || _currentShrinkingScale < _perfectScale - _failureThreshold)
                {
                    _reelingScore = 0;
                    _reelingPopup.PopUp(999);
                }
                // perfect success (close to perfect scale)
                else if (_currentShrinkingScale > _perfectScale - _perfectThreshold && _currentShrinkingScale < _perfectScale + _perfectThreshold)
                {
                    _reelingScore = 1;
                    _reelingPopup.PopUp(0);
                }
                // slightly early click (score between 0 and 1)
                else if (_currentShrinkingScale > _perfectScale)
                {
                    _reelingScore = math.remap(_perfectScale + _failureThreshold, _perfectScale + _perfectThreshold, 0, 1, _currentShrinkingScale);
                    _reelingPopup.PopUp(-1);
                }
                // slightly late click (score between 0 and 1)
                else if (_currentShrinkingScale < _perfectScale)
                {
                    _reelingScore = math.remap(_perfectScale - _failureThreshold, _perfectScale - _perfectThreshold, 0, 1, _currentShrinkingScale);
                    _reelingPopup.PopUp(1);
                }

                // determine combined fishing score
                float combinedScore = (_castingScore + _reelingScore) / 2.0f;

                // useful debugs for balancing
                //Debug.Log("Casting Score: " + _castingScore + "  Reeling Score: " + _reelingScore);
                Debug.Log("Score: " + combinedScore);

                // actually catch fish
                _catchRandomizer.CatchFish(combinedScore);

                // reset fishing for next cast
                _reelIndicator.SetActive(false);
                _bobber.ReturnBobber();
                _castingGoal.RandomizeCastingGoal();
                _isReeling = false;
                _isWaiting = true;

                _anim.SetTrigger("Yank");
            }
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
    #endregion
}
