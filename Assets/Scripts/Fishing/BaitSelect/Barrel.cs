using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

/// <summary>
/// Handles all functions and data relating to Barrel buttons for selecting baits.
/// </summary>
public class Barrel : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField, Tooltip("Used to acquire certain references to bait selection data.")]
    private BaitSelector _baitSelector;
    [SerializeField, Tooltip("Used to spawn selected bait in layout group.")]
    private GameObject _selectedBaitPrefab;
    [SerializeField, Tooltip("used to enable or disable the lock sprite.")]
    private GameObject _lock;
    [SerializeField, Tooltip("Used to trigger unable to select animation.")]
    private Animator _anim;
    [SerializeField, Tooltip("Used to enable/disable locked audio button.")]
    private GameObject _lockedAudioButton;

    private Button _barrelButton;

    [Header("Barrel State")]
    [SerializeField, Tooltip("Bait type that current barrel button corresponds to")]
    public BaitType BaitType;
    [SerializeField, Tooltip("Vertical offset beneath the barrel that the UI popup will appear")]
    private float _tooltipOffset;
    [Header("SFX")]
    [SerializeField] EventReference selectBait;
    [SerializeField] EventReference tooFull;
    [SerializeField] EventReference cantBuy;


    private void Start()
    {
        // REQUIREMENT: Button Component on Barrel
        if (!TryGetComponent(out _barrelButton))
            throw new System.Exception("Barrel MUST have a Button component.");
    }

    /// <summary>
    /// Handles adding a bait of the appropriate type to the selected bait list
    /// </summary>
    public void BaitToSlot()
    {
        // If there is still space for more bait
        if (_baitSelector.GetRemainingBaitSlots() > 0)
        {
            // Instantiate selected bait into Grid Slot
            GameObject selectedBait = Instantiate(_selectedBaitPrefab, _baitSelector.SelectedBaitParent.transform);
            
            // Assign the bait a type based on the barrel's name
            SelectedBait baitScript = selectedBait.AddComponent<SelectedBait>();
            baitScript.Initialize(BaitType);

            // REQUIREMENT: prefab must have Button Component
            if (!selectedBait.TryGetComponent(out Button button))
                throw new System.Exception("Selected Bait prefab MUST have Button component");
            // Add put back Button functionality
            button.onClick.AddListener(() => baitScript.PutBack());

            // one less slot that can be filled
            _baitSelector.DecreaseRemainingBaitSlots();

            // clicking a barrel will automatically close the confirmation popup
            _baitSelector.CancelConfirmationPopup();

            // play select bait audio
            SoundManager.Instance.PlayOneShot(selectBait, gameObject.transform.position);
        }
        else
        {
            _anim.SetTrigger("Shake");

            // play negative feedback audio
            SoundManager.Instance.PlayOneShot(tooFull, gameObject.transform.position);
        }
    }

    /// <summary>
    /// called when mouse hovers over barrel.
    /// </summary>
    public void OnHover()
    {
        // Display UI tooltip: "Purchase at shop" / Locked
        if(!_barrelButton.interactable)
        {
            _baitSelector.LockedTooltip.transform.position = transform.position + Vector3.down * _tooltipOffset;
            _baitSelector.LockedTooltip.SetActive(true);
        }
    }

    /// <summary>
    /// called the frame that the mouse no longer hovers over barrel.
    /// </summary>
    public void OnExit()
    {
        // Ensure tooltip is hidden
        _baitSelector.LockedTooltip.SetActive(false);
    }

    /// <summary>
    /// Configures barrel to be properly locked.
    /// </summary>
    public void SetLocked(bool newState)
    {
        _lock.SetActive(newState);
        _lockedAudioButton.SetActive(newState);
    }

    /// <summary>
    /// Plays audio feedback for clicking on a locked barrel.
    /// Also shakes the barrel.
    /// </summary>
    public void OnLockedBarrelClick()
    {
        _anim.SetTrigger("Shake");

        // play negative feedback audio
        SoundManager.Instance.PlayOneShot(cantBuy, gameObject.transform.position);
    }
}
