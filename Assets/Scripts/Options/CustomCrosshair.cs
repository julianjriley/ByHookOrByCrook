using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomCrosshair : MonoBehaviour
{

    // private SpriteRenderer _renderer;
    public Image DefaultCrosshair;

    public Slider Red, Green, Blue, Opacity, Size;
    public TextMeshProUGUI RPercent, GPercent, BPercent, OPercent;

    private void Start()
    {
        //Cursor.visible = false;
        DefaultCrosshair = GetComponent<Image>();
       // _renderer = GetComponent<SpriteRenderer>();
        

        //Cursor.SetCursor(DefaultCrosshair.texture,new Vector2(0,0), CursorMode.ForceSoftware);
    }

    private void Update()
    {
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameManager.Instance.GamePersistent.Crosshair = this;
        GameManager.Instance.GamePersistent.crosshairSprite = DefaultCrosshair.sprite;
    }

    public void ChangeColor()
    {
        DefaultCrosshair.color = new Color(Red.value, Green.value, Blue.value, Opacity.value);

        // Change percents

        RPercent.text = ((int)Red.value * 100).ToString() + " %";
        GPercent.text = ((int)Green.value * 100).ToString() + " %";
        BPercent.text = ((int)Blue.value * 100).ToString() + " %";
        OPercent.text = ((int)Opacity.value * 100).ToString() + " %";


    }
    public void ChangeSize()
    {
        float size = Size.value;
        DefaultCrosshair.gameObject.GetComponent<RectTransform>().localScale = new Vector2(size, size);
    }
}
