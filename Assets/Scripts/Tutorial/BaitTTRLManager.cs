using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class BaitTTRLManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

    [SerializeField] private Animator _signAnim;
    [SerializeField] private GameObject _backToHubButton;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _foregroundScreen;

    [Header("Bait Selector")]
    [SerializeField] private BaitSelector _bs;

    private bool _readSign = false;
    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialBait)
        {

            _tutorialView.SetActive(true);
            GameManager.Instance.GamePersistent.IsTutorialHub = false; // Turn it off behind you as you go
            _backToHubButton.SetActive(false); // No going back to the hub during the tutorial
            StartCoroutine(DoTutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialBait)
        {
            if (_bs.GetRemainingBaitSlots() == 0)
            {
                _nextButton.SetActive(true);
            }
            else
            {
                _nextButton.SetActive(false);
            }
        }

           
    }

    public void ReadTheSign()
    {
        _readSign = true;
    }

    private IEnumerator DoTutorial()
    {
        // Show the sign
        _signAnim.Play("Appear", 0, 0);
        // When they click the button, the sign goes away and they can select bait
        yield return new WaitUntil(() => _readSign);
        _signAnim.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(7f / 12f);
        _foregroundScreen.SetActive(false);
        


    }
}
