using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
                barrelScript.SetLockVisual(!button.interactable);
        }
    }

    public int GetRemainingBaitSlots()
    {
        return _remainingBaitSlots;
    }

    public void DecreaseRemainingBaitSlots()
    {
        _remainingBaitSlots--;

        if (_remainingBaitSlots < 0)
            throw new System.Exception("Error: attempting to add a selected bait when no slots remain");
    }
    public void IncreaseRemainingBaitSlots()
    {
        _remainingBaitSlots++;

        if (_remainingBaitSlots > GameManager.Instance.GamePersistent.BaitInventorySize)
            throw new System.Exception("Error: attempting to remove a selected bait when all are already removed.");
    }

    /// <summary>
    /// Simple scene transition
    /// </summary>
    public void BackToHub(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Handles properly uploading finalized bait data to game manager, then loads next scene
    /// </summary>
    public void ContinueToFishing(string sceneName)
    {
        // TODO: Confirmation popup if player is attempting to continue without filling all of their bait slots

        // Add selected baits to GameManager
        SelectedBait[] selectedBaits = SelectedBaitParent.GetComponentsInChildren<SelectedBait>();
        foreach (SelectedBait bait in selectedBaits)
            GameManager.Instance.AddBait(bait.BaitType);

        // Load fishing scene
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Returns current number of slots that are full.
    /// </summary>
    public int GetCurrentFullSlots()
    {
        return GameManager.Instance.GamePersistent.BaitInventorySize - _remainingBaitSlots;
    }
}
