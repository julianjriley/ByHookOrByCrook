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

// TODO
// Figure out how to store an NPC's icon and script so they can be called up
// Set up UI for conversations
// Set up UI so that the amount of gold you have is displayed only when in front of a shop, DKC style
// Purchases will have to wait for the GameManager so I can understand how all of this is being stored.

public class Interactor : MonoBehaviour
{
    private InputAction _interactAction;
    
    [Tooltip("The prompt to interact with the object (press E)")]
    [SerializeField] private GameObject _interactPrompt;
    [Tooltip("Whether this interaction point is for an NPC or a shop point.")]
    public bool IsNPC;
    
    // An NPC interaction point will lock the player into an interaction.
    // A shop interaction point will attempt to use the player's money to purchase something.
    [Header("NPC Interaction Variables")]
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [Tooltip("The main hub camera, and the camera focused on this specific interaction")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _interactCamera;
    [Tooltip("The player's movement script")]
    [SerializeField] private HubMovement _player;

    [Header("Shop Interaction Variables")]
    [Tooltip("Whether to get rid of this shop after something is bought.")]
    [SerializeField] private bool _destroyOnPurchase;

    bool isActiveCoroutine;
    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !isActiveCoroutine)
        {
            if (IsNPC) // If E is pressed on an NPC
            {
                StartCoroutine(DoInteractionNPC());
            }
            else // If E is pressed on a shop
            {
                // Check if they have enough money
                // If they do, sell it to 'em
                // If not, [LOUD INCORRECT BUZZER NOISE]
            }
        }
    }

    private IEnumerator DoInteractionNPC()
    {
        isActiveCoroutine = true;

        // Deactivate the interaction prompt
        _interactPrompt.SetActive(false);

        // Switch cameras
        _interactCamera.SetActive(true);
        _mainCamera.SetActive(false);

        // Stop player movement
        _player.IsIdle = true;

        // Disable the interaction trigger so the player pressing E to talk doesn't re-trigger the interaction

        yield return new WaitForSeconds(3f);

        // Switch cameras back
        _interactCamera.SetActive(false);
        _mainCamera.SetActive(true);

        // Give player movement back
        _player.IsIdle = false;

        isActiveCoroutine = false;
        yield return null;
    }

    #region TRIGGER PROXIMITY CHECKS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interactPrompt.SetActive(true);
            if(IsNPC)
                _cryerPrompt.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interactPrompt.SetActive(false);
        }
    }
    #endregion

}
