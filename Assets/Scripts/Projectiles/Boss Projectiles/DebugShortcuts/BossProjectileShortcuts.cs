using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossProjectileShortcuts : MonoBehaviour
{
    private const string MOUSE_INPUT = "Mouse Position";

    private ActionControls _controls;
    private InputAction _mouseInput;

    private int _currProjectile = 0;
    [SerializeField, Tooltip("Max number of projectiles that currently have shortcuts to spawn them.")]
    private int _numOfProjectileTypes;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] spawners;

    private void Awake()
    {
        _controls = new ActionControls();
        _mouseInput = InputSystem.actions.FindAction(MOUSE_INPUT);
    }

    private void Start()
    {
        _controls.Player.SwitchWeapon.started += SwitchCurrProjectile;
        _controls.Player.FireWeapon.started += SpawnProjectile;

        _controls.Player.SwitchWeapon.Enable();
        _controls.Player.FireWeapon.Enable();
    }

    private void SwitchCurrProjectile(InputAction.CallbackContext context)
    {
        // increment or loop current projectile
        _currProjectile++;
        if (_currProjectile >= _numOfProjectileTypes)
            _currProjectile = 0;
    }

    private void SpawnProjectile(InputAction.CallbackContext context)
    {
        // get current mouse position
        Vector2 mousePos = _mouseInput.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        // spawn projectile
        switch(_currProjectile)
        {
            case 0: // Pixel Ink

                GameObject newSpawner = Instantiate(spawners[_currProjectile], worldPos, spawners[_currProjectile].transform.rotation);

                // facing center (0, 0)
                Quaternion rot = new Quaternion();
                rot.SetFromToRotation(Vector3.right, -worldPos);

                newSpawner.transform.rotation = rot * transform.rotation;

                break;
        }
    }
}
