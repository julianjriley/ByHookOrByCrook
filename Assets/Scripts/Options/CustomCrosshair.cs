using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// THIS SCRIPT IS DEPRECATED, DO NOT USE IT
/// </summary>
public class CustomCrosshair : MonoBehaviour
{
    public Slider SizeSlider;
    public List<Sprite> crosshairList;
    [SerializeField]
    private CursorController _controller;

    private void Awake()
    {

        Cursor.visible = false;

        this.GetComponent<Image>().sprite = crosshairList[0];

    }

    public void ChangeSize()
    {
        // Only update the size in combat scenes
        if (!(SceneManager.GetActiveScene().buildIndex >= 0 && SceneManager.GetActiveScene().buildIndex <= 7
            || SceneManager.GetActiveScene().buildIndex > 12 && SceneManager.GetActiveScene().buildIndex <= 14))
        {
            switch (SizeSlider.value)
            {
                case 0:
                    _controller.UpdateSprite(crosshairList[0]);
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
}
