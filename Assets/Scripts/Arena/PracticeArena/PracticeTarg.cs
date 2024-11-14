using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTarg : MonoBehaviour
{
    private BoxCollider _boxCol;
    [SerializeField] private Animator _anim;

    void Start()
    {
        _boxCol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit()
    {
        
        _anim.Play("Bobble", 0, 0);
    }
}
