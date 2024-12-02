using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    Image _image;
    [SerializeField] float fadeInDuration = 1;
    [SerializeField, Tooltip("True only for the filler bar.")] private bool _isTransparentBar = false;
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<Image>(out Image image))
        {
            _image = image;
            StartCoroutine(FadeIn());
        }

    }

    
    IEnumerator FadeIn()
    {
        Color originalColor = _image.color;
        originalColor.a = 0f;
        _image.color = originalColor;
        for(int i = 0; i < 20 * fadeInDuration; i++)
        {
            originalColor.a += (_isTransparentBar ? 0.0375f : 0.05f)/fadeInDuration;
            _image.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
