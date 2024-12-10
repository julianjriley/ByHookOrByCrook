using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        actions.Enable();
        _isPaused = false;
        Time.timeScale = 1f;
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
        AreYouSure();
        // Make sure to call LoadScene function by 0 ON BUTTON
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void Hub()
    {
        AreYouSure();
        // Make sure to call LoadScene function by 3 ON BUTTON
    }

    public void AreYouSure()
    {
        // Make content appear
        _areYouSureMenu.SetActive(true);
    }
    public void IMSUREGOD()
    {
        _areYouSureMenu.SetActive(false);
        Time.timeScale = 1f;
        actions.Enable();
        GameManager.Instance.ResetScenePersistentData();
    }
    public void IMNOTSURE()
    {
        _areYouSureMenu.SetActive(false);
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
