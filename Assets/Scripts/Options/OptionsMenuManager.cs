using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField]
    private string _startScene;


    [SerializeField] private GameObject _currentTab;
    [SerializeField] private int _currentTabNum;

    [SerializeField, Header("Tabs")]
    private List<GameObject> _tabs;

    [SerializeField, Header("Buttons")]
    private List<Button> _buttons;

    [SerializeField, Tooltip("SFX slider")]
    private Slider _sfxSlider;
    [SerializeField, Tooltip("Music slider")]
    private Slider _musicSlider;

    [SerializeField, Tooltip("Saturation slider")]
    private Slider _saturationSlider;
    [SerializeField, Tooltip("Brightness slider")]
    private Slider _brightnessSlider;

    [SerializeField, Tooltip("Profile")]
    private PostProcessProfile _profile;
    [SerializeField, Tooltip("Pass in the main camera")]
    private PostProcessLayer _layer;
    private AutoExposure _exposure;
    private ColorGrading _saturation;

   
    public List<TextMeshProUGUI> _tmpList;
    public List<Slider> _sliderList;

    private void Start()
    {
        // Correctly default set brightness and exposure
        _profile.TryGetSettings(out _exposure);
        _exposure.keyValue.value = GameManager.Instance.GamePersistent.Brightness;
        _brightnessSlider.value = GameManager.Instance.GamePersistent.Brightness;
        AdjustBrightness();

        _profile.TryGetSettings(out _saturation);
        _saturation.saturation.value = GameManager.Instance.GamePersistent.Saturation;
        _saturationSlider.value = GameManager.Instance.GamePersistent.Saturation;
        AdjustSaturation();

        // Correctly default set volume settings
        _sfxSlider.value = GameManager.Instance.GamePersistent.SFXVolume;
        _musicSlider.value = GameManager.Instance.GamePersistent.MusicVolume;

        if (_currentTab != null)
        {
            _buttons[_currentTabNum].interactable = false;
        }
    }

    public void AdjustSFX()
    {
        GameManager.Instance.GamePersistent.SFXVolume = _sfxSlider.value;
        SoundManager.Instance.SetGlobalParameter("SFXSlider", _sfxSlider.value);
    }
    public void AdjustMusic()
    {
        GameManager.Instance.GamePersistent.MusicVolume = _musicSlider.value;
        SoundManager.Instance.SetGlobalParameter("MusicSlider", _musicSlider.value);
    }
    public void AdjustSaturation()
    {
        _profile.TryGetSettings(out _saturation);
        _saturation.saturation.value = _saturationSlider.value;
        GameManager.Instance.GamePersistent.Saturation = _saturationSlider.value;
    }
    public void AdjustBrightness()
    {
        if (_brightnessSlider.value != 0)
        {
            _exposure.keyValue.value = _brightnessSlider.value;
            GameManager.Instance.GamePersistent.Brightness = _brightnessSlider.value;
        }
        else
        {
            _exposure.keyValue.value = .05f; // the lowest brightenss setting
            GameManager.Instance.GamePersistent.Brightness = .05f;
        }
        
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene(_startScene);
    }

    public void SetCurrentTab(int tabNum)
    {
        _currentTabNum = tabNum;
        _currentTab = _tabs[tabNum];
    }

    public void TurnOn()
    {
        _buttons[_currentTabNum].interactable = false;
        _currentTab.SetActive(true);
    }

    public void TurnOff()
    {
        if (_currentTab != null)
        {
            _buttons[_currentTabNum].interactable = true;
            _currentTab.SetActive(false);
        }
    }
}
