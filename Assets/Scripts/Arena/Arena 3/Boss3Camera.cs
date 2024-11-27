using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Camera : MonoBehaviour
{
    [SerializeField] private float _topCameraSpeed = 2f;

    private float _currentCameraSpeed = 0;
    [Header("Ramping Variables For Round Start")]
    [SerializeField, Tooltip("How much closer we get to full camera speed each [delay]")] private float _rampIncrement;
    [SerializeField, Tooltip("How long we wait before each [increment]")] private float _rampDelay;
    private bool _cameraRampingUp = false;

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
                this.transform.position = new Vector3(this.transform.position.x, _finalStop, this.transform.position.z);
            }
        }

        if(!_fullStop)
            this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (_currentCameraSpeed * Time.deltaTime), transform.position.z);
    }

    public void SetRestingPlace(float yVal)
    {
        _finalStop = yVal;
    }

    public void SetCamMoving()
    {
        if (!_cameraRampingUp)
        {
            StartCoroutine(DoCamRampUp());
        }
        
    }

    private IEnumerator DoCamRampUp()
    {
        _cameraRampingUp = true;
        while(_currentCameraSpeed < _topCameraSpeed)
        {
            _currentCameraSpeed += _rampIncrement;
            yield return new WaitForSeconds(_rampDelay);
        }
        _currentCameraSpeed = _topCameraSpeed;
    }

    public bool GetFullStop()
    {
        return _fullStop;
    }
}
