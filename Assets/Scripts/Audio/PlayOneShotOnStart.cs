using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotOnStart : MonoBehaviour
{
    [SerializeField] EventReference oneShot;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayOneShot(oneShot, gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
