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
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _areYouSure;

    void Start()
    {
        if (_continueButton != null) {
            if(GameManager.Instance.GamePersistent.SaveStarted) {  //GameManager.Instance.GamePersistent.BossNumber == 0 && GameManager.Instance.GamePersistent.Gill == 5
                _continueButton.SetActive(true);
            } else {
                _continueButton.SetActive(false);
            }
        }
    }    
    /// <summary>
    /// RESUME FUNCTIONALITY
    /// </summary>
    public void LoadGameScene()
    {
        _transitionHandler.LoadScene(gameScene);
    }

    public void NewSaveCheck() {
        if (GameManager.Instance.GamePersistent.SaveStarted) { // GameManager.Instance.GamePersistent.BossNumber != 0
            _areYouSure.SetActive(true);
        } else {
            ClearData();
        }
    }
    public void DeactivateAreYouSure() {
        _areYouSure.SetActive(false);
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
        _transitionHandler.LoadScene(creditsScene, SceneTransitionsHandler.TransitionType.SlideLeft);
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
