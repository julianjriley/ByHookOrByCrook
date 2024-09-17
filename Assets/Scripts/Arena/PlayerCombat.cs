using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    private ActionControls controls;

    //Player Movement Script
    ArenaMovement playerMovement;

    //Stats
    [SerializeField] private int _baseHealth;
    private int _health;
    [SerializeField] private float _baseSpeed;
    private float _speed;

    //Children Transforms
    private Transform _weaponsTransform;
    private Transform _passivesTransform;

    private WeaponInstance _equippedWeapon;

    //Total Inventory
    private Inventory _items;

    //Weapons Inventory
    private List<WeaponInstance> _weapons;

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.FireWeapon.started += FireWeapon;
    }


    void Start()
    {
        Inventory items = new Inventory();
    }



    void FireWeapon(InputAction.CallbackContext context)
    {
        _equippedWeapon.Fire(Vector3.zero);
    }




    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public Transform GetWeaponsTransform()
    {
        return _weaponsTransform;
    }

    public Transform GetPassivesTransform()
    {
        return _passivesTransform;
    }
}
