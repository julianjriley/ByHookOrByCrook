using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    [SerializeField] EventReference music;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.InitializeMusic(music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
