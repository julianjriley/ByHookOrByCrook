using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTTRLManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject _hubPlayer;
    private HubMovement _pMovement;

    [Header("Skipper")]

    [Header("NPCs")]
    [SerializeField] private NPCInteractor _rodNPC;
    [SerializeField] private NPCInteractor _baitNPC;
    [SerializeField] private NPCInteractor _bagNPC;

    [SerializeField] private Conversation _rodTutConvo;
    [SerializeField] private Conversation _baitTutConvo;
    [SerializeField] private Conversation _bagTutConvo;

    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialHub)
        {
            _pMovement = _hubPlayer.GetComponent<HubMovement>();
            StartCoroutine(DoTutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoTutorial()
    {
        // All of the shops are toggled off by default when IsTutorialHub is active
        // Set the unique NPC convos
        _rodNPC.SetConversation(_rodTutConvo, -1);
        _baitNPC.SetConversation(_baitTutConvo, -1);
        _bagNPC.SetConversation(_bagTutConvo, -1);

        // Player falls from the sky and cannot move
        // Text box & skipper sprite appears, gab gab gab
        // Skipper disappears, leaving a sign behind
        // Player is allowed to walk to the dock, explore, all that; hub portion of the tutorial is over
        yield return null;
    }
}
