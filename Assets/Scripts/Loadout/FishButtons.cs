using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishButtons : MonoBehaviour
{
    public bool isChosen = false;

    Item scriptableObject;

    // Display items in game
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _itemDescription;
    private Image _sprite;


    public void AssignItem(Item fishItem)
    {
        scriptableObject = fishItem;
    }

    // we assign UI tooltip stuff by referencing the information from the scriptableObject
    public void OnHover()
    {
        
        // Instantiate prefab with UI assets
        // Based on what item it is, display the item name, description, and sprite 
    }
}
