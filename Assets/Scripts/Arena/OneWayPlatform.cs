using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private GameObject _player;
    ArenaMovement _playerMovement;
    [SerializeField] private Transform topOfPlatform;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<ArenaMovement>();
    }


    //See if the player is below the platform or holding the down key
    private void FixedUpdate()
    {
        if(topOfPlatform.position.y > _player.transform.position.y || _playerMovement.GoThroughPlatforms < 0)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
        }
    }
}
