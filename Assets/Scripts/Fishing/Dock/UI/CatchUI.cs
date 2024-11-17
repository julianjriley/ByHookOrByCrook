using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles enabling and scaling catch UI after a catch.
/// Also handles the check for if the scene is ready to transition
/// </summary>
public class CatchUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Used for setting sprite properly upon new catch.")]
    private Image _image;
    [SerializeField, Tooltip("Text for caught fish to be updated upon new catch.")]
    private TextMeshProUGUI _text;
    [SerializeField, Tooltip("Used to enable/disable")]
    private Image _textBacker;
    [SerializeField, Tooltip("Image for indicating icon type to be updated upon new catch.")]
    private Image _iconImage;
    [SerializeField, Tooltip("Used for changing size of popup.")]
    private RectTransform _rect;
    [SerializeField, Tooltip("Ordered sprites for icon types.")]
    private Sprite[] _iconSprites;

    [Header("Behavior")]
    [SerializeField, Tooltip("Height and width of sprite at maximum scale.")]
    private float _maxWidth;
    [SerializeField, Tooltip("'Snappiness' of expanding and shrinking of popup.")]
    private float _scaleSharpness;
    [SerializeField, Tooltip("Distance from goal width at which smooth lerping will snap to goal.")]
    private float _snappingThreshold;
    [SerializeField, Tooltip("Time during which the popup stays at max scale before shrinking.")]
    private float _freezeTime;

    [Header("Scene Transition")]
    [SerializeField, Tooltip("Scene name of the loadout scene to follow this one")]
    private string _loadoutSceneName;

    private int _catchCount = 0;

    // Update is called once per frame
    void Update()
    {
        // a new catch has been made
        if (GameManager.Instance.ScenePersistent.CaughtFish.Count > _catchCount)
        {
            StartCoroutine(UIPopup());

            _catchCount++;
        }

        // TODO: smoother transition effect
        // Scene transition to end fishing
        if (GameManager.Instance.ScenePersistent.BaitList.Count == 0 && !IsPopupActive())
            SceneManager.LoadScene(_loadoutSceneName);
    }

    /// <summary>
    /// Handles smooth lerping from small to large, holds at large size, then scales back down.
    /// </summary>
    private IEnumerator UIPopup()
    {
        // Configure / enable popup
        _rect.sizeDelta = Vector2.zero;
        _image.sprite = GameManager.Instance.PeekCaughtFish().GetSprite();
        _image.enabled = true;

        // still increasing
        while(_rect.sizeDelta.x < _maxWidth - _snappingThreshold)
        {
            // smoothly lerp width size UP
            float newWidth = Mathf.Lerp(_rect.sizeDelta.x, _maxWidth, 1f - Mathf.Exp(-_scaleSharpness * Time.deltaTime));
            _rect.sizeDelta = new Vector2(newWidth, newWidth);
            
            yield return null;
        }
        // ensure snapped to full max size
        _rect.sizeDelta = new Vector2(_maxWidth, _maxWidth);

        // TODO: some wiggle effect when popup is actually shown at full scale.

        // update text and show only at full scale
        _text.text = GameManager.Instance.PeekCaughtFish().GetItemName();
        _text.enabled = true;
        _textBacker.enabled = true;
        // update icon asset only at full scale
        _iconImage.sprite = _iconSprites[(int)GameManager.Instance.PeekCaughtFish().GetItemType()];
        _iconImage.enabled = true;

        // pause while max scale
        yield return new WaitForSeconds(_freezeTime);

        // decreasing scale
        while (_rect.sizeDelta.x > _snappingThreshold)
        {
            // smoothly lerp width size UP
            float newWidth = Mathf.Lerp(_rect.sizeDelta.x, 0, 1f - Mathf.Exp(-_scaleSharpness * Time.deltaTime));
            _rect.sizeDelta = new Vector2(newWidth, newWidth);

            yield return null;
        }
        // ensure snapped to full min size
        _rect.sizeDelta = Vector2.zero;

        // Done - disable image
        _image.enabled = false;
        _iconImage.enabled = false;
        _text.enabled = false;
        _textBacker.enabled = false;
    }

    /// <summary>
    /// Returns whether UI popup is still on screen.
    /// Useful for locking fishing controls until popup is done.
    /// </summary>
    public bool IsPopupActive()
    {
        return _image.enabled;
    }
}
