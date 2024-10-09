using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Editor Only")]
    [SerializeField, Tooltip("List of fish used when starting Unity within this scene. Instead of using GameManager.")]
    private List<Item> _editorFishList;

    void Start()
    {
        // Always add fish button for starter gun
        CreateFishButton(_starterGun);

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
        _spawnedFish.GetComponent<FishButtons>().Initialize(item);
    }

    /// <summary>
    /// Transitions to next scene (either combat or practice arena)
    /// </summary>
    public void NextScene(string sceneName)
    {
        // TODO: Confirmation popup if player is attempting to continue without filling all of their bait slots

        // Add selected loadout items to GameManager
        FishButtons[] loadoutFish = LoadoutFishParent.GetComponentsInChildren<FishButtons>();
        foreach (FishButtons fish in loadoutFish)
            GameManager.Instance.AddLoadoutItem(fish.Item);

        // Use this function to transition to PRACTICE or COMBAT scene
        StartCoroutine(Fadeout(sceneName));
    }

    private IEnumerator Fadeout(string sceneName)
    {
        // TODO: add actual smooth transition visual effects here

        // Fadeout logic/delay
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
