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

public class Interactor : MonoBehaviour
{
    protected InputAction _interactAction;
    protected BoxCollider2D _col;
    protected bool _canInteract;
    
    [Tooltip("The prompt to interact with the object (press E)")]
    [SerializeField] protected GameObject _interactPrompt;

    public enum ShopType { Rod, BaitSpace, BagSpace, WeaponBait, AttackBait, MovementBait, SupportBait, Skipper, Bobber, Nothing };
    public ShopType GoodsSold;

    protected bool _isActiveCoroutine;

    private Animator _interactAnim;

    protected void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _col = GetComponent<BoxCollider2D>();  

        _interactAnim = _interactPrompt.GetComponentInChildren<Animator>();

        _interactPrompt.SetActive(true);
        _interactAnim.Play("Disappear", 0, 0);
        _interactPrompt.SetActive(false);
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
            _interactAnim.Play("Appear", 0, 0);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = false;
            StartCoroutine(DoDisappear());
        }
    }

    private IEnumerator DoDisappear()
    {
        _interactAnim.Play("Disappear", 0, 0);
        yield return new WaitForSeconds(.2f);
        _interactPrompt.SetActive(false);
    }
    #endregion

}
