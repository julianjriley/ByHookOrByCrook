/*
 * Script for mouse movement when navigating the hub.
 * The movement of the mouse will influence where the camera is.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{
    private InputAction _mousePosAction;

    public float MouseRadius = 1;   // How far our tracker is allowed to be from the player (not one-to-one with mouse)

    private Vector2 _mousePos;
    private float _mouseTrackerX;
    private float _mouseTrackerZ;
    void Start()
    {
        _mousePosAction = InputSystem.actions.FindAction("Mouse Position");
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = _mousePosAction.ReadValue<Vector2>();
        Debug.Log(_mousePos.x + " " +  _mousePos.y);
        _mouseTrackerX = _mousePos.x - (1920/2);
        _mouseTrackerZ = _mousePos.y - (1080/2);

        // Imposing bounds
        if (_mouseTrackerX > MouseRadius)
            _mouseTrackerX = MouseRadius;
        if (_mouseTrackerX < -MouseRadius)
            _mouseTrackerX = -MouseRadius;

        if (_mouseTrackerZ > MouseRadius)
            _mouseTrackerZ = -MouseRadius;
        if (_mouseTrackerZ < -MouseRadius)
            _mouseTrackerZ = MouseRadius;

        this.transform.localPosition = new Vector3(_mouseTrackerX, this.transform.position.y, _mouseTrackerZ);
    }
}
