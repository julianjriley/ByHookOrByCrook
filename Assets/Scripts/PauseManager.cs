using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// WISHLIST: Countdown Timer. Some functions are written but don't work.

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    private bool _isPaused;

    [SerializeField]
    private GameObject _optionsMenu;

    [SerializeField]
    private GameObject _areYouSureMenu;

    private InputActionAsset actions;
    private ActionControls _controls;
    private PlayerCombat _player;

    private float _startTime;
    private float _endTime;

    private bool _isSure;

    [Header("Scene Transitions")]
    [SerializeField, Tooltip("Name of hub scene.")]
    private string _hubScene;
    [SerializeField, Tooltip("Name of main menu scene.")]
    private string _mainMenuScene;

    private bool _isToLoadHub = true;

    private void Awake()
    {
        actions = InputSystem.actions;  
    }

    private void Start()
    {
        _isSure = false;
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
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        actions.Enable();
        _isPaused = false;
        Time.timeScale = 1f;
    }
    public void Options()
    {
        _optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        _isToLoadHub = false;

        AreYouSure();
    }
    public void Hub()
    {
        _isToLoadHub = true;

        AreYouSure();
    }

    public void AreYouSure()
    {
        _areYouSureMenu.SetActive(true);
    }
    public void IMSUREGOD()
    {
        // This goes on YES button on the Are You Sure? menu
        _isSure = true;
        Time.timeScale = 1f;
        actions.Enable();
        GameManager.Instance.ResetScenePersistentData();

        // actually load the scene
        if (_isToLoadHub)
            SceneManager.LoadScene(_hubScene);
        else
            SceneManager.LoadScene(_mainMenuScene);
    }
    public void IMNOTSURE()
    {
        // This goes on NO button on the Are You Sure? menu
        _areYouSureMenu.SetActive(false);
        _isSure = false;
    }

    /*private IEnumerator DoCountdown()
    {
        _startTime = Time.unscaledTime;
        _endTime = _startTime + 3f;
 
        yield return new WaitUntil(() => Time.unscaledTime >= _endTime);

            Time.timeScale = 1f;
            _isPaused = false;
    }

    void Countdown()
    {
        StartCoroutine(DoCountdown());
    }*/
}
