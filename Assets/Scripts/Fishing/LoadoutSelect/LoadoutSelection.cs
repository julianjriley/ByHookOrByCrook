using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles Instantiation of FishButtons at start of scene.
/// Stores relevant scene data used elsewhere.
/// Contains button functionality for Practice and Fight buttons.
/// </summary>
public class LoadoutSelection : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField, Tooltip("Scriptable object reference to starter gun. Always added as button.")]
    private Weapon _starterGun;
    [SerializeField, Tooltip("Used for instantiating fish buttons.")]
    private GameObject _fishButtonPrefab;
    [SerializeField, Tooltip("Game object where caught fish buttons are created.")]
    public GameObject CaughtFishParent;
    [SerializeField, Tooltip("Game object where loadout fish buttons are created.")]
    public GameObject LoadoutFishParent;
    [SerializeField, Tooltip("Used to determine if player wants to take plush.")]
    private Toggle _defaultFishToggle;
    [SerializeField, Tooltip("Used to visually indicate when plush toggle is locked.")]
    private GameObject _plushLock;

    [Header("Editor Only")]
    [SerializeField, Tooltip("List of fish used when starting Unity within this scene. Instead of using GameManager.")]
    private List<Item> _editorFishList;

    [HideInInspector]
    public int NumOfWeaponsPicked = 0;

    void Start()
    {
        // Spawn fish button for each caught fish (from GameManager)
        foreach (Item item in GameManager.Instance.ScenePersistent.CaughtFish)
            CreateFishButton(item);

#if UNITY_EDITOR
        if(GameManager.Instance.ScenePersistent.CaughtFish.Count == 0)
        {
            foreach (Item item in _editorFishList)
                CreateFishButton(item);
        }
#endif
    }
    
    // Creates a new fish button of the given item type
    private void CreateFishButton(Item item)
    {
        GameObject _spawnedFish = Instantiate(_fishButtonPrefab, CaughtFishParent.transform);
        _spawnedFish.GetComponent<FishButton>().Initialize(item);
    }

    /// <summary>
    /// Current number of selected fish.
    /// </summary>
    public int GetCurrentLoadoutSize()
    {
        return LoadoutFishParent.transform.childCount;
    }

    /// <summary>
    /// Current number of unselected fish.
    /// </summary>
    public int GetCurrentUnselectedFish()
    {
        return CaughtFishParent.transform.childCount;
    }

    /// <summary>
    /// Removes lock and enables fish toggle.
    /// </summary>
    public void EnablePlushToggle()
    {
        _defaultFishToggle.interactable = true;
        _plushLock.SetActive(false);
    }

    /// <summary>
    /// Sets fish toggle to set and locks it.
    /// </summary>
    public void DisablePlushToggle()
    {
        _defaultFishToggle.isOn = true;
        _defaultFishToggle.interactable = false;
        _plushLock.SetActive(true);
    }

    #region SCENE TRANSITIONS
    [Header("Scene Transitions")]
    [SerializeField, Tooltip("Scene names of the different boss scenes.")] 
    private string[] _bossScenes;
    [SerializeField, Tooltip("Game object to activate for confirmation popup.")]
    private GameObject _combatConfirmationPopup;
    [SerializeField, Tooltip("Name of practice scene to transition to.")]
    private string _practiceSceneName;
    [SerializeField, Tooltip("Game object to activate for confirmation popup.")]
    private GameObject _practiceConfirmationPopup;
    [SerializeField, Tooltip("Used to call scene transitions.")]
    private SceneTransitionsHandler _transitionsHandler;

    /// <summary>
    /// Continues if bait slots are full. Otherwise, brings up Confirmation Popup for combat.
    /// </summary>
    public void TryContinueToCombat()
    {
        // continue if slots are full OR there are no more fish to add to loadout
        if (GetCurrentLoadoutSize() == GameManager.Instance.GamePersistent.BattleInventorySize || GetCurrentUnselectedFish() == 0)
            ContinueToCombat();
        // confirm popup
        else
        {
            _combatConfirmationPopup.SetActive(true);
        }
    }

    /// <summary>
    /// Used to go back from the confirmation popup menu
    /// </summary>
    public void CancelCombatConfirmationPopup()
    {
        _combatConfirmationPopup.SetActive(false);
    }

    /// <summary>
    /// Transitions to next scene (either combat or practice arena)
    /// </summary>
    public void ContinueToCombat()
    {
        // add loadout to game manager
        AddFish();

        // Use this function to transition to PRACTICE or COMBAT scene
        string sceneToSwitchTo;
        switch (GameManager.Instance.GamePersistent.BossNumber)
        {
            case 0:
                sceneToSwitchTo = _bossScenes[0]; break;
            case 1:
                sceneToSwitchTo = _bossScenes[1]; break;
            case 2:
                sceneToSwitchTo = _bossScenes[2]; break;
            default:
                sceneToSwitchTo = _bossScenes[0]; break;
        }

        // actually transition scene
        _transitionsHandler.LoadScene(sceneToSwitchTo);
    }

    /// <summary>
    /// Continues if bait slots are full. Otherwise, brings up Confirmation Popup for practice.
    /// </summary>
    public void TryContinueToPractice()
    {
        // continue if slots are full OR there are no more fish to add to loadout
        if (GetCurrentLoadoutSize() == GameManager.Instance.GamePersistent.BattleInventorySize || GetCurrentUnselectedFish() == 0)
            ContinueToPractice();
        // confirm popup
        else
        {
            _practiceConfirmationPopup.SetActive(true);
        }
    }

    /// <summary>
    /// Used to go back from the confirmation popup menu
    /// </summary>
    public void CancelPracticeConfirmationPopup()
    {
        _practiceConfirmationPopup.SetActive(false);
    }

    /// <summary>
    /// Transitions to next scene (either combat or practice arena)
    /// </summary>
    public void ContinueToPractice()
    {
        // add loadout to game manager
        AddFish();

        // actually transition scene -> DON'T DO WATER TRANSITION FOR PRACTICE
        _transitionsHandler.LoadScene(_practiceSceneName, SceneTransitionsHandler.TransitionType.SlideRight);
    }

    public void AddFish()
    {
        // add plush if toggle is on
        if (_defaultFishToggle.isOn)
            GameManager.Instance.AddLoadoutItem(_starterGun);

        // Add selected loadout items to GameManager
        FishButton[] loadoutFish = LoadoutFishParent.GetComponentsInChildren<FishButton>();
        foreach (FishButton fish in loadoutFish)
            GameManager.Instance.AddLoadoutItem(fish.Item);

        // Add plush gun if no gun found in current loadout
        bool isWeaponFound = false;
        foreach (Item item in GameManager.Instance.ScenePersistent.Loadout)
        {
            if (item.GetItemType() == Item.ItemType.WEAPON)
            {
                isWeaponFound = true;
                break;
            }
        }
        if (!isWeaponFound)
            GameManager.Instance.AddLoadoutItem(_starterGun);
    }
    #endregion
}
