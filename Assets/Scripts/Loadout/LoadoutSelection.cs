using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadoutSelection : MonoBehaviour
{
    // This script will be used for the Weapon Loadout Screen

    // ** CURRENT BUGS: 
    //      1. On click, the buttons are removed before onclick finishes, causes Event System Error. 
    //          Current workaround- starting a coroutine to let onclick finish while using SetActive = false.

    ///     TODO:
    ///     1. Shows all items caught-- each are a button the player clicks on
    ///     2. Clicking on an item puts it into your loadout slots-- determined by backpack upgrade
    ///        a. ON HOVER: Display name & description tooltip
    ///        b. ON CLICK: Move into correct slot
    ///     3. Display PRACTICE? or FIGHT!  
    ///         a. ON CLICK: Change scene

    [SerializeField]
    private List<Button> _caughtFishButtons; 
    [SerializeField]
    private List<Button> _chosenFishButtons;

    private GameManager _gameManager;
    private Inventory _caughtFish;

    private Button spawnedItem;

    // Spawnpoints in the grid layout so our items follow a nice grid
    public GameObject caughtItemsSpawn;
    public GameObject chosenItemsSpawn;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _caughtFish = _gameManager.ScenePersistent.CaughtFish;

        // Below instantiates the item loadout into grid layout
        foreach (var item in _caughtFish.items)
        {
            // Here we spawn buttons and assign them to the items
            Button fishClone = Instantiate(spawnedItem, caughtItemsSpawn.transform);
            FishButtons fishButtonClone = fishClone.AddComponent<FishButtons>();
            fishButtonClone.AssignItem(item);
            fishClone.onClick.AddListener(() => MoveToLoadout(fishClone));
            _caughtFishButtons.Add(fishClone);
        }
    }

    public void MoveToLoadout(Button fish)
    {
        // This function moves the button between CAUGHT and CHOSEN slots

        if (fish.GetComponent<FishButtons>().isChosen) {
            MoveBack(fish);
        }
        else
        {
            fish.GetComponent<FishButtons>().isChosen = true;
            Button fishClone = Instantiate(fish, chosenItemsSpawn.transform);
            _chosenFishButtons.Add(fishClone);
                        
            if (fish != null)
            {
                _caughtFishButtons.Remove(fish);
                fish.gameObject.SetActive(false);
                StartCoroutine(Waiting(fish.gameObject));
               
            }
        }
    }

    IEnumerator Waiting(GameObject _fishGameObject)
    {
        yield return new WaitForSeconds(3f);
        Destroy(_fishGameObject);
    }
    public void MoveBack(Button fish)
    {
        fish.GetComponent<FishButtons>().isChosen = false;
        Button fishClone = Instantiate(fish, caughtItemsSpawn.transform);
        _caughtFishButtons.Add(fishClone);

        if (fish != null)
        {
            _chosenFishButtons.Remove(fish);
            fish.gameObject.SetActive(false);
            StartCoroutine(Waiting(fish.gameObject));
        }
    }

    void NextScene(int sceneIndex)
    {
        // Use this function to transition to PRACTICE or COMBAT scene

        StartCoroutine(Fadeout(sceneIndex));
    }


    public IEnumerator Fadeout(int index)
    {
        // Fadeout logic

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
