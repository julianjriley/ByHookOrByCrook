using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryerPrompt : MonoBehaviour
{
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [SerializeField] private GameObject _newConvo;
    [SerializeField] private NPCInteractor _npcInteractor;

    public bool IsCurrentConvoHad;

    private Animator _cryerAnim;

    private void Start()
    {
        _cryerAnim = GetComponentInChildren<Animator>();
        _cryerPrompt.SetActive(true);
        _cryerAnim.Play("Disappear", 0, 0);
        _cryerPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _cryerPrompt.SetActive(true);
            _cryerAnim.Play("Appear", 0, 0);
            if (!IsCurrentConvoHad)
            {
                if (_npcInteractor.getSetConvo().GeneralPriority == 0 && !_npcInteractor.getNPCConvoExhausted())
                {
                    _newConvo.SetActive(true);
                }
                else if (_npcInteractor.getSetConvo().GeneralPriority != 0 || _npcInteractor.getSetConvo().RequiredBossLossCount != 0)
                {
                    _newConvo.SetActive(true);
                }
                else
                {
                    _newConvo.SetActive(false);
                }
            }
            else
            {
                _newConvo.SetActive(false);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _cryerPrompt.activeInHierarchy)  
        {
            _cryerAnim.Play("Disappear", 0, 0);
        }
    }
}
