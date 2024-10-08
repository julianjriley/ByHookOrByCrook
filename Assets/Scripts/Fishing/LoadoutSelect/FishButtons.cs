using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishButtons : MonoBehaviour
{
    public bool isChosen = false;

    private LoadoutSelection _loadoutSelection;

    // Display items in game
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _itemDescription;
    private Image _sprite;
    public GameObject tooltipChild;

    public void AssignItem(Item fishItem)
    {
        // This function assigns the fish properties to properties that appear in game

        tooltipChild = this.gameObject.transform.GetChild(0).gameObject;
        _itemName = tooltipChild.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _itemDescription = tooltipChild.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _sprite = tooltipChild.transform.GetChild(3).GetComponent<Image>();

        _itemName.text = fishItem.GetItemName(); 
        _itemDescription.text = fishItem.GetItemDescription();
        _sprite.sprite = fishItem.GetSprite();
    }

}
