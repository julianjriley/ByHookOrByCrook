using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCleanUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.CleanUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}