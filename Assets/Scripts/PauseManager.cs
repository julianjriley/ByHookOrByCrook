using System.Collections;
using System.Collections.Generic;
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

    // --------README--------
    // PauseMenuController MUST appear in all scenes for the sake of resetting the time scale.
    // Main Menu OPTION button needs an updated OnClick() Function that passes in the prefab instead of loading a scene.
    // The solution right now is scuffed, but should be enough for the beta.
    // You will have to update the first IF statement conditions if you're adding MORE build scenes.



    private void Start()
    {
        _isPaused = false;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1
            || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4
            || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6
            | SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 8
            || SceneManager.GetActiveScene().buildIndex == 9 || SceneManager.GetActiveScene().buildIndex == 10)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale = 1f;
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0
            || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
            {
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
        _isPaused = true;
        _pauseMenu.SetActive(true);
       
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        StartCoroutine(Countdown());

    }

    IEnumerator Countdown()
    {
        // TODO: doesn't give us the 3-2-1 countdown we need before continuing the game

        Time.timeScale = 1f;
        _isPaused = false;
        yield return new WaitForSeconds(3);

    }

    public void Options()
    {
        _optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Hub()
    {
        SceneManager.LoadScene(3);
    }
}
