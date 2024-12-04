using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    private bool _isPaused;

    [SerializeField]
    private GameObject _optionsMenu;

    private InputActionAsset actions;
    private ActionControls _controls;
    private PlayerCombat _player;

    private float _startTime;
    private float _endTime;

    private void Awake()
    {
        actions = InputSystem.actions;
       
    }

    private void Start()
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);
        _player = FindAnyObjectByType<PlayerCombat>();
    }
    private void Update()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                // Prevents pausing in main menu
                return;
            }

            if (_isPaused == false)
            {
                //Debug.Log("Pause");
                Pause();

            }
            else
            {
                //Debug.Log("Resume");
                Resume();

            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        actions.Disable();
        _isPaused = true;
        _pauseMenu.SetActive(true);
        //Debug.Log("disabling input");

    }


    public void Resume()
    {
        _pauseMenu.SetActive(false);
        actions.Enable();
        _isPaused = false;
        Time.timeScale = 1f;


        // WISHLIST: Countdown Timer. Some functions are written but don't work.

    }

    void Countdown()
    {
        StartCoroutine(DoCountdown());

    }

    public void Options()
    {
        _optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        actions.Enable();
        _pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void Hub()
    {
        Time.timeScale = 1f;
        actions.Enable();
        SceneManager.LoadScene(3);
    }

    private IEnumerator DoCountdown()
    {
        _startTime = Time.unscaledTime;
        _endTime = _startTime + 3f;
 
        yield return new WaitUntil(() => Time.unscaledTime >= _endTime);

            Time.timeScale = 1f;
            _isPaused = false;
    }
}
