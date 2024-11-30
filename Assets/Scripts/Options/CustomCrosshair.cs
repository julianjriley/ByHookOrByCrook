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
    public Slider SizeSlider;
    public List<Sprite> crosshairList;
    [SerializeField]
    private CursorController _controller;

    public void ChangeSize()
    {
        switch (SizeSlider.value)
        {
            case 0:
                _controller.UpdateSprite(crosshairList[0]); 
                //_sprite = crosshairList[0];
                this.GetComponent<Image>().sprite = crosshairList[0];
                break;
            case 1:
                _controller.UpdateSprite(crosshairList[1]);
                this.GetComponent<Image>().sprite = crosshairList[1];
                break;
            case 2:
                _controller.UpdateSprite(crosshairList[2]);
                this.GetComponent<Image>().sprite = crosshairList[2];
                break;
        }

    }
}
