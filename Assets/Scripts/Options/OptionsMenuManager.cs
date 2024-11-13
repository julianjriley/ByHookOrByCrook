using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class OptionsMenuManager : MonoBehaviour
{
    //[SerializeField]
    //private List<TextMeshProUGUI> _listOfPercentages;

    //private TextMeshProUGUI _textTMP;

    [SerializeField]
    private string _startScene;

    [SerializeField]
    private GameObject _currentTab;

    [SerializeField]
    private GameObject Controls, Gameplay, Audio, Graphics;

    [SerializeField, Tooltip("Saturation slider")]
    private Slider _saturationSlider;
    [SerializeField, Tooltip("Brightness slider")]
    private Slider _brightnessSlider;
    [SerializeField, Tooltip("Brightness profile")]
    private PostProcessProfile _profile;
    [SerializeField, Tooltip("Pass in the main camera")]
    private PostProcessLayer _layer;
    private AutoExposure _exposure;
    private ColorGrading _saturation;

   
    public List<TextMeshProUGUI> _tmpList;

    public List<Slider> _sliderList;

    private void Start()
    {
        _profile.TryGetSettings(out _exposure);
        AdjustBrightness();

        _profile.TryGetSettings(out _saturation);
        AdjustSaturation();
    }

    public void AdjustSaturation()
    {
        if (_saturationSlider.value != 0)
        {
            _saturation.saturation.value = _saturationSlider.value;
        }
        else
        {
            _saturation.saturation.value = .05f; // lowest saturation setting
        }
    }
    public void AdjustBrightness()
    {
        if (_brightnessSlider.value != 0)
        {
            _exposure.keyValue.value = _brightnessSlider.value;
        }
        else
        {
            _exposure.keyValue.value = .05f; // the lowest brightenss setting
        } 
    }
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
