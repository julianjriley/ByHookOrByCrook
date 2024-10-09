/*
 * Script for hub interaction. An interactor should show a prompt for
 * interaction when walked through, and be able to trigger a conversation
 * or manage a purchase.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

// TODO
// Figure out how to store an NPC's icon and script so they can be called up
// Set up UI for conversations
// Set up UI so that the amount of gold you have is displayed only when in front of a shop, DKC style
// Purchases will have to wait for the GameManager so I can understand how all of this is being stored.

public class Interactor : MonoBehaviour
{
    protected InputAction _interactAction;
    protected BoxCollider2D _col;
    protected bool _canInteract;
    
    [Tooltip("The prompt to interact with the object (press E)")]
    [SerializeField] protected GameObject _interactPrompt;

    public enum ShopType { Rod, BaitSpace, BagSpace, WeaponBait, AttackBait, MovementBait, SupportBait };
    public ShopType GoodsSold;

    protected bool _isActiveCoroutine;

    protected void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _col = GetComponent<BoxCollider2D>();  
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    #region TRIGGER PROXIMITY CHECKS
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = true;
            _interactPrompt.SetActive(true);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = false;
            _interactPrompt.SetActive(false);
        }
    }
    #endregion

}
