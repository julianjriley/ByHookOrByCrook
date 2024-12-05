using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField] private List<Animator> _animList;
    [SerializeField] SceneTransitionsHandler _transitionsHandler;
    public string SceneName;
    protected InputAction _interactAction;
    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.Enable();
        StartCoroutine(DoIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoIntro()
    {
        _animList[0].Play("Page"); // Turn page 0
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => _interactAction.IsPressed());

        _animList[1].Play("Page"); // Turn page 1
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !_interactAction.IsPressed());
        yield return new WaitUntil(() => _interactAction.IsPressed());

        _animList[2].Play("Page"); // Turn page 2
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !_interactAction.IsPressed());
        yield return new WaitUntil(() => _interactAction.IsPressed());

        _animList[3].Play("Page"); // Turn page 2
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !_interactAction.IsPressed());
        yield return new WaitUntil(() => _interactAction.IsPressed());

        _animList[4].Play("Page"); // Turn page 2
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => !_interactAction.IsPressed());
        yield return new WaitUntil(() => _interactAction.IsPressed());

        // Then we change scenes to Hub
        _transitionsHandler.LoadScene(SceneName);
        yield return null;
    }
}
