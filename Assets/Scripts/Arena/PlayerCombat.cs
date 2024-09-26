using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    private ActionControls controls;
    Camera cam;

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
    [SerializeField] private Transform _aimPivot;

    private WeaponInstance _equippedWeapon;

    //Total Inventory
    private Inventory _inventory;

    //Weapons Inventory
    private List<WeaponInstance> _weapons;

    //Basically for testing and stuff
    [SerializeField] private Weapon testWeapon;

    //Firing Stuff
    Vector2 mousePosition;
    Vector3 worldPos;
    Vector2 weaponDirection;

    private void OnEnable()
    {
        cam = Camera.main;
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

        _inventory = new Inventory();
        _inventory.AddItem(testWeapon);
        Weapon weapon = _inventory.items[0] as Weapon;
        weapon.SetPlayer(this);
        _equippedWeapon = _weaponsTransform.GetComponentInChildren<WeaponInstance>();
        
    }



    void FireWeapon(InputAction.CallbackContext context)
    {
        if(_equippedWeapon != null)
            _equippedWeapon.Fire(weaponDirection);
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        worldPos = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

    }

    private void FixedUpdate()
    {

        if (_equippedWeapon != null)
        {
            weaponDirection = (worldPos - _aimPivot.transform.position).normalized;
            //_equippedWeapon.UpdateRotation(weaponDirection);
            UpdateRotation(weaponDirection);
            
        }


    }
    public void UpdateRotation(Vector2 lookAt)
    {
        _aimPivot.transform.rotation = Quaternion.FromToRotation(Vector3.right, lookAt);
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

    public void AppendItemToWeaponInstances(GameObject prefab)
    {
        GameObject instantiatedPrefab = Instantiate(prefab, _weaponsTransform);
    }
}
