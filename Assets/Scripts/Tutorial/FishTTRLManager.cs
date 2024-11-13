using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTTRLManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

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
        // First non-fullscreen popup: Hold the mouse down
        // Second non-fullscreen popup: Let the mouse go
        // FREEZE THE BITE LOGIC
        // Third non-fullscreen popup: When it bites, do the thing!
        // To continue, they click a button on that popup, which unfreezes the bite logic
        // Once they reel it in, a fourth popup appears with a summary and telling them to keep going
        yield return null;

    }
}
