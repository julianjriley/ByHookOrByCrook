using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMusicContactTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.SetParameter(SoundManager.Instance.musicEventInstance, "BaitSelect", 1);
        SoundManager.Instance.CleanButSpare("Hub", true);
    }
}
