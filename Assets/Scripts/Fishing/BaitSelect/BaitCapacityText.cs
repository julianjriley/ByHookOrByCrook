using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaitCapacityText : MonoBehaviour
{
    [SerializeField, Tooltip("Text to be updated based on slots.")]
    private TextMeshProUGUI _text;
    [SerializeField, Tooltip("Used to get current number of used slots.")]
    private BaitSelector _baitSelector;

    // Update is called once per frame
    void Update()
    {
        _text.text = _baitSelector.GetCurrentFullSlots() + " / " + GameManager.Instance.GamePersistent.BaitInventorySize;
    }
}
