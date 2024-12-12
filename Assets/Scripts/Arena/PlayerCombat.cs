using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour, IDamageable
{

    private InputActionMap controls;
    Camera cam;

    //Player Movement Script
    ArenaMovement playerMovement;
    Rigidbody rb;

    //Stats
    [SerializeField] private int _baseHealth;
    private int _health;
    //[SerializeField] private float _baseSpeed;
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
    //[SerializeField] private Weapon defaultWeapon;

    //Firing Stuff
    Vector2 mousePosition;
    Vector3 worldPos;
    Vector2 weaponDirection;

    bool _facingLeft;

    bool dead;
    //Damage taking stuff
    public bool BossDefeated = false;
    private bool _invulnerable;
    private bool _assistInvulnerable;
    private Collider _collider;
    private LayerMask _invulnerabilityMask;
    private Coroutine _invulnerableWindow;

    //Sprite Renderer
    SpriteRenderer _spriteRenderer;

    public delegate void PlayerDied();
    public static event PlayerDied playerDeath;
    public delegate void HealthChange(int health);
    public event HealthChange HealthChanged;
    public static event Action<bool> PlayerIsZombie;
    public static event Action<int> WeaponSwitched;

    //Buff Specific Variables
    public bool useShortRangeDamage = false;
    public bool canRevive = false;
    private bool _hasRevived = false;
    public bool canInvincibleDash = false;
    public static event Action DeadFishUIEvent;

    //Skipper
    [SerializeField] GameObject skipper;
    

    [SerializeField] EventReference damageSound;

#if UNITY_EDITOR 
    [Header("Unity Editor Only")]
    [SerializeField] PassiveItem[] testItems;
    [SerializeField] Weapon[] testWeapons;
#endif

    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void Awake()
    {
        controls = InputSystem.actions.actionMaps[0];
    }
    void Start()
    {
        controls.FindAction("FireWeapon").started += FireWeapon;
        controls.FindAction("FireWeapon").canceled += FireWeapon;
        controls.FindAction("FireWeapon").Enable();

        controls.FindAction("SwitchWeapon").Enable();
        controls.FindAction("SwitchWeapon").performed += ChangeWeapon;

        ResetStats();
        _weapons = new List<WeaponInstance>();
        playerMovement = GetComponent<ArenaMovement>();
        rb = GetComponent<Rigidbody>();
        gameObject.AddComponent<EffectManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        equippedWeaponindex = 0;

        BossDefeated = false;
        _invulnerabilityMask = LayerMask.GetMask("Boss", "BreakableBossProjectile", "BossProjectile");

        _collider = GetComponent<Collider>();

        _inventory = new Inventory();
        foreach(Item item in GameManager.Instance.ScenePersistent.Loadout)
        {
            AddItemToPlayer(item);
        }

#if UNITY_EDITOR
        // only add testing loadout of stuff wasnt picked from loadout scene
        if(GameManager.Instance.ScenePersistent.Loadout.Count == 0)
        {
            foreach (PassiveItem passiveItem in testItems)
                AddItemToPlayer(passiveItem);
            foreach (Weapon weapon in testWeapons)
            {
                AddItemToPlayer(weapon);
                GameManager.Instance.ScenePersistent.Loadout.Add(weapon);
            }
                
            
        }
#endif
        //AddItemToPlayer(defaultWeapon);

        //Can Be gotten rid of whenever
        //AddItemToPlayer(testWeapon2);

        StartCoroutine(EnableStartingWeaponVisual());
        
        if(GameManager.Instance.GamePersistent.IsSkipper)
        {
            Instantiate(skipper, new Vector3(0,0,0), Quaternion.identity);
        }
    }

    private void OnDisable()
    {
        controls.FindAction("FireWeapon").started -= FireWeapon;
        controls.FindAction("FireWeapon").canceled -= FireWeapon;
        controls.FindAction("SwitchWeapon").performed -= ChangeWeapon;

        controls.FindAction("FireWeapon").Disable();
        controls.FindAction("SwitchWeapon").Disable();
        foreach(Item item in _inventory.items)
        {
            if(item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                weapon.ResetStats();
            }
        }
    }

    


    void FireWeapon(InputAction.CallbackContext context)
    {
        if(_equippedWeapon != null && context.started)
        {
            FireFunctionality();
        }
        else if(_equippedWeapon != null && context.canceled)
        {
            CeaseFire();
        }
            
    }

    void ChangeWeapon(InputAction.CallbackContext context)
    {
        if (_weapons.Count < 2)
            return;
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
        if (controls.FindAction("FireWeapon").IsPressed())
        {
            _equippedWeapon.SetAim(weaponDirection);
            FireFunctionality();
        }

        WeaponSwitched?.Invoke(equippedWeaponindex);
            
        //Debug.Log(equippedWeaponindex);

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
        worldPos = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -cam.transform.position.z));
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

        // make sure invulnerability state ALWAYS matches game manager
        _assistInvulnerable = GameManager.Instance.GamePersistent.IsInvulnerable;
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
        set { 
                _speed = value;
                playerMovement.MaxSpeed = value;
            }
    }

    public int BaseHealth
    {
        get { return _baseHealth; }
        set { 
            _baseHealth = value;
            _health = value;
            }
    }

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    //the damage parameter can be ignored here its just how the interfacing works to make things easier
    public void TakeDamage(float damage, bool dontUseSound = false)
    {
        if (_invulnerable || _assistInvulnerable || BossDefeated || SceneManager.GetActiveScene().name == "6.5PracTut")
            return;
        Health -= 1;
        SoundManager.Instance.PlayOneShot(damageSound, gameObject.transform.position);
        HealthChanged?.Invoke(Health);
        
        if(Health <= 0)
        {
            if(!ZombieTime())
                playerDeath.Invoke();
        }
        else
        {
            _invulnerableWindow = StartCoroutine(InvulnerabilityWindow(1));
        }
       
    }

    public Transform GetWeaponsTransform()
    {
        return _weaponsTransform;
    }

    public Transform GetPassivesTransform()
    {
        return _passivesTransform;
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public ArenaMovement GetPlayerMovement()
    {
        return playerMovement;
    }

    public WeaponInstance GetWeaponInstance()
    {
        return _equippedWeapon;
    }
    /// <summary>
    /// The direction that the player is aiming. The vector from the player to the mouse.
    /// Direction vector is normalized.
    /// </summary>
    public Vector2 GetAimDirection()
    {
        return weaponDirection.normalized;
    }

    void ResetStats()
    {
        _health = _baseHealth;
        //_speed = _baseSpeed;
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
        if (_weaponsTransform.childCount < 1)
            yield break;
        yield return new WaitForSeconds(0.1f);
        foreach (WeaponInstance weaponInstance in _weaponsTransform.GetComponentsInChildren<WeaponInstance>())
        {
            _weapons.Add(weaponInstance);
        }
        _equippedWeapon = _weapons[0];
        _equippedWeapon.EnableRendering();
    }

    public IEnumerator InvulnerabilityWindow(float duration)
    {
        if (duration <= 0)
            yield break;
        _invulnerable = true;
        _collider.excludeLayers = _invulnerabilityMask;
        StartCoroutine(DamageFlash());
        yield return new WaitForSeconds(1);
        _collider.excludeLayers = 0;
        _invulnerable = false;
    }

    IEnumerator DamageFlash()
    {
        Color baseColor = _spriteRenderer.color;
        for(int i = 0; i < 8; i++)
        {
            baseColor.a = 0f;
            _spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(0.0625f);
            baseColor.a = 1f;
            _spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(0.0625f);
        }
    }

    public void PassEffect(EffectData effectData)
    {
        GetComponent<EffectManager>().PassEffect(effectData);
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        foreach(Weapon weapon in testWeapons)
        {
            weapon.ResetStats();
        }
    }
#endif
    


    #region Zombie Mode Code

    //Revive Functionality
    //TODO: Sprite change maybe?
    bool ZombieTime()
    {
        if (canRevive && !_hasRevived)
        {
            PlayerIsZombie?.Invoke(true);
            Health = BaseHealth;
            HealthChanged?.Invoke(Health);
            canRevive = false;
            _hasRevived = true;
            _invulnerableWindow = StartCoroutine(InvulnerabilityWindow(1));
            StartCoroutine(ZombieDeathTimer());
            return true;
        }
        return false;
    }

    IEnumerator ZombieDeathTimer()
    {
        DeadFishUIEvent?.Invoke();
        yield return new WaitForSeconds(30);
        playerDeath?.Invoke();
    }

    #endregion

    #region Invincible Dash Code

    public void InvincibleDash(float duration)
    {
        if(canInvincibleDash)
        {
            StartCoroutine(InvulnerabilityWindow(duration));
        }
    }

    #endregion

    #region Brickfish Code

    public void ActivateBrickfish()
    {
        playerMovement.dashRestricted = true;
    }

    #endregion

    #region Recoral Code

    public void ApplyRecoil(float amount)
    {
        rb.AddForce(-weaponDirection * amount, ForceMode.Impulse);
    }

    #endregion

}
