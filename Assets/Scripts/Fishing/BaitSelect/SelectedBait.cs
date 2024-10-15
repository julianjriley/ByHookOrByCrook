using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

/// <summary>
/// Handles all functions and data relating to SelectedBait buttons for showing and deselecting baits.
/// </summary>
public class SelectedBait : MonoBehaviour
{
    public BaitType BaitType;

    private BaitSelector _baitSelector;
    private Image _img;
 
    public void Initialize(BaitType baitType)
    {
        // REQUIREMENT: BaitSelector exists in scene
        _baitSelector = FindObjectOfType<BaitSelector>();
        if (_baitSelector is null)
            throw new System.Exception("BaitSelector object MUST be present in bait select scene.");

        // REQUIREMENT: prefab must have Image Component
        if (!TryGetComponent(out _img))
            throw new System.Exception("Selected Bait prefab MUST have Image Component.");

        // set bait type
        BaitType = baitType;

        // set image to match bait
        _img.sprite = _baitSelector.BaitSprites[(int) BaitType];
    }

    /// <summary>
    /// Removes this selected bait from the currently selected list.
    /// </summary>
    public void PutBack()
    {
        // free up a slot so a different bait can be picked
        _baitSelector.IncreaseRemainingBaitSlots();

        // Pseudo-deletion (avoids MissingReferenceException issues with Unity event system)
        Destroy(this); // remove SelectedBait component
        gameObject.SetActive(false);
    }
}
