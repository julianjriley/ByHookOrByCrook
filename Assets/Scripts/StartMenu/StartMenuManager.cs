using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private string gameScene, creditsScene, startScene, newStartScene;
    [SerializeField]
    GameObject optionsScreen;
    [SerializeField]
    private Sprite bubble, pop;
    [SerializeField] private Image _optionsImage;
    [SerializeField] private SceneTransitionsHandler _transitionHandler;
    public void LoadGameScene()
    {
        if (GameManager.Instance.GamePersistent.LossCounter != 0 || GameManager.Instance.GamePersistent.BossNumber != 0)
        {
            SceneManager.LoadScene(gameScene);
        }
    }

    public void ClearData()
    {
        GameManager.Instance.InitializeSaveData(true);
        _transitionHandler.LoadScene(newStartScene);
    }
    public void LoadOptionsScene()
    {
        StartCoroutine(BubblePop());
        
    }


    IEnumerator BubblePop() {
        _optionsImage.sprite = pop;
        yield return new WaitForSeconds(.1f);
        optionsScreen.SetActive(true);
        yield return new WaitForSeconds(.01f);
        _optionsImage.sprite = bubble;

    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void LoadStartScene()
    {
        _transitionHandler.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
