using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractor : Interactor
{
    [Header("NPC Interaction Variables")]
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [Tooltip("The text box and text item that will be used for talking")]
    [SerializeField] private GameObject _convoBubble;
    [SerializeField] private TextMeshPro _convoText;
    private Conversation _conversation;
    private int _convoIndex;
    [Tooltip("Delay for re-interacting with an NPC")]
    [SerializeField] private float _endDelay = 1f;
    [Tooltip("The main hub camera, and the camera focused on this specific interaction")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _interactCamera;
    [Tooltip("The player's movement script")]
    [SerializeField] private HubMovement _player;

    bool isSkippingLine;

    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !_isActiveCoroutine && _canInteract)
        {
          StartCoroutine(DoInteractionNPC());
        }
    }

    public void SetConversation(Conversation convo, int index)
    {
        _conversation = convo;
        _convoIndex = index;
    }
    private List<bool> getIsConvoHad()
    {
        if (GoodsSold == ShopType.Rod)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadRod;
        }
        else if (GoodsSold == ShopType.BagSpace || GoodsSold == ShopType.BaitSpace)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBag;
        }
        else
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBait;
        }
    }

    #region NPC COROUTINES

    private IEnumerator DoInteractionNPC()
    {
        _isActiveCoroutine = true;
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
        if (_convoIndex >= 0)
            getIsConvoHad()[_convoIndex] = true;
        foreach (string line in _conversation.lines)
        {
            isSkippingLine = false;
            StartCoroutine(DoTextEscapeSubroutine());
            for (int i = 0; i < line.Length; i++)
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
                else
                {
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

        yield return new WaitForSeconds(_endDelay);
        _isActiveCoroutine = false;
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

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _cryerPrompt.SetActive(false);
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D (collision);
    }

}