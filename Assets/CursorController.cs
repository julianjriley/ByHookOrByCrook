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

    private void Awake()
    {
        Cursor.visible = false;
        if (!(SceneManager.GetActiveScene().buildIndex >= 0 && SceneManager.GetActiveScene().buildIndex <= 7
            || SceneManager.GetActiveScene().buildIndex > 12 && SceneManager.GetActiveScene().buildIndex <= 14))
        {
            this.GetComponent<Image>().sprite = _cursorList[2];
        }
    }
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 cursorPos = Mouse.current.position.value;//ArenaCamera.ScreenToWorldPoint(Mouse.current.position.value);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, 1);

        if (SceneManager.GetActiveScene().buildIndex >= 0 && SceneManager.GetActiveScene().buildIndex <= 7
            || SceneManager.GetActiveScene().buildIndex > 12 && SceneManager.GetActiveScene().buildIndex <= 14)
        {
            // Bear cursor in main menu, cutscene, options, credits, hub, bait, fishing, loadout, cashout, ending

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
