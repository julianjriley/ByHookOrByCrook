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
    [SerializeField]
    private Sprite bubble, pop;
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
        StartCoroutine(BubblePop());
    }


    IEnumerator BubblePop() { 
        yield return new WaitForSeconds(1);
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
