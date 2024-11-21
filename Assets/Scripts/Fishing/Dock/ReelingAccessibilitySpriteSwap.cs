using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles configuring sprites of Reeling UI to match whether the player is or isn't using the accessibility bobber.
/// </summary>
public class ReelingAccessibilitySpriteSwap : MonoBehaviour
{
    [Header("Images")]
    [SerializeField, Tooltip("Image for shrinking circl.")]
    private SpriteRenderer _shrinkingCircleImage;
    [SerializeField, Tooltip("Image for shrinking circle inner border.")]
    private SpriteRenderer _innerBorderImage;
    [SerializeField, Tooltip("Image for shrinking circle outer border.")]
    private SpriteRenderer _outerBorderImage;

    [Header("Sprites")]
    [SerializeField, Tooltip("Sprite for Accessibility bobber.")]
    private Sprite _thickerSprite;
    [SerializeField, Tooltip("Sprite for non-accessibility bobber.")]
    private Sprite _thickSprite;

    // Start is called before the first frame update
    void Awake()
    {
        // accessibility
        if (GameManager.Instance.GamePersistent.IsBobber)
        {
            _shrinkingCircleImage.sprite = _thickerSprite;
            _innerBorderImage.sprite = _thickerSprite;
            _outerBorderImage.sprite = _thickerSprite;
        }
        // non-accessibility
        else
        {
            _shrinkingCircleImage.sprite = _thickSprite;
            _innerBorderImage.sprite = _thickSprite;
            _outerBorderImage.sprite = _thickSprite;
        }    
    }
}