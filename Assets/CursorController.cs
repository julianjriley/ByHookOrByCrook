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
        Cursor.visible = false;

        // Bear cursor in main menu, cutscene, options, credits, hub, bait, fishing, loadout, cashout, ending
        if (!(SceneManager.GetActiveScene().buildIndex >= 0 && SceneManager.GetActiveScene().buildIndex <= 7
            || SceneManager.GetActiveScene().buildIndex > 12 && SceneManager.GetActiveScene().buildIndex <= 14))
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
    private void Start()
    {
        Cursor.visible = false;
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
    }

    public void UpdateSprite(Sprite sprite)
    {
        this.GetComponent<Image>().sprite = sprite;
    }
}
