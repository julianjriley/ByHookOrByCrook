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

    // --------README--------
    // The solution right now is scuffed, but should be enough for the beta.
    // You will have to update the first IF statement conditions if you're adding MORE build scenes.

    private void Awake()
    {
        actions = InputSystem.actions;
       
    }

    private void Start()
    {
        _isPaused = false;
        _player = FindAnyObjectByType<PlayerCombat>();
    }
    private void Update()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1
        //    || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4
        //    || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6
        //    | SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 8
        //    || SceneManager.GetActiveScene().buildIndex == 9 || SceneManager.GetActiveScene().buildIndex == 10)
        //{
        //    if (Time.timeScale < 1f)
        //    {
        //        Time.timeScale = 1f;
        //    }
        //}

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0
            || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
            {
                // Prevents pausing in main menu, credits and something else
                return;
            }

            if (_isPaused == false)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        actions.Disable();
        _isPaused = true;
        _pauseMenu.SetActive(true);
        Debug.Log("disabling input");

    }


    public void Resume()
    {
        _pauseMenu.SetActive(false);
        actions.Enable();


        Countdown(); // remove later, just testing for now

        // uncomment if statement below after testing and getting all build indicies

        //if (SceneManager.GetActiveScene().buildIndex > 9 && SceneManager.GetActiveScene().buildIndex < 12)
        //{
        //    // In combat scenes we countdown 
        //    Countdown();
        //}

    }

    void Countdown()
    {
        _startTime = Time.unscaledTime;
        Debug.Log("Start Time = " + _startTime);
        _endTime = _startTime + 3f;
        Debug.Log("Emd Time = " + _endTime);

        Debug.Log("Unscaled time is = " + Time.unscaledTime);

        if (Time.unscaledTime >= _endTime)
        {
            Debug.Log("Unscaled time == endtime");
            Time.timeScale = 1f;
            _isPaused = false;
        }

    }

    public void Options()
    {
        _optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Hub()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }
}
