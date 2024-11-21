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
    Cursor cursor;
    // private SpriteRenderer _renderer;
    public Image DefaultCrosshair;
    public Texture2D Crosshair;

    public Slider Red, Green, Blue, Opacity, Size;

    private Color newColor;

    private void Start()
    {
        //Cursor.visible = false;
        //DefaultCrosshair = GetComponent<Image>();
         // _renderer = GetComponent<SpriteRenderer>();
        

        //Cursor.SetCursor(DefaultCrosshair.texture,new Vector2(0,0), CursorMode.ForceSoftware);
    }

    private void OnGUI()
    {
        GUI.skin.settings.cursorColor = newColor;
    }

    private void Update()
    {
        
        //Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //Cursor.SetCursor(Crosshair, new Vector2(30, 25), CursorMode.ForceSoftware);
        //GameManager.Instance.GamePersistent.Crosshair = this;
        //GameManager.Instance.GamePersistent.crosshairSprite = DefaultCrosshair.sprite;
    }

    public void UpdateCrosshair()
    {
        DefaultCrosshair.color = new Color(Red.value, Green.value, Blue.value, Opacity.value);
        newColor = new Color(Red.value, Green.value, Blue.value, Opacity.value);
        // Texture2D texture = new Texture2D((int)Red.value, (int)Green.value, (int)Blue.value, (int)Opacity.value);

        // TODO: Link to texture2D
        // for loop through all pixels... 
        // https://docs.unity3d.com/ScriptReference/Texture2D.SetPixel.html


    }
    public void ChangeSize()
    {
        float size = Size.value;
        DefaultCrosshair.gameObject.GetComponent<RectTransform>().localScale = new Vector2(size, size);
    }
}
