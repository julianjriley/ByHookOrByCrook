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
    private InputAction _interactAction;
    private BoxCollider2D _col;
    private bool _canInteract;
    
    [Tooltip("The prompt to interact with the object (press E)")]
    [SerializeField] private GameObject _interactPrompt;
    [Tooltip("Whether this interaction point is for an NPC or a shop point.")]
    public bool IsNPC;
    
    // An NPC interaction point will lock the player into an interaction.
    // A shop interaction point will attempt to use the player's money to purchase something.
    [Header("NPC Interaction Variables")]
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [Tooltip("The text box and text item that will be used for talking")]
    [SerializeField] private GameObject _convoBubble;
    [SerializeField] private TextMeshPro _convoText;
    [Tooltip("The object that represents the conversation script")]
    [SerializeField] private Conversation _conversation;
    private int _convoIndex;
    [Tooltip("The main hub camera, and the camera focused on this specific interaction")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _interactCamera;
    [Tooltip("The player's movement script")]
    [SerializeField] private HubMovement _player;

    [Header("Shop Interaction Variables")]
    [Tooltip("Whether to get rid of this shop in the scene after something is bought.")]
    [SerializeField] private bool _destroyOnPurchase;
    [Tooltip("Price of the item")]
    [SerializeField] private int _cost;

    bool isActiveCoroutine;
    bool isSkippingLine;
    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !isActiveCoroutine && _canInteract)
        {
            if (IsNPC) // If E is pressed on an NPC
            {
                StartCoroutine(DoInteractionNPC());
            }
            else // If E is pressed on a shop
            {
                StartCoroutine(DoInteractionShop());
            }
        }
    }

    public void SetConversation(Conversation convo, int index)
    {
        _conversation = convo;
        _convoIndex = index;
    }

    private IEnumerator DoInteractionShop()
    {
        // Check if they have enough money
        if(GameManager.Instance.GamePersistent.Gill >= _cost)
        {
            // Remove the cost from their balance
            GameManager.Instance.GamePersistent.Gill -= _cost;
            // If this is a one-time purchase shop
            //TODO:
            // How do we add an item to player inventory
            // How do we store shop states so that a rod shop restocks a rod, a bait shop point stays purchased after it's bought, etc.
            // Shop types
                // One purchase, and that's it, shop's closed. (Bait)
                // Multiple purchases of the same thing until you hit an upper cap (Bait space, battle space)
                // Potentially dynamic pricing?
                // One purchase, but more to come (Rods)

            if (_destroyOnPurchase) // If this shop shouldn't be accessed again within this scene, get rid of it
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // If not, [LOUD INCORRECT BUZZER NOISE]
            // We can put a little sound effect and a little text box animation here to really
            // emphasize to the player that they are poor
        }
        yield return null;

    }

    #region NPC COROUTINES

    private IEnumerator DoInteractionNPC()
    {
        isActiveCoroutine = true;
        _col.enabled = false;

        // Deactivate the interaction prompt
        _interactPrompt.SetActive(false);

        // Switch cameras
        _interactCamera.SetActive(true);
        _mainCamera.SetActive(false);

        // Stop player movement
        _player.IsIdle = true;

        // Show the discussion box
        _convoBubble.SetActive(true);

        // Set up the conversation
        GameManager.Instance.GamePersistent.IsConvoHad[_convoIndex] = true;
        foreach(string line in _conversation.lines)
        {
            isSkippingLine = false;
            StartCoroutine(DoTextEscapeSubroutine());
            for(int i = 0; i < line.Length; i++)
            {
                if (!isSkippingLine)
                {
                    if (line[i].Equals(' '))
                    {
                        i++;
                    }
                    _convoText.text = line.Substring(0, i);

                    yield return new WaitForSeconds(.07f);
                }
                else { 
                    _convoText.text = line;
                    break;
                }
                
            }
            StopCoroutine(DoTextEscapeSubroutine());
            _convoText.text = line;
            yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => _interactAction.IsPressed());
        }
        // Hide the discussion box
        _convoBubble.SetActive(false);

        // Switch cameras back
        _interactCamera.SetActive(false);
        _mainCamera.SetActive(true);

        // Give player movement back
        _player.IsIdle = false;

        yield return new WaitForSeconds(3f);
        isActiveCoroutine = false;
        _col.enabled = true;
        yield return null;
    }

    private IEnumerator DoTextEscapeSubroutine()
    {
        yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
        yield return new WaitUntil(() => _interactAction.IsPressed());
        isSkippingLine = true;
    }

    #endregion

    #region TRIGGER PROXIMITY CHECKS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = true;
            _interactPrompt.SetActive(true);
            if(IsNPC)
                _cryerPrompt.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = false;
            _interactPrompt.SetActive(false);
        }
    }
    #endregion

}
