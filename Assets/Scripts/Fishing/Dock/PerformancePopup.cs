using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles coroutine for performance popup to show and then go away.
/// </summary>
public class PerformancePopup : MonoBehaviour
{
    [SerializeField, Tooltip("Whether this is for casting or reeling.")]
    private bool _isCasting = true;
    [SerializeField, Tooltip("snappiness of motion")]
    private float _sharpness;
    [SerializeField, Tooltip("snapping distance of zoom")]
    private float _snappingDistance;
    [SerializeField, Tooltip("goal scale of lerping")]
    private float _goalScale;
    [SerializeField, Tooltip("time to keep performance indicator at max size")]
    private float _holdingTime;
    [SerializeField, Tooltip("used to enable/disable the sprite.")]
    private SpriteRenderer _renderer;
    [SerializeField, Tooltip("Sprite list for different text options")]
    private Sprite[] _popupSprites;

    /// <summary>
    /// 0 = Perfect
    /// -1 = Early/Short
    /// 1 = Far/LAte
    /// </summary>
    public void PopUp(int value)
    {
        StartCoroutine(DoPopup(value));
    }

    private IEnumerator DoPopup(int value)
    {
        // set sprite based on performance
        if (value == 0)
            _renderer.sprite = _popupSprites[0]; // perfect
        else if (value == 999)
            _renderer.sprite = _popupSprites[5]; // Okay
        else if(_isCasting)
        {
            if (value < 0)
                _renderer.sprite = _popupSprites[1]; // Short
            else
                _renderer.sprite = _popupSprites[2]; // Far
        }
        else // reeling
        {
            if (value < 0)
                _renderer.sprite = _popupSprites[3]; // Early
            else
                _renderer.sprite = _popupSprites[4]; // Late
        }

        transform.localScale = new Vector3(0, 0, 1);
        _renderer.enabled = true;

        // get bigger
        while(Mathf.Abs(transform.localScale.x - _goalScale) > _snappingDistance)
        {
            float lerpScale = Mathf.Lerp(transform.localScale.x, _goalScale, 1f - Mathf.Exp(-_sharpness * Time.deltaTime));
            Vector3 newScale = transform.localScale;
            newScale.x = lerpScale;
            newScale.y = lerpScale;
            transform.localScale = newScale;

            yield return null;
        }
        // snap to goal
        transform.localScale = new Vector3(_goalScale, _goalScale, 1);

        yield return new WaitForSeconds(_holdingTime);

        // get smaller
        while (Mathf.Abs(transform.localScale.x - 0) > _snappingDistance)
        {
            float lerpScale = Mathf.Lerp(transform.localScale.x, 0, 1f - Mathf.Exp(-_sharpness * Time.deltaTime));
            Vector3 newScale = transform.localScale;
            newScale.x = lerpScale;
            newScale.y = lerpScale;
            transform.localScale = newScale;

            yield return null;
        }
        // snap to goal
        transform.localScale = new Vector3(0, 0, 1);
    }
}
