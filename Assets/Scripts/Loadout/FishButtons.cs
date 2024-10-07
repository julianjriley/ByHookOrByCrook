using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishButtons : MonoBehaviour
{
    public bool isChosen = false;

    Item scriptableObject;
    private LoadoutSelection _loadoutSelection;

    // Display items in game
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _itemDescription;

    // Note: Only Image items appear correctly on UI
    private Image _sprite; 

    public void AssignItem(Item fishItem)
    {
        // This function assigns the fish properties to properties that appear in game

        Debug.Log("AssignItem() is called");
        _loadoutSelection = FindAnyObjectByType<LoadoutSelection>();
        _itemName = _loadoutSelection.fishName;
        _itemDescription = _loadoutSelection.fishDescription;
        _sprite = _loadoutSelection.fishIcon;

        scriptableObject = fishItem;
        _itemName.text = fishItem.GetItemName(); // ERROR HERE because TMPGUI elements are null
        _itemDescription.text = fishItem.GetItemDescription();
        _sprite.sprite = fishItem.GetSprite();
    }

}
