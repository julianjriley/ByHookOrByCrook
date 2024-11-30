using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public Camera ArenaCamera;

    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 cursorPos = ArenaCamera.ScreenToWorldPoint(Mouse.current.position.value);

        transform.position = new Vector3(cursorPos.x, cursorPos.y, 1);
    }

    public void UpdateSprite(Sprite sprite)
    {
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
