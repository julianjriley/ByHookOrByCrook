using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadoutCapacityText : MonoBehaviour
{
    [SerializeField, Tooltip("Text to be updated based on slots.")]
    private TextMeshProUGUI _text;
    [SerializeField, Tooltip("Used to get current number of used slots.")]
    private LoadoutSelection _loadoutSelection;

    // Update is called once per frame
    void Update()
    {
        _text.text = _loadoutSelection.GetCurrentLoadoutSize() + " / " + GameManager.Instance.GamePersistent.BattleInventorySize;
    }
}
