using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingCutsceneManager : MonoBehaviour
{
    [SerializeField] private List<Animator> _animList;
    [SerializeField] SceneTransitionsHandler _transitionsHandler;
    public string SceneName;
    protected InputAction _interactAction;
    [SerializeField] EventReference pageTurnSound;

    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.Enable();
        StartCoroutine(DoEnding());
        SoundManager.Instance.musicEventInstance.setParameterByName("HubPosition", 0, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoEnding()
    {
        yield return new WaitUntil(() => _interactAction.IsPressed());
        for (int i = 0; i < _animList.Count - 1; i++)
        {
            Debug.Log(i);
            _animList[i].Play("Page");
            SoundManager.Instance.PlayOneShot(pageTurnSound, gameObject.transform.position); // Turn page
            if(i == 1)
            {
                SoundManager.Instance.musicEventInstance.setParameterByName("HubPosition", 1);
            }
            if (i == 8)
            {
                SoundManager.Instance.musicEventInstance.setParameterByName("HubPosition", 0);
            }
            yield return new WaitForSeconds(1.5f);
            yield return new WaitUntil(() => !_interactAction.IsPressed());
            yield return new WaitUntil(() => _interactAction.IsPressed());
        }

        _transitionsHandler.LoadScene(SceneName);

        // And then load the credits
    }
}
