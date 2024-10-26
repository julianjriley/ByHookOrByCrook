using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField]
    private string _startScene;

    [SerializeField]
    private GameObject _currentTab;

    [SerializeField]
    private GameObject Controls, Gameplay, Audio, Graphics;
    public void LoadStartScene()
    {
        SceneManager.LoadScene(_startScene);
    }

    public void SetCurrentTab(GameObject Tab)
    {
        _currentTab = Tab;
    }

    public void TurnOn()
    {   
        _currentTab.transform.parent.GetComponent<Button>().interactable = false;
        _currentTab.SetActive(true);
    }

    public void TurnOff()
    {
        if (_currentTab != null)
        {
            _currentTab.transform.parent.GetComponent<Button>().interactable = true;
            _currentTab.SetActive(false);
        }
    }
}
