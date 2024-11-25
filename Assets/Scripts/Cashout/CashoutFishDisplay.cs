using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles initialization of fish display for cashout scene.
/// </summary>
public class CashoutFishDisplay : MonoBehaviour
{
    [SerializeField, Tooltip("Text to update for fish name.")]
    private TextMeshProUGUI _fishName;
    [SerializeField, Tooltip("Text to update for fish money amount.")]
    private TextMeshProUGUI _fishAmount;
    [SerializeField, Tooltip("Image to update with fish image.")]
    Image _fishImage;

    /// <summary>
    /// configures display based on provided parameters
    /// </summary>
    public void Initialize(string fishName, string fishAmount, Sprite fishSprite)
    {
        _fishName.text = fishName;
        _fishAmount.text = fishAmount;
        _fishImage.sprite = fishSprite;
    }
}
