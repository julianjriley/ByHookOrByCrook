using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles enabling and scaling catch UI after a catch.
/// Also handles the check for if the scene is ready to transition
/// </summary>
public class CatchUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Used for setting sprite properly upon new catch.")]
    private Image _image;
    [SerializeField, Tooltip("Used for changing size of popup.")]
    private RectTransform _rect;

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

        // pause while max scale
        yield return new WaitForSeconds(_freezeTime);

        // TODO: some wiggle effect when popup is actually shown at full scale.

        // decreasing scale
        while(_rect.sizeDelta.x > _snappingThreshold)
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
