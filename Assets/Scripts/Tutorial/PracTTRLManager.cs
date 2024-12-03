using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracTTRLManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

    [SerializeField] private GameObject _fightButton;

    [Header("Boss Scenes")]
    [SerializeField] string[] _bossScenes;

    [Header("Outro Transition")]
    [SerializeField] private GameObject _firstHalfWaterTransition;
    private bool _isPressed = false;

    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialCombat)
        {
            _tutorialView.SetActive(true);
            _fightButton.SetActive(false);
        }
        else
        {
            _tutorialView.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    public void SceneChange()
    {
        // Use this function to transition to PRACTICE or COMBAT scene
        string sceneToSwitchTo;
        switch (GameManager.Instance.GamePersistent.BossNumber)
        {
            case 0:
                sceneToSwitchTo = _bossScenes[0]; break;
            case 1:
                sceneToSwitchTo = _bossScenes[1]; break;
            case 2:
                sceneToSwitchTo = _bossScenes[2]; break;
            default:
                sceneToSwitchTo = _bossScenes[0]; break;
        }
        SceneManager.LoadScene(sceneToSwitchTo);
    }
    public void FightButton() {
        if (_isPressed) {
            return;
        }
        _isPressed = true;
        StartCoroutine(SceneDelay());
    }
    IEnumerator SceneDelay() {
        _firstHalfWaterTransition.SetActive(true);
        yield return new WaitForSeconds(1.433f);
        SceneChange();
    }
}
