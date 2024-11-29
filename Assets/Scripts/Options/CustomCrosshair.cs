using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomCrosshair : MonoBehaviour
{
    public GameObject DefaultCrosshair; // this should be a reference to the global crosshair
    private Sprite _sprite;
    public Slider SizeSlider;
    public List<Sprite> crosshairList;

    private Sprite exampleSprite;
    private void Start()
    {
        _sprite = DefaultCrosshair.GetComponent<Sprite>();
        exampleSprite = GetComponent<Image>().sprite;
    }
    public void ChangeSize()
    {
        switch (SizeSlider.value)
        {
            case 0:

                _sprite = crosshairList[0];
                this.GetComponent<Image>().sprite = _sprite;
                break;
            case 1:
                _sprite = crosshairList[1];
                this.GetComponent<Image>().sprite = _sprite;
                break;
            case 2:
                _sprite = crosshairList[2];
                this.GetComponent<Image>().sprite = _sprite;
                break;
        }

    }
}
