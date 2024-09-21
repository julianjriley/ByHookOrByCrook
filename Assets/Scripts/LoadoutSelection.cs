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

    public GameObject caughtFishInvis;
    public GameObject loadoutInvis;
    public float padding;

    [SerializeField]
    private List<Button> _caughtFish;
    [SerializeField]
    private List<Button> _chosenFish;
    
    //private bool _isChosen = false;

    void Start()
    {
        // get reference from game manager to access
        //      1. list of catches from fishing minigame
        //      2. list of catches you choose to bring into battle
    }

    void Update()
    {
        ///     shows all items the player has caught-- each are a button the player clicks on
        ///         a. determined by how much bait the player casts
        ///     clicking on an item puts it into your loadout slots-- determined by backpack upgrade
        ///        a. ON HOVER: display description+effect in a pop-up textbox below the item
        ///        b. ON CLICK: call a function that greys out the item AND move it into the correct slot
        ///     display PRACTICE? or FIGHT!  
        ///         a. ON CLICK, change scene

    }

    public void OnHover()
    {
        // Instantiate prefab with UI assets
        // Based on what item it is, display the item name, description, and sprite 
    }

    public void MoveToLoadout(Button fish)
    {
        
        if (fish.GetComponent<FishButtons>().isChosen) {
            MoveBack(fish);
            Debug.Log("move fish back to caught" + fish);
        }
        else
        {
            fish.GetComponent<FishButtons>().isChosen = true;
            Button fishClone = Instantiate(fish, loadoutInvis.transform);
            _chosenFish.Add(fishClone);
            

            Debug.Log("move fish back to loadout" + fish.gameObject);
            
            if (fish != null)
            {
                _caughtFish.Remove(fish);
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
        Button fishClone = Instantiate(fish, caughtFishInvis.transform);
        _caughtFish.Add(fishClone);

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
