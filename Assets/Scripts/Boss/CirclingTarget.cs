using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclingTarget : MonoBehaviour
{
    public float Radius = 1f; //adjust the distance circled
    public float Speed = 1f; //make the target circle faster
    public bool Clockwise = true; //can only change on start
    private float _timeCounter = 0f;
    private float _xOffset;
    private float _yOffset;
    private float _direction = 1f;
    void Start() {
        _xOffset = transform.position.x;
        _yOffset = transform.position.y;
        if (Clockwise) {
            _direction = -1f;
        }
    }
    void Update()
    {
        _timeCounter += Time.deltaTime;
        float xPos = Mathf.Cos(_timeCounter * Speed * _direction);
        float yPos = Mathf.Sin(_timeCounter * Speed * _direction);
        transform.position = new Vector3((xPos * Radius) + _xOffset, (yPos * Radius) + _yOffset, transform.position.z); //offset value change by current x and y values
    }

    /// <summary>
    /// Updates origin around which target circles.
    /// </summary>
    public void SetNewCenter(float newX, float newY)
    {
        _xOffset = newX;
        _yOffset = newY;
    }
}
