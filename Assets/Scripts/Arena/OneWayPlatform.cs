using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private GameObject _player;
    ArenaMovement _playerMovement;
    [SerializeField] private Transform topOfPlatform;
    private Collider _collider;
    //Must equal one in order to be used again
    float _canBeUsed = 1;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<ArenaMovement>();
        //This is needed just trust me
        topOfPlatform.localPosition = new Vector3(0, 1.9f, 0);
    }

    private void Update()
    {
        _canBeUsed = Mathf.Clamp(_canBeUsed + 2 * Time.deltaTime,0,1);
    }

    //See if the player is below the platform or holding the down key
    private void FixedUpdate()
    {
        if(topOfPlatform.position.y > _player.transform.position.y || _playerMovement.GoThroughPlatforms < 0)
        {
            if(_playerMovement.GoThroughPlatforms < 0)
                _canBeUsed = 0;
            _collider.enabled = false;
        }
        else
        {
            if(_canBeUsed >= 1)
                _collider.enabled = true;
        }

        
    }

    
}
