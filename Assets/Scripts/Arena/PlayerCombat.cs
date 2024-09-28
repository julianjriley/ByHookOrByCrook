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
    int equippedWeaponindex = 0;

    //RottenFish Default Gun
    [SerializeField] private Weapon defaultWeapon;

    //Testing purposes, can be disposed of whenever
    [SerializeField] private Weapon testWeapon2;

    //Firing Stuff
    Vector2 mousePosition;
    Vector3 worldPos;
    Vector2 weaponDirection;

    bool _facingLeft;

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
        controls.Player.FireWeapon.started += FireWeapon;
        controls.Player.FireWeapon.canceled += FireWeapon;
        controls.Player.FireWeapon.Enable();

        controls.Player.SwitchWeapon.Enable();
        controls.Player.SwitchWeapon.performed += ChangeWeapon;
        
        
        ResetStats();
        _weapons = new List<WeaponInstance>();

        

        _inventory = new Inventory();

        AddItemToPlayer(defaultWeapon);

        //Can Be gotten rid of whenever
        AddItemToPlayer(testWeapon2);

        StartCoroutine(EnableStartingWeaponVisual());
        
        
    }



    void FireWeapon(InputAction.CallbackContext context)
    {
        if(_equippedWeapon != null && context.started)
        {
            FireFunctionality();
        }
        else if(_equippedWeapon != null && context.canceled)
        {
            Debug.Log("NONO");
            CeaseFire();
        }
            
    }

    void ChangeWeapon(InputAction.CallbackContext context)
    {
       
        _equippedWeapon.CeaseFire();
        if(context.ReadValue<float>() > 0)
        {
            equippedWeaponindex += 1;
        }
        else
        {
            equippedWeaponindex -= 1;
        }
        
        
        
        if(equippedWeaponindex > _weapons.Count - 1)
        {
            equippedWeaponindex = 0;
        }
        else if(equippedWeaponindex < 0)
            equippedWeaponindex = _weapons.Count - 1;

        _equippedWeapon.DisableRendering();
        _equippedWeapon = _weapons[equippedWeaponindex];
        _equippedWeapon.EnableRendering();
        if (controls.Player.FireWeapon.IsPressed())
        {
            _equippedWeapon.SetAim(weaponDirection);
            FireFunctionality();
        }
            
        Debug.Log(equippedWeaponindex);

    }

    void FireFunctionality()
    {
        if(_equippedWeapon != null)
        {
            _equippedWeapon.Fire(weaponDirection);
        }
    }

    void CeaseFire()
    {
        if(_equippedWeapon != null)
        {
            _equippedWeapon.CeaseFire();
        }
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        worldPos = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 11));
        if(_equippedWeapon != null)
            _equippedWeapon.SetAim(weaponDirection);
        if (worldPos.x < gameObject.transform.position.x)
        {
            _facingLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
            
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            _facingLeft = false;
        }
        
        
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
        if (!_facingLeft)
            _aimPivot.transform.rotation = Quaternion.FromToRotation(Vector3.right, lookAt);
        else
            _aimPivot.transform.rotation = Quaternion.FromToRotation(-Vector3.right, lookAt);
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

    public void AddItemToPlayer(Item theItem)
    {
        if(theItem is PassiveItem)
        {
            _inventory.AddItem(theItem);
            (theItem as PassiveItem).SetPlayer(this);
        }
        else if(theItem is Weapon)
        {
            _inventory.AddItem(theItem);
            (theItem as Weapon).SetPlayer(this);
        }
    }

    //Need like a split second of time before the player's weapon can be equipped and rendered
    
    IEnumerator EnableStartingWeaponVisual()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (WeaponInstance weaponInstance in _weaponsTransform.GetComponentsInChildren<WeaponInstance>())
        {
            _weapons.Add(weaponInstance);
        }
        _equippedWeapon = _weapons[0];
        _equippedWeapon.EnableRendering();
    }
}
