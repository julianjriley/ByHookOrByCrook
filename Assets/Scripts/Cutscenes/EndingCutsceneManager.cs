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

    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.Enable();
        StartCoroutine(DoEnding());
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
            _animList[i].Play("Page"); // Turn page
            yield return new WaitForSeconds(1.5f);
            yield return new WaitUntil(() => !_interactAction.IsPressed());
            yield return new WaitUntil(() => _interactAction.IsPressed());
        }

        _transitionsHandler.LoadScene(SceneName);

        // And then load the credits
    }
}
