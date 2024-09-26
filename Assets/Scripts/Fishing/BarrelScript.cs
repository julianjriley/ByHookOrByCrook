using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarrelScript : MonoBehaviour
{
    // Locked or unlocked
    public bool locked;
    public BaitSelector baitSelector;

    [SerializeField] 
    private Button spawnedBait;

    private int _baitSlots;

    public void BaitToSlot()
    {
        // If barrels aren't locked and we have slots
        if (!locked && baitSelector.GetBaitSlots() != 0)
        {
            // Instantiate bait into Grid Slot
            Button baitButton = Instantiate(spawnedBait, baitSelector.baitSlotSpawn.transform);
            
            // Assign the bait a type based on the barrel's name
            Bait baitScript = baitButton.AddComponent<Bait>();
            baitScript.AssignBaitType(gameObject.name);
            baitButton.onClick.AddListener(() => baitScript.PutBack());

            baitSelector.DecreaseBaitSlot();
        }
    }

    public void OnHover()
    {
        // Display UI tooltip: "Purchase at shop"
        if (locked)
        {
            baitSelector.TooltipParent.SetActive(true);
        }

        // Insert shake animation here
    }

    public void OnExit()
    {
        // Turn off UI tooltip
        if (locked)
        {
            baitSelector.TooltipParent.SetActive(false);
        }
    }
}
