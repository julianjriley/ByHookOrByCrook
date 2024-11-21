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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FightButton()
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
}
