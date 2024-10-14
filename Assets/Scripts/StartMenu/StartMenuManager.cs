using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private string gameScene, optionsScene, creditsScene, startScene;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene(optionsScene);
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
