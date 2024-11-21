using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class NPCInteractor : Interactor
{
    [Header("NPC Interaction Variables")]
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [Tooltip("The script for that text box")]
    [SerializeField] private CryerPrompt _cryerScript;
    [Tooltip("The text box and text item that will be used for talking")]
    [SerializeField] private GameObject _convoBubble;
    [SerializeField] private GameObject _newConvoNotif;
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
    [SerializeField] EventReference dialogueSound;

    bool isSkippingLine;

    new void Start()
    {
        base.Start();
        _newConvoNotif.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !_isActiveCoroutine && _canInteract)
        {
          StartCoroutine(DoInteractionNPC());
        }


        if (_convoIndex >= 0)
        {
            if (!getIsConvoHad()[_convoIndex])
            {
                if (getSetConvo().GeneralPriority == 0 && !getNPCConvoExhausted())
                {
                    _newConvoNotif.SetActive(true);
                }
                else if (getSetConvo().GeneralPriority != 0 || getSetConvo().RequiredBossLossCount != 0)
                {
                    _newConvoNotif.SetActive(true);
                }
                else
                {
                    _newConvoNotif.SetActive(false);
                }
            }
            
        }
        else if(_convoIndex < 0)
        {
            _newConvoNotif.SetActive(true);
        }
        else
        {
            _newConvoNotif.SetActive(false);
        }


    }

    public void SetConversation(Conversation convo, int index)
    {
        _conversation = convo;
        _convoIndex = index;
        _cryerScript.IsCurrentConvoHad = false;
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

        // Stop other SFX
        SoundManager.Instance.CleanButSpare("Hub", false);

        // Show the discussion box
        _convoBubble.SetActive(true);

        // Set up the conversation
        if (_convoIndex >= 0)
            getIsConvoHad()[_convoIndex] = true;
        _cryerScript.IsCurrentConvoHad = true;

        foreach (string line in _conversation.lines)
        {
            isSkippingLine = false;
            SoundManager.Instance.InitializeDialogue(dialogueSound); //start dialogue sound
            SoundManager.Instance.SetGlobalParameter("MusicDown", 1);//turn music down
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
            SoundManager.Instance.StopDialogue();//stop dialogue sound
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
        
        // Bring music back to full volume
        SoundManager.Instance.SetGlobalParameter("MusicDown", 0);

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

    public Conversation getSetConvo()
    {
        return _conversation;
    }

    public bool getNPCConvoExhausted()
    {
            if (GoodsSold == ShopType.Rod)
            {
                return GameManager.Instance.GamePersistent.AllConvosHadRod;
            }
            else if (GoodsSold == ShopType.WeaponBait)
            {
                return GameManager.Instance.GamePersistent.AllConvosHadBait;
            }
            else
            {
                return GameManager.Instance.GamePersistent.AllConvosHadBag;
            }
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
