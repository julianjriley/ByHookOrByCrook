using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Camera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = .5f;

    private float _finalStop = -1;
    private bool _fullStop = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(_finalStop > 0) // If we've set a final stopping point
        {
            if(this.transform.position.y >= _finalStop)
            {
                _fullStop = true;
            }
        }

        if(!_fullStop)
            this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (cameraSpeed * Time.deltaTime), transform.position.z);
    }

    public void SetRestingPlace(float yVal)
    {
        _finalStop = yVal;
    }
}
