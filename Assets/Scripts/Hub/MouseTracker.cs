/*
 * Script for mouse movement when navigating the hub.
 * The movement of the mouse will influence where the camera is.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class MouseTracker : MonoBehaviour
{
    private InputAction _mousePosAction;

    public float MouseRadius = 3;   // How far our tracker is allowed to be from the player (not one-to-one with mouse)

    private Vector2 _mousePos;
    private float _mouseTrackerX;
    private float _mouseTrackerY;
    void Start()
    {
        _mousePosAction = InputSystem.actions.FindAction("Mouse Position");
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = _mousePosAction.ReadValue<Vector2>();
        _mouseTrackerX = _mousePos.x - (1920/2);
        _mouseTrackerY = _mousePos.y - (1080/2);

        // Imposing bounds
        _mouseTrackerX = math.remap(- Screen.width / 2, Screen.width / 2, -MouseRadius, MouseRadius, _mouseTrackerX);
        _mouseTrackerY = math.remap(-Screen.height / 2, Screen.height / 2, -MouseRadius, MouseRadius, _mouseTrackerY);

        if (_mousePos.x < 0 || _mousePos.x > Screen.width)
            _mouseTrackerX = 0;
        if (_mousePos.y < 0 || _mousePos.y > Screen.height)
            _mouseTrackerY = 0;

        this.transform.position = new Vector3(_mouseTrackerX, _mouseTrackerY, this.transform.position.z); // localP
    }
}
