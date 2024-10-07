using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ** CURRENT BUGS: 
//      On click, the buttons are removed before onclick finishes. It causes an Event System Error. 
//          Current workaround- starting a coroutine to let onclick finish while using SetActive = false.

public class LoadoutSelection : MonoBehaviour
{
    // This script will be used for the Weapon Loadout Screen

    // Temporary list of scriptable objects
    public List<Item> TempListofFish;
    [SerializeField]
    private List<Button> _caughtFishButtons; 
    [SerializeField]
    private List<Button> _chosenFishButtons;

    private GameManager _gameManager;
    private Inventory _caughtFish;

    [SerializeField]
    private Button _buttonPrefab;

    // Spawnpoints in the grid layout so our items follow a nice grid
    public GameObject caughtItemsSpawn;
    public GameObject chosenItemsSpawn;

    private GameObject _tooltipChild;
    void Start()
    {
        _gameManager = GameManager.Instance;
        _caughtFish = _gameManager.ScenePersistent.CaughtFish;

        foreach (var item in TempListofFish) // Change list to GM list later 
        {
            // Spawns buttons into grid and assigns each to a scriptable objects' properties

             Button _spawnedFish = Instantiate(_buttonPrefab, caughtItemsSpawn.transform);
            _spawnedFish.GetComponent<FishButtons>().AssignItem(item);
            _caughtFishButtons.Add(_spawnedFish.GetComponent<Button>());
        }
    }

    public void OnHover(GameObject tooltipChild)
    {
        tooltipChild.SetActive(true);
    }
    public void OnExit(GameObject tooltipChild)
    {
        tooltipChild.SetActive(false);
    }

    public void MoveToLoadout(Button fish)
    {
        // This function moves the fish items between CAUGHT and CHOSEN slots
        
        fish.transform.GetChild(0).gameObject.SetActive(false);

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
