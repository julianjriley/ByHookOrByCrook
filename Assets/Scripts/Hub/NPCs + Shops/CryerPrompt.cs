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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _cryerPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _cryerPrompt.SetActive(false);
        }
    }
}
