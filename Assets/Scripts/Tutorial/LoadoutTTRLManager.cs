using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadoutTTRLManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

    [SerializeField] private Animator _signAnim;
    [SerializeField] private GameObject _fightButton;
    [SerializeField] private GameObject _practiceButton;
    [SerializeField] private GameObject _foregroundScreen;
    [SerializeField] private GameObject _caughtFishParent;

    [Header("Loadout Selector")]
    [SerializeField] private LoadoutSelection _bs;

    [Header("SFX")]
    [SerializeField] private EventReference _signSound;

    private bool _readSign = false;
    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialCombat)
        {

            _tutorialView.SetActive(true);
            GameManager.Instance.GamePersistent.IsTutorialFish = false; // Turn it off behind you as you go
            _fightButton.SetActive(false); // No going straight into battle
            StartCoroutine(DoTutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialCombat)
        {
            if (_bs.GetCurrentLoadoutSize() == GameManager.Instance.GamePersistent.BattleInventorySize || _caughtFishParent.transform.childCount == 0)
            {
                _practiceButton.SetActive(true);
            }
            else
            {
                _practiceButton.SetActive(false);
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
        SoundManager.Instance.PlayOneShot(_signSound, gameObject.transform.position);
        // When they click the button, the sign goes away and they can select bait
        yield return new WaitUntil(() => _readSign);
        _signAnim.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(7f / 12f);
        _foregroundScreen.SetActive(false);
    }
}
