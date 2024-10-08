using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class BarrelScript : MonoBehaviour
{
    public bool locked;
    public BaitSelector baitSelector;

    [SerializeField] 
    private Button spawnedBait;
    private int _baitSlots;
    private Vector2 _tooltipSpot;

    public void BaitToSlot(int baitType)
    {
        // If barrels aren't locked and we have slots
        if (!locked && baitSelector.GetBaitSlots() != 0)
        {
            // Instantiate bait into Grid Slot
            Button baitButton = Instantiate(spawnedBait, baitSelector.baitSlotSpawn.transform);
            
            // Assign the bait a type based on the barrel's name
            Bait baitScript = baitButton.AddComponent<Bait>();
            baitScript.AssignBaitType(gameObject.name);
            baitButton.onClick.AddListener(() => baitScript.PutBack(baitType));

            //Add bait to scene persistent inventory
            GameManager.Instance.ScenePersistent.BaitList.Add((GameManager.BaitType) baitType);
            baitSelector.DecreaseBaitSlot();
        }
    }

    public void AssignTooltipSpawn(int index)
    {
        // Assigns the tooltip's spawnpoint for each barrel
        _tooltipSpot = baitSelector.ToolTipSpawnpoints[index];
    }

    public void OnHover()
    {
        // Display UI tooltip: "Purchase at shop"
        if (locked)
        {
            baitSelector.TooltipParent.transform.localPosition = _tooltipSpot;
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
