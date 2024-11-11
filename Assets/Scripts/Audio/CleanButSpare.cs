using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanButSpare : MonoBehaviour
{
    [SerializeField] string spare;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.CleanButSpare(spare);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
