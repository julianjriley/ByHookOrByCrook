using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryerPrompt : MonoBehaviour
{
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;

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
