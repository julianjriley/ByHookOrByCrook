using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaMovement : MonoBehaviour
{
    PlayerCombat playerCombat;
    Rigidbody rb;
    ActionControls controls;

    InputAction movement;
    private void OnEnable()
    {
        
        
    }
    private void Awake()
    {
        controls = new ActionControls();
    }
    private void Start()
    {
        controls.Player.MoveArena.Enable();
        movement = controls.Player.MoveArena;

        rb = GetComponent<Rigidbody>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement.ReadValue<Vector2>().normalized * playerCombat.Speed;
    }


}
