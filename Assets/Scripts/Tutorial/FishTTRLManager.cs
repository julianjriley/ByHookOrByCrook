using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTTRLManager : MonoBehaviour
{
    [Header("Fishing")]
    [SerializeField] private FishingControls _fControls;

    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;
    [SerializeField] private Animator _sign1;
    [SerializeField] private Animator _sign2;
    [SerializeField] private Animator _sign3;
    [SerializeField] private Animator _sign4;

    private bool _hasHeldDownCast = false;
    private bool _hasCast = false;
    private bool _hasConfirmed = false;

    private void OnEnable()
    {
        FishingControls.onFirstCast += HasPrepped;
        FishingControls.onFirstBobberLand += HasCast;
    }

    private void OnDisable()
    {
        FishingControls.onFirstCast -= HasPrepped;
        FishingControls.onFirstBobberLand -= HasCast;
    }

    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialFish)
        {
            _tutorialView.SetActive(true);
            GameManager.Instance.GamePersistent.IsTutorialBait = false; // Turn it off behind you as you go
            StartCoroutine(DoTutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoTutorial()
    {
        _sign1.Play("Appear", 0, 0);    // First non-fullscreen popup: Hold the mouse down

        yield return new WaitUntil(()=>_hasHeldDownCast);
        _sign2.Play("Appear", 0, 0);    // Second non-fullscreen popup: Let the mouse go

        yield return new WaitUntil(() => _hasCast);
        _sign3.Play("Appear", 0, 0);    // Third non-fullscreen popup: When it bites, do the thing!

        // FREEZE THE BITE LOGIC
        // To continue, they click a button on that popup, which unfreezes the bite logic

        yield return new WaitUntil(() => _hasConfirmed);
        _sign1.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(.05f);
        _sign2.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(.05f);
        _sign3.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(2f);
        _fControls.SetTutorialReeling();
        // Once they reel it in, a fourth popup appears with a summary and telling them to keep going
        yield return new WaitUntil(() => !_fControls.IsFishingLoopDone);
        yield return new WaitForSeconds(6f);
        _sign4.Play("Appear", 0, 0);

        yield return null;

    }

    private void HasPrepped() // For events that trigger in FishingControls
    {
        _hasHeldDownCast = true;
    }

    private void HasCast()
    {
        _hasCast = true;
    }

    public void Confirmation()
    {
        _hasConfirmed = true;
    }
}
