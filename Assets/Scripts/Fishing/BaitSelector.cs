using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaitSelector : MonoBehaviour
{
    public List<Sprite> _baitSprites = new List<Sprite>();
    public List<Button> _barrelList; // For now, I will manually add barrels to this list
    
    private GameManager _gameManager;
    private BarrelScript _barrelScript;

    public GameObject baitSlotSpawn;

    private int baitSlots = 3;

    public GameObject TooltipParent;
    public GameObject TooltipSpawn;

    public List<Vector2> ToolTipSpawnpoints;

    void Start()
    {
        _gameManager = GameManager.Instance;
        TooltipParent.SetActive(false);

        // Initialize each barrel as locked, shouldn't include the basic bait barrel
        foreach (var button in _barrelList)
        {
            if (button.GetComponent<BarrelScript>() == null)
            {
                _barrelScript = button.AddComponent<BarrelScript>();
            }
            _barrelScript = button.GetComponent<BarrelScript>();

            // Weapon bait unlocked for prototype
            if (_barrelScript.gameObject.name != "Basic Bait"  && _barrelScript.gameObject.name != "Weapon Bait") {
                _barrelScript.locked = true;
            }

            if (!_barrelScript.locked)
            {
                // Make barrel clickable
                _barrelScript.GetComponent<Button>().interactable = true;
            }
            else
            {
                _barrelScript.GetComponent<Button>().interactable = false;
            }
        }
    }

    public int GetBaitSlots()
    {
        return baitSlots;
    }

    public void DecreaseBaitSlot()
    {
        baitSlots--;
    }
    public void increaseBaitSlot()
    {
        baitSlots++;
    }
}
