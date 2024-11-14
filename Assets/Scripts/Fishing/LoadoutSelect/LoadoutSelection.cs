using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Editor Only")]
    [SerializeField, Tooltip("List of fish used when starting Unity within this scene. Instead of using GameManager.")]
    private List<Item> _editorFishList;

    [Header("Boss Scenes")]
    [SerializeField] string[] _bossScenes;
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
    /// Transitions to next scene (either combat or practice arena)
    /// </summary>
    public void NextScene()
    {
        // TODO: Confirmation popup if player is attempting to continue without filling all of their bait slots

        // add plush if toggle is on
        if (_defaultFishToggle.isOn)
            GameManager.Instance.AddLoadoutItem(_starterGun);
        
        // Add selected loadout items to GameManager
        FishButton[] loadoutFish = LoadoutFishParent.GetComponentsInChildren<FishButton>();
        foreach (FishButton fish in loadoutFish)
            GameManager.Instance.AddLoadoutItem(fish.Item);

        // Use this function to transition to PRACTICE or COMBAT scene
        string sceneToSwitchTo;
        switch (GameManager.Instance.GamePersistent.BossNumber)
        { 
            case 0:
                sceneToSwitchTo = _bossScenes[0]; break;
            case 1:
                sceneToSwitchTo= _bossScenes[1]; break;
            case 2:
                sceneToSwitchTo=_bossScenes[2]; break;
            default:
                sceneToSwitchTo = _bossScenes[0]; break;
        }

        StartCoroutine(Fadeout(sceneToSwitchTo));
    }

    private IEnumerator Fadeout(string sceneName)
    {
        // TODO: add actual smooth transition visual effects here

        // Fadeout logic/delay
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Current number of selected fish.
    /// </summary>
    public int GetCurrentLoadoutSize()
    {
        return LoadoutFishParent.transform.childCount;
    }
}
