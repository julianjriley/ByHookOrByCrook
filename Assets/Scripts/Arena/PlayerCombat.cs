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
    [SerializeField] private Transform _weaponsTransform;
    [SerializeField] private Transform _passivesTransform;

    private WeaponInstance _equippedWeapon;

    //Total Inventory
    private Inventory _inventory;

    //Weapons Inventory
    private List<WeaponInstance> _weapons;

    //Basically for testing and stuff
    //[SerializeField] private PassiveItem _testItem;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        controls = new ActionControls();
    }
    void Start()
    {
        controls.Player.FireWeapon.Enable();
        controls.Player.FireWeapon.started += FireWeapon;
        ResetStats();

        /*
        
        Just Showing off How inventory works feel free to annihilate this
        _inventory = new Inventory();
        _inventory.AddItem(_testItem);

        PassiveItem itemTest = _inventory.items[0] as PassiveItem;
        itemTest.SetPlayer(this);
        itemTest.CreatePrefabOnPlayer();
        */
    }



    void FireWeapon(InputAction.CallbackContext context)
    {
        if(_equippedWeapon != null)
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

    void ResetStats()
    {
        _health = _baseHealth;
        _speed = _baseSpeed;
    }

    public void AppendItemToPassiveInstances(GameObject prefab)
    {
        GameObject instantiatedPrefab = Instantiate(prefab, _passivesTransform);
    }
}
