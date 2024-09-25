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

    // Note: Only Image items appear correctly on UI
    private Image _sprite; 

    public void AssignItem(Item fishItem)
    {
        // This function assigns the scriptable object properties to properties that appear in game
        scriptableObject = fishItem;
        _itemName.text = fishItem.GetItemName();
        _itemDescription.text = fishItem.GetItemDescription();
        _sprite.sprite = fishItem.GetSprite();
    }
    public void OnHover()
    {
        // TODO: Make sprite box with text appear
        // Buttons will need an Event Trigger "Pointer Enter" Event Type added to them
    }
}
