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
        _mouseTrackerX = _mousePos.x - (1920/2);
        _mouseTrackerZ = _mousePos.y - (1080/2);

        // Imposing bounds
        _mouseTrackerX = math.remap(- Screen.width / 2, Screen.width / 2, -MouseRadius, MouseRadius, _mouseTrackerX);
        _mouseTrackerZ = math.remap(-Screen.height / 2, Screen.height / 2, MouseRadius, -MouseRadius, _mouseTrackerZ);

        this.transform.localPosition = new Vector3(_mouseTrackerX, this.transform.position.y, _mouseTrackerZ);
    }
}
