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
    private string _filePath;

    void Start()
    {
        _filePath = Application.persistentDataPath + "/GameData.json";
        if(GameManager.Instance.GamePersistent.BossNumber == 0 && GameManager.Instance.GamePersistent.Gill == 5) {  //System.IO.File.Exists(_filePath)
            _continueButton.SetActive(false);
        } else {
            _continueButton.SetActive(true);
        }
    }    
    /// <summary>
    /// RESUME FUNCTIONALITY
    /// </summary>
    public void LoadGameScene()
    {
        // TODO: make this actually work properly as a check for if you have save data or not
        // (loss counter is probably not a good check since you technically don't need to die)
        //if (GameManager.Instance.GamePersistent.BossNumber != 0) //GameManager.Instance.GamePersistent.LossCounter != 0 || 
        //{
            _transitionHandler.LoadScene(gameScene);
        //} else {
            //Debug.Log("Chicken butt");
        //}
        
        // TODO: we need some logic to hide the resume button altogether when there is no data to resume.
        // in that case it should only be new available to press
    }

    public void NewSaveCheck() {
        if (GameManager.Instance.GamePersistent.BossNumber != 0) { //System.IO.File.Exists(_filePath)
            _areYouSure.SetActive(true);
        } else {
            ClearData();
        }
    }
    public void NoNewSave() {
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
