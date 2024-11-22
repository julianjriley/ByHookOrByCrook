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
    //public Texture2D Crosshair;

    public Slider Red, Green, Blue, Opacity, Size;

    private Color newColor;

    public Texture2D Crosshair;


    private void Start()
    {

        //Crosshair = new Texture2D(82, 82, TextureFormat.RGBA32, false);
        Cursor.SetCursor(Crosshair, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        transform.position = Mouse.current.position.value;
    }

    void FillCrosshair()
    {
        for (int x = 0; x < Crosshair.width; x++)
        {
            
            for (int y = 0; y < Crosshair.height; y++)
            {
                if (Crosshair.GetPixel(x,y) != Color.clear)
                    Crosshair.SetPixel(x,y, newColor);
            }
        }
        Crosshair.Apply();

        Cursor.SetCursor(Crosshair, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void UpdateCrosshair()
    {
        DefaultCrosshair.color = new Color(Red.value, Green.value, Blue.value, Opacity.value);
        newColor = new Color(Red.value, Green.value, Blue.value, Opacity.value);

        newColor = Color.HSVToRGB(Red.value, 1, 1);
        //newColor = Color.HSVToRGB(Green.value, 1, 1);
        //newColor = Color.HSVToRGB(Blue.value, 1, 1);
        //newColor = Color.HSVToRGB(Opacity.value, 1, 1);
        FillCrosshair();
        
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
