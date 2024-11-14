using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private GameObject _player;
    ArenaMovement _playerMovement;
    [SerializeField] private Transform topOfPlatform;
    private Collider _collider;
    //Must equal one in order to be used again
    public float _canBeUsed = 1;
    bool _used = false;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<ArenaMovement>();
        //This is needed just trust me
        topOfPlatform.localPosition = new Vector3(0, -0.5f, 0);

    }

    private void Update()
    {
        _canBeUsed = Mathf.Clamp(_canBeUsed + 2 * Time.deltaTime, 0, 1);

    }

    //See if the player is below the platform or holding the down key
    private void FixedUpdate()
    {

        if (_playerMovement.bottomOfFeet.position.y < topOfPlatform.position.y || _playerMovement.GoThroughPlatforms < 0)
        {
            //if(_playerMovement.GoThroughPlatforms < 0 )
            //   _canBeUsed = 0;
            _collider.isTrigger = true;
        }
        else
        {
            //if (_canBeUsed >= 1)
            //   _collider.isTrigger = false
            if(!_used)
                _collider.isTrigger = false;
        }
        


    }

    IEnumerator Die()
    {
        _used = true;
        _collider.isTrigger = true;
        yield return new WaitForSeconds(0.1f);
        _used = false;
        _collider.isTrigger = false;

    }

    private void OnCollisionStay(Collision collision)
    {
        if(_playerMovement.GoThroughPlatforms < 0)
            StartCoroutine(Die());
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if(_playerMovement.bottomOfFeet.position.y < topOfPlatform.position.y)
        {
            //_canBeUsed = 0;
            //_collider.isTrigger = true;
        }
    }
    */






}
