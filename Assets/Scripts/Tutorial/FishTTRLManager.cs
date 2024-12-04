using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTTRLManager : MonoBehaviour
{
    [Header("Fishing")]
    [SerializeField] private FishingControls _fControls;

    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;
    [SerializeField] private GameObject _void;
    [SerializeField] private Animator _sign1;
    [SerializeField] private Animator _sign2;
    [SerializeField] private EventReference _signSound;

    private bool _hasCast = false;
    private bool _hasConfirmed = false;
    private bool _hasConfirmed2 = false;

    private void OnEnable()
    {
        FishingControls.onFirstBobberLand += HasCast;
    }

    private void OnDisable()
    {
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

    private IEnumerator DoTutorial()
    {
        _sign1.Play("Appear", 0, 0);    // Show how to hold and release, and then let them
        SoundManager.Instance.PlayOneShot(_signSound, gameObject.transform.position);
        yield return new WaitUntil(() => _hasConfirmed);
        _sign1.Play("Disappear", 0, 0);
        
        yield return new WaitForSeconds(.5f);
        _void.SetActive(false);
        _fControls.SetTutorialCasting();
        yield return new WaitUntil(() => _hasCast);
        yield return new WaitForSeconds(1f);
        _void.SetActive(true);
        _sign2.Play("Appear", 0, 0);    // Third non-fullscreen popup: When it bites, do the thing!
        SoundManager.Instance.PlayOneShot(_signSound, gameObject.transform.position);

        // FREEZE THE BITE LOGIC
        // To continue, they click a button on that popup, which unfreezes the bite logic

        yield return new WaitUntil(() => _hasConfirmed2);
        _void.SetActive(false);
        _sign2.Play("Disappear", 0, 0);
        _fControls.SetTutorialReeling();
        // Once they reel it in, a fourth popup appears with a summary and telling them to keep going

    }

    private void HasCast()
    {
        _hasCast = true;
    }

    public void Confirmation()
    {
        _hasConfirmed = true;
    }
    public void Confirmation2()
    {
        _hasConfirmed2 = true;
    }

}
