using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsHandler : MonoBehaviour
{
    [System.Serializable]
    public enum TransitionType
    {
        Water,
        SlideRight,
        SlideLeft,
        None
    }

    [Header("Configuration")]
    [SerializeField, Tooltip("Transition type for entering scene.")]
    private TransitionType _enterTransitionType;
    [SerializeField, Tooltip("Transition type for exiting scene.")]
    private TransitionType _exitTransitionType;

    [Header("Water Transitions")]
    [SerializeField, Tooltip("Animated sprite for water scene enter.")]
    private GameObject _enterWater;
    [SerializeField] EventReference _enterWaterSound;
    [SerializeField, Tooltip("Animated sprite for water scene exit.")]
    private GameObject _exitWater;
    [SerializeField] EventReference _exitWaterSound;
    [Header("Slide Right Transitions")]
    [SerializeField, Tooltip("Animated sprite for slide right scene enter.")]
    private GameObject _enterSlideRight;
    [SerializeField, Tooltip("Animated sprite for slide right scene exit.")]
    private GameObject _exitSlideRight;

    [Header("Slide Right Transitions")]
    [SerializeField, Tooltip("Animated sprite for slide left scene enter.")]
    private GameObject _enterSlideLeft;
    [SerializeField, Tooltip("Animated sprite for slide left scene exit.")]
    private GameObject _exitSlideLeft;
    [SerializeField] EventReference _slideTransitionSound;

    // whether the enter transition has completed
    private bool _isDoneLoading = false;
    public bool IsDoneLoading() { return _isDoneLoading; }

    // whether it is time to call the scene transition
    private bool _isReadyToLoad = false;
    private bool IsReadyToLoad() { return _isReadyToLoad; }

    // Start is called before the first frame update
    void Start()
    {
        // activate enter transition instantly
        switch(_enterTransitionType)
        {
            case TransitionType.Water:
                _enterWater.SetActive(true);
                SoundManager.Instance.PlayOneShot(_enterWaterSound, gameObject.transform.position);
                break;
            case TransitionType.SlideRight:
                _enterSlideRight.SetActive(true);
                break;
            case TransitionType.SlideLeft:
                _enterSlideLeft.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Called wherever a scene transition would normally be called.
    /// </summary>
    public void LoadScene(string nextSceneName, TransitionType transitionType = TransitionType.None)
    {
        // determine which type to use
        TransitionType type = transitionType;
        if (type is TransitionType.None)
            type = _exitTransitionType;

        // activate enter transition instantly
        switch (type)
        {
            case TransitionType.Water:
                _exitWater.SetActive(true);
                SoundManager.Instance.PlayOneShot(_exitWaterSound, gameObject.transform.position);
                break;
            case TransitionType.SlideRight:
                _exitSlideRight.SetActive(true);
                SoundManager.Instance.PlayOneShot(_slideTransitionSound, gameObject.transform.position);
                break;
            case TransitionType.SlideLeft:
                _exitSlideLeft.SetActive(true);
                SoundManager.Instance.PlayOneShot(_slideTransitionSound, gameObject.transform.position);
                break;
        }

        // invoke load
        StartCoroutine(LoadWhenReady(nextSceneName));
    }

    /// <summary>
    /// Loads scene once boolean has been updated indicating scene transition is complete.
    /// </summary>
    private IEnumerator LoadWhenReady(string nextSceneName)
    {
        yield return new WaitUntil(IsReadyToLoad);

        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// Updates boolean so transition handler knows it is time to load next scene
    /// </summary>
    public void SetReadyToLoad()
    {
        _isReadyToLoad = true;
    }

    /// <summary>
    /// Updates boolean so transition handler knows loading has completed.
    /// </summary>
    public void SetDoneLoading()
    {
        _isDoneLoading = true;
    }
}
