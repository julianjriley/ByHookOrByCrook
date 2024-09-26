using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    ///        a. ON HOVER: Display name & description tooltip ** STOPPED HERE: 09-25 11:37pm
    ///        b. ON CLICK: Move into correct slot
    ///     3. Display PRACTICE? or FIGHT!  
    ///         a. ON CLICK: Change scene

    [SerializeField]
    private List<Button> _caughtFishButtons; 
    [SerializeField]
    private List<Button> _chosenFishButtons;

    private GameManager _gameManager;
    private Inventory _caughtFish;

    [SerializeField]
    private Button spawnedItem;

    // Spawnpoints in the grid layout so our items follow a nice grid
    public GameObject caughtItemsSpawn;
    public GameObject chosenItemsSpawn;

    // Parent GameObject that contains all UI assets for the fish desctriptions
    public GameObject descriptionToolTip;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _caughtFish = _gameManager.ScenePersistent.CaughtFish;

        // Below instantiates the item loadout into grid layout
        foreach (var item in _caughtFish.items)
        {
            // Here we spawn buttons into the grid layout and assign them to the items
            Button fishClone = Instantiate(spawnedItem, caughtItemsSpawn.transform);
            FishButtons fishButtonClone = fishClone.AddComponent<FishButtons>();
            fishButtonClone.AssignItem(item);

            // Automatically adds OnClick function to spawned buttons
            fishClone.onClick.AddListener(() => MoveToLoadout(fishClone));

            // Trying to add OnHover function by adding Event Trigger "Pointer Enter" Event Type programatically
            EventTrigger trigger = fishClone.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventdata) => { OnHover();});

            _caughtFishButtons.Add(fishClone);
        }
    }

    public void OnHover()
    {
        descriptionToolTip.SetActive(true);
    }

    public void MoveToLoadout(Button fish)
    {
        // This function moves the fish items between CAUGHT and CHOSEN slots

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
