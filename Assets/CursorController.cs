using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public Camera ArenaCamera;
    [SerializeField, Tooltip("Bear cursor list")]
    private List<Sprite> _cursorList;
    [SerializeField, Tooltip("Multiplier to scale up the bear cursor.")]
    private float _bearHandScaleMult;
    [SerializeField, Tooltip("Used to apply offset for bear hand cursor.")]
    private RectTransform _rect;

    private bool _isCrosshair = false; // whether the crosshair is being used in this scene

    private void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        // Cross hair only in practice, boss 1, boss 2, boss 3
        if (sceneName.Equals("6.5PracTut") || sceneName.Equals("7-1_BETA_Boss1") || sceneName.Equals("7-2_BETA_Boss2") || sceneName.Equals("7-3_BETA_Boss3"))
        {
            this.GetComponent<Image>().sprite = _cursorList[2];
            _isCrosshair = true;
        }
        else // bear hand
        {
            // make it bigger to account for the fact that the cursor is slid over to the side to make the click part on the finger not the middle
            _rect.localScale *= _bearHandScaleMult;
        }
    }

    private void Update()
    {
        // center sprite directly on cursor
        Vector2 cursorPos = Mouse.current.position.value;

        _rect.position = new Vector3(cursorPos.x, cursorPos.y, 1);

        if (!_isCrosshair)
        {
            // account for click vs non-click sprite
            if (Mouse.current.leftButton.isPressed)
            {
                this.GetComponent<Image>().sprite = _cursorList[1];
            }
            else
            {
                this.GetComponent<Image>().sprite = _cursorList[0];
            }
        }

        // enable default cursor if out of bounds of game window, otherwise keep it hidden
        if (cursorPos.x < 0 || cursorPos.x > Screen.width || cursorPos.y < 0 || cursorPos.y > Screen.height)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }

    public void UpdateSprite(Sprite sprite)
    {
        this.GetComponent<Image>().sprite = sprite;
    }
}
