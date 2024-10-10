using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private GameObject _player;
    
    [SerializeField] private Transform topOfPlatform;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if(topOfPlatform.position.y > _player.transform.position.y)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
        }
    }
}
