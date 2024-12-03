using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private string gameScene, creditsScene, startScene;
    [SerializeField]
    GameObject optionsScreen;
    //[SerializeField]
    //private GameObject optionsScene;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ClearData()
    {
        GameManager.Instance.InitializeSaveData(true);
        SceneManager.LoadScene(gameScene);
    }
    public void LoadOptionsScene()
    {
        optionsScreen.SetActive(true);
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
