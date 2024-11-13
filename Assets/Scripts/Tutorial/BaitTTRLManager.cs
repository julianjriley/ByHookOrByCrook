using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class BaitTTRLManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

    [SerializeField] private Animator _signAnim;
    [SerializeField] private GameObject _backToHubButton;


    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialBait)
        {
            _tutorialView.SetActive(true);
            GameManager.Instance.GamePersistent.IsTutorialHub = false; // Turn it off behind you as you go
            _backToHubButton.SetActive(false); // No going back to the hub during the tutorial
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
