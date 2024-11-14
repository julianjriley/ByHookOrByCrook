using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HubMusicZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.Instance.SetParameter(SoundManager.Instance.musicEventInstance, "HubPosition", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SoundManager.Instance.SetParameter(SoundManager.Instance.musicEventInstance, "HubPosition", 0);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SoundManager.Instance.SetParameter(SoundManager.Instance.musicEventInstance, "HubPosition", 1);
    }
}

