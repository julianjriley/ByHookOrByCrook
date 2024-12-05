using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class BaitSelector : MonoBehaviour
{
    [Header("Barrels")]
    [Tooltip("List of barrel buttons for adding baits to bait list")]
    public List<Button> BarrelList; // For now, I will manually add barrels to this list
    [Tooltip("List of bait sprites for the possible types (in order)")]
    public List<Sprite> BaitSprites = new List<Sprite>();
    [Tooltip("List of type sprites for possible types (in order)")]
    public List<Sprite> TypeSprites = new List<Sprite>();

    [Header("Locked Tooltips")]
    [Tooltip("Used to make locked tooltip enable/disable appropriately.")]
    public GameObject LockedTooltip;

    [Header("Selected Bait")]
    [SerializeField, Tooltip("Object to which selected bait objects are instantiated to.")]
    public GameObject SelectedBaitParent;
    [Header("SFX")]
    [SerializeField] EventReference returnSound;

    private int _remainingBaitSlots;

    void Start()
    {
        // Initialize Selected baits capacity
        _remainingBaitSlots = GameManager.Instance.GamePersistent.BaitInventorySize;

        // Initialized locked tooltip
        LockedTooltip.SetActive(false);

        // Initialize each barrel
        foreach (var barrel in BarrelList)
        {
            // REQUIREMENT: BarrelScript Component
            if (!barrel.TryGetComponent(out Barrel barrelScript))
                throw new System.Exception("Each Barrel MUST have BarrelScript component.");

            // REQUIREMENT: Button Component
            if (!barrel.TryGetComponent(out Button button))
                throw new System.Exception("Each Barrel MUST have a Button component.");

            // set barrel button's interactable state based on whether specific bait type has been unlocked
            if (barrelScript.BaitType == GameManager.BaitType.Default)
                button.interactable = true; // always unlocked
            else if (barrelScript.BaitType == GameManager.BaitType.Weapon)
                button.interactable = GameManager.Instance.GamePersistent.WeaponBait;
            else if (barrelScript.BaitType == GameManager.BaitType.Attack)
                button.interactable = GameManager.Instance.GamePersistent.AttackBait;
            else if (barrelScript.BaitType == GameManager.BaitType.Support)
                button.interactable = GameManager.Instance.GamePersistent.SupportBait;
            else if (barrelScript.BaitType == GameManager.BaitType.Movement)
                button.interactable = GameManager.Instance.GamePersistent.MovementBait;
            else
                throw new System.Exception("Barrel MUST be assigned to a valid BaitType.");

            // update lock visual
            if(barrelScript.BaitType != GameManager.BaitType.Default)
                barrelScript.SetLocked(!button.interactable);
        }
    }

    public int GetRemainingBaitSlots()
    {
        return _remainingBaitSlots;
    }

    /// <summary>
    /// Handles changes to remaining bait counter.
    /// Also enables continue button.
    /// </summary>
    public void DecreaseRemainingBaitSlots()
    {
        _remainingBaitSlots--;

        if (_remainingBaitSlots < 0)
            throw new System.Exception("Error: attempting to add a selected bait when no slots remain");

        // since a bait was added, we know there is AT LEAST one bait
        _continueButton.SetActive(true);
    }

    /// <summary>
    /// Handles changes to remaining bait counter.
    /// Also handles disabling continue button if all bait deselected.
    /// </summary>
    public void IncreaseRemainingBaitSlots()
    {
        _remainingBaitSlots++;

        if (_remainingBaitSlots > GameManager.Instance.GamePersistent.BaitInventorySize)
            throw new System.Exception("Error: attempting to remove a selected bait when all are already removed.");

        // if we have ALL slots remaining, then no bait is selected
        if (_remainingBaitSlots == GameManager.Instance.GamePersistent.BaitInventorySize)
            _continueButton.SetActive(false);
    }

    public void PlayAudio()
    {
        SoundManager.Instance.PlayOneShot(returnSound, gameObject.transform.position);
    }
    #region SCENE TRANSITIONS
    [Header("Scene Transitions")]
    [SerializeField, Tooltip("Name of hub scene to transition back to.")]
    private string _hubSceneName;
    [SerializeField, Tooltip("Name of fishing scene to transition to.")]
    private string _fishingSceneName;
    [SerializeField, Tooltip("Game object to be activated for confirming bait selection when all slots are not full.")]
    private GameObject _confirmationPopup;
    [SerializeField, Tooltip("Game object to be activated to ensure player can only continue with AT LEAST one bait.")]
    private GameObject _continueButton;
    [SerializeField, Tooltip("Used to actually call scene transitions.")]
    private SceneTransitionsHandler _transitionsHandler;

    /// <summary>
    /// Simple scene transition back to hub
    /// </summary>
    public void BackToHub()
    {
        // left transition override for back to hub
        _transitionsHandler.LoadScene(_hubSceneName, SceneTransitionsHandler.TransitionType.SlideLeft);
    }

    /// <summary>
    /// Continues if bait slots are full. Otherwise, brings up Confirmation Popup.
    /// </summary>
    public void TryContinueToFishing()
    {
        // continue if slots are full
        if (_remainingBaitSlots == 0)
            ContinueToFishing();
        // confirm popup
        else
        {
            _confirmationPopup.SetActive(true);
        }
    }

    /// <summary>
    /// Used to go back from the confirmation popup menu
    /// </summary>
    public void CancelConfirmationPopup()
    {
        _confirmationPopup.SetActive(false);
    }

    /// <summary>
    /// Handles properly uploading finalized bait data to game manager, then loads next scene
    /// </summary>
    public void ContinueToFishing()
    {
        // Add selected baits to GameManager
        SelectedBait[] selectedBaits = SelectedBaitParent.GetComponentsInChildren<SelectedBait>();
        foreach (SelectedBait bait in selectedBaits)
            GameManager.Instance.AddBait(bait.BaitType);
        SoundManager.Instance.CleanUp(); //stops the music

        // Load fishing scene
        _transitionsHandler.LoadScene(_fishingSceneName);
    }

    /// <summary>
    /// Returns current number of slots that are full.
    /// </summary>
    public int GetCurrentFullSlots()
    {
        return GameManager.Instance.GamePersistent.BaitInventorySize - _remainingBaitSlots;
    }
    #endregion
}
