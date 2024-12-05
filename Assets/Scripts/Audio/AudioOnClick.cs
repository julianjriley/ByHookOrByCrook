using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnClick : MonoBehaviour
{
    [SerializeField] EventReference clickSound;
    [SerializeField] EventReference cantClickSound;
    [SerializeField] Button thisButton;
    // Start is called before the first frame update

    private void Start()
    {
        thisButton = GetComponent<Button>();
    }
    public void PlayAudioOnClick()
    {
        if (thisButton.interactable)
        {
            SoundManager.Instance.PlayOneShot(clickSound, gameObject.transform.position);
        }
        else
        {
            SoundManager.Instance.PlayOneShot(clickSound, gameObject.transform.position);
        }
    }
}
