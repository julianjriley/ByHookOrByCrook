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
    ///     1. shows all items the player has caught-- each are a button the player clicks on
    ///         a. determined by how much bait the player casts
    ///     2. clicking on an item puts it into your loadout slots-- determined by backpack upgrade
    ///        a. ON HOVER: display description+effect in a pop-up textbox below the item
    ///        b. ON CLICK: call a function that greys out the item AND move it into the correct slot
    ///     3. display PRACTICE? or FIGHT!  
    ///         a. ON CLICK, change scene




    // Spawnpoints in the grid layout so our items follow a nice grid
    public GameObject caughtItemsSpawn; 
    public GameObject chosenItemsSpawn;

    [SerializeField]
    private List<Button> _caughtFishButtons; 
    [SerializeField]
    private List<Button> _chosenFish;

    private GameManager _gameManager;
    private Inventory _caughtFish;

    private Button spawned;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _caughtFish = _gameManager.ScenePersistent.CaughtFish;
        foreach (var item in _caughtFish.items)
        {
            Button fishClone = Instantiate(spawned, caughtItemsSpawn.transform);
            FishButtons fishButtonClone = fishClone.AddComponent<FishButtons>();
            fishButtonClone.AssignItem(item);
        }

        // get reference from game manager to access
        //      1. list of catches from fishing minigame
        //      2. list of catches you choose to bring into battle   
        // loop through the lists and instantiate each button into the grid layout
                    // each button has a reference to the scriptable objects
    }

    void Update()
    {

    }

    public void MoveToLoadout(Button fish)
    {
        
        if (fish.GetComponent<FishButtons>().isChosen) {
            MoveBack(fish);
        }
        else
        {
            fish.GetComponent<FishButtons>().isChosen = true;
            Button fishClone = Instantiate(fish, chosenItemsSpawn.transform);
            _chosenFish.Add(fishClone);
                        
            if (fish != null)
            {
                _caughtFishButtons.Remove(fish);
                fish.gameObject.SetActive(false);
                StartCoroutine(Waiting(fish.gameObject));
               
            }
        }
    }

    IEnumerator Waiting(GameObject f)
    {
        yield return new WaitForSeconds(3f);
        Destroy(f);
    }
    public void MoveBack(Button fish)
    {
        fish.GetComponent<FishButtons>().isChosen = false;
        Button fishClone = Instantiate(fish, caughtItemsSpawn.transform);
        _caughtFishButtons.Add(fishClone);

        if (fish != null)
        {
            _chosenFish.Remove(fish);
            fish.gameObject.SetActive(false);
            StartCoroutine(Waiting(fish.gameObject));
        }

    }

    void NextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        
        // For Fadeout, call the Fadeout coroutine
    }


    //public IEnumerator Fadeout(int index)
    //{
    //    fade.FadeIn();
    //    Turn off all of the assets
    //    yield return new WaitForSeconds(1);
    //    SceneManager.LoadScene(index);

    //}


}
