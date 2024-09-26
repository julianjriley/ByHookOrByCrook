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
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    // Note: Only Image items appear correctly on UI
    public Image sprite; 

    public void AssignItem(Item fishItem)
    {
        // This function assigns the scriptable object properties to properties that appear in game
        scriptableObject = fishItem;
        itemName.text = fishItem.GetItemName();
        itemDescription.text = fishItem.GetItemDescription();
        sprite.sprite = fishItem.GetSprite();
    }
    public void OnHover()
    {
        // TODO: Make sprite box with text appear
        // Buttons will need an Event Trigger "Pointer Enter" Event Type added to them
    }
}
