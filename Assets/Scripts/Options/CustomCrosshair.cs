using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomCrosshair : MonoBehaviour
{

    private SpriteRenderer _renderer;
    public Sprite DefaultCrosshair;

    public Slider Red, Green, Blue, Size;

    private void Start()
    {
        Cursor.visible = false;
        _renderer = GetComponent<SpriteRenderer>();
        Cursor.SetCursor(DefaultCrosshair.texture,new Vector2(0,0), CursorMode.ForceSoftware);
    }

    private void Update()
    {
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.GamePersistent.Crosshair = this;
        GameManager.Instance.GamePersistent.crosshairSprite = DefaultCrosshair;
    }

    public void ChangeColor()
    {
        _renderer.color = new Color(Red.value, Green.value, Blue.value);
    }
    public void ChangeSize()
    {
        //DefaultCrosshair.textureRect.size = 
    }
}
