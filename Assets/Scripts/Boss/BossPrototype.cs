using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Unity.Mathematics.math;

[RequireComponent(typeof(Rigidbody))]

public class BossPrototype : MonoBehaviour, IDamageable
{
    [Header ("Boss Movement")]
    protected Transform _target;
    private Transform _defaultTarget;
    protected Rigidbody _rb;
    public float Speed = 50f;
    private float _defaultSpeed;
    private bool _right = true;
    protected bool _checkingSwap = false;
    public Transform _playerTransform;
    private protected SpriteRenderer _renderer;

    [Header ("Boss Intro & Outro")]
    [SerializeField] private Transform _offscreenTarget;
    [SerializeField] private Transform _entranceTarget;
    [SerializeField] private GameObject _fightText;
    [SerializeField] private GameObject _introUI;
    [SerializeField] private GameObject _victoryText;
    [SerializeField] private GameObject _defeatText;
    [SerializeField, Tooltip("Used to call scene transitions.")]
    private SceneTransitionsHandler _transitionsHandler;

    private InputAction _skipIntroAction;
    private Coroutine _part1Intro;
    private GameObject _player;
    private InputActionAsset _actions;

    //private InputAction leftMouseClick;

    [Header ("Boss Phases + Attacks")]
    public float BossHealth;
    [HideInInspector]
    public float MaxBossHealth;
    protected int _phaseCounter = 0;
    private bool _defeated = false;
    protected Transform _spawnLocation;
    [SerializeField]
    protected PhaseInfo[] _phases;
    private int _lastChosenAttack = -1;

    [Header ("Boss SFX")]
    [SerializeField] EventReference damageSound;

    [Header("Scene Transitions")]
    [SerializeField, Tooltip("Scene name of cashout scene that follows boss fight.")]
    private string _cashoutSceneName;

    //For UI Update
    public delegate void HealthChange(float health);
    public event HealthChange HealthChanged;

    //For GameManager; Should be set to the next one
    [SerializeField] protected int _bossProgressionNumber = 0;
    
    void Awake() {
        _actions = InputSystem.actions;
    }
    // Start is called before the first frame update
    virtual protected void Start()
    {
        _introUI.SetActive(true);
        _rb = GetComponent<Rigidbody>();
        _spawnLocation = GameObject.Find("AttackHolderEmpty").GetComponent<Transform>();
        _target = GameObject.Find("Boss Target").GetComponent<Transform>();
        _defaultTarget = _target;
        _defaultSpeed = Speed;
        MaxBossHealth = BossHealth;

        //Debug.Log("Phase Counter = " + _phaseCounter);
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();

        _part1Intro = StartCoroutine(TitleCard());

        PlayerCombat.playerDeath += HandlePlayerDeath;

        gameObject.AddComponent<EffectManager>();
    }

    private void OnDisable()
    {
        PlayerCombat.playerDeath -= HandlePlayerDeath;
    }

    protected IEnumerator TitleCard() {
        yield return new WaitForSeconds(0.1f);

        _skipIntroAction = new InputAction("skipIntro", binding: "<Mouse>/leftButton");
        _skipIntroAction.AddBinding("<Mouse>/rightButton");
        _skipIntroAction.AddBinding("<Keyboard>/space");
        _skipIntroAction.AddBinding("<Keyboard>/w");
        _skipIntroAction.AddBinding("<Keyboard>/a");
        _skipIntroAction.AddBinding("<Keyboard>/s");
        _skipIntroAction.AddBinding("<Keyboard>/d");
        _skipIntroAction.Enable();
        _skipIntroAction.performed += EndTitleCard;

        _fightText.SetActive(false);

        //disable player combat and movement
        _player = _playerTransform.gameObject;
        _actions.Disable();

        SetNewTarget(_offscreenTarget, -1);

        yield return new WaitForSeconds(4f);

        EndTitleCard();
    }

    protected void EndTitleCard() {
        _skipIntroAction.performed -= EndTitleCard;
        _skipIntroAction.Disable();
        Destroy(_introUI);
        if (_part1Intro != null) {
            StopCoroutine(_part1Intro);
        }
        StartCoroutine(BossEntrance());
    }

    // this variant is used for input cancelling early
    protected void EndTitleCard(InputAction.CallbackContext context) {
        _skipIntroAction.performed -= EndTitleCard;
        _skipIntroAction.Disable();
        Destroy(_introUI);
        if (_part1Intro != null) {
            StopCoroutine(_part1Intro);
        }
        StartCoroutine(BossEntrance());
    }

    protected IEnumerator BossEntrance() {
        //change boss target
        SetNewTarget(_entranceTarget, 2f);
        //wait
        yield return new WaitForSeconds(2f);
        //enable fight! text
        _fightText.SetActive(true);
        //enable player combat and movement
        _actions.Enable();
        PhaseSwitch();
        SetDefaultTarget();
    }

    // Update is called once per frame
    virtual protected void FixedUpdate()
    {
        if (!_defeated) {
            Move();
            //check for phase change
            if (BossHealth <= 0) {
                DefeatLogic();
            } else if (_phaseCounter == _phases.Length - 1) { 
                return; //if it's the last phase, don't check ahead and avoid error
            } else if (BossHealth < _phases[_phaseCounter + 1].Threshold) {
                //Debug.Log("Initiating new phase");
                _phaseCounter++; //if under next phase Threshold meet, up the phase counter and switch phases
                PhaseSwitch();
            }
        } 
    }

    public virtual void Move() {
        if (_rb == null) {
            //Debug.Log("No rigidbody");
            return;
        }
        if (_target == null) {
            //Debug.Log("No target");
            return;
        }

        // makes the boss lean in the direction it's heading
        // remapped/clamped in such a way that it does not jitter when it has a low velocity but rather stabilizes around 0 degree rotation
        float rotationVal;
        if (_rb.velocity.x < 0)
        {
            rotationVal = remap(-13, -2, 20, 0, _rb.velocity.x);
            rotationVal = Mathf.Clamp(rotationVal, 0, 20);
        }
        else
        {
            rotationVal = remap(2, 13, 0, -20, _rb.velocity.x);
            rotationVal = Mathf.Clamp(rotationVal,-20, 0);
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotationVal); 
            
        //Debug.Log("Rigidbody velocity = " + _rb.velocity);
        _rb.AddForce((_target.position - transform.position).normalized * Speed, ForceMode.Force);
        if (!(_checkingSwap)) { //ensure only one check is happening at a time
            SpriteSwapCheck();
        } 
    }

    public void SetNewTarget(Transform newTarget, float duration = -1) {
        _target = newTarget;
        StopCoroutine("ChangeTargetBack"); //ensure two resets are not counting down concurrently
        if (duration == -1) { //-1 duration means set the target indefinitely, so avoid resetting the target to default below
            return;
        }
        StartCoroutine(ChangeTargetBack(duration));
    }
    public void SetSpeed(float newSpeed) {
        Speed = newSpeed;
    }

    /// <summary>
    /// Sets speed back to default (where it was at the start of the scene).
    /// </summary>
    protected void SetDefaultSpeed()
    {
        Speed = _defaultSpeed;
    }

    IEnumerator ChangeTargetBack(float duration) {
        yield return new WaitForSeconds(duration);
        SetDefaultTarget();
    }
    public void SetDefaultTarget() {
        _target = _defaultTarget;
    }

    protected void SpriteSwapCheck() {
        //Debug.Log("_right = " + _right);
        if (transform.position.x >= _playerTransform.position.x) { //if boss is on the right of player
            if (_right == false) { //and was just on the left
                StartCoroutine(SpriteSwapCheckTimer(_right)); 
            }
            _right = true;
        } else { 
            if (_right == true) { 
                StartCoroutine(SpriteSwapCheckTimer(_right)); 
            } 
            _right = false;
        }
    }

    IEnumerator SpriteSwapCheckTimer(bool right) {
        _checkingSwap = true;
        float rand = UnityEngine.Random.Range(0.4f, 2.1f);
        yield return new WaitForSeconds(rand);
        if (right == true) {
            if (transform.position.x < _playerTransform.position.x) { //honestly i can't even keep track anymore, this seems to work tho lmao
                _renderer.flipX = true;
            }
        } else {
            if (transform.position.x >= _playerTransform.position.x) { 
                _renderer.flipX = false;
            }
        }
        _checkingSwap = false;
    }

    virtual protected void AttackLogic() {
        //random choosing
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        ChooseAttack(ref chosenAttack, _phaseCounter);
        //Debug.Log("Instantiating: " + chosenAttack.name);
        Instantiate(chosenAttack, _spawnLocation);
    }

    public virtual void SpawnAttackOnce(GameObject gameObj) {
        Instantiate(gameObj, _spawnLocation);
    }

    protected void ChooseAttack(ref GameObject choice, int phaseNum) {
        int rand = UnityEngine.Random.Range(0, _phases[phaseNum].AttackPrefabs.Length); //special attack
        if (rand == _lastChosenAttack && _phases[phaseNum].AttackPrefabs.Length > 1) { //avoid choosing the same attack if there is more than 1 option
            ChooseAttack(ref choice, phaseNum);
            return;
        }
        _lastChosenAttack = rand;
        //Debug.Log("last chosen attack = " + _lastChosenAttack);
        choice = _phases[phaseNum].AttackPrefabs[rand];
    }

    void PhaseSwitch() {
        CancelInvoke();
        _phases[_phaseCounter].StartEvent.Invoke();
        InvokeRepeating("AttackLogic", _phases[_phaseCounter].StartDelay, _phases[_phaseCounter].RepeatRate);
        _lastChosenAttack = -1; //reset last chosen attack bc it's a new phase now and no attacks have been chosen
    }
    void DefeatLogic() {
        //Debug.Log("Defeated!");
        _defeated = true;
        CancelInvoke();
        foreach (Transform child in _spawnLocation) { //delete all attacks to ensure player doesn't die after defeating the boss
            Destroy(child.gameObject);
        }
        GameManager.Instance.GamePersistent.BossNumber = _bossProgressionNumber;
        transform.Find("SmokeExplosionVFX_0").gameObject.SetActive(true);
        _victoryText.SetActive(true);
        //_fadeOutPanel.SetActive(true);
        if(_bossProgressionNumber == 3)
        {
            GameManager.Instance.GamePersistent.BossNumber = 2;
            StartCoroutine(NextSceneDelay(true));
        }
        else
        {
            StartCoroutine(NextSceneDelay(false));
        }
        
    }

    void HandlePlayerDeath() {
        _actions.Disable();
        _defeatText.SetActive(true);
        _player.transform.Find("SmokeExplosionVFX_0").gameObject.SetActive(true);
        _player.GetComponent<PlayerCombat>().BossDefeated = true;
        StartCoroutine(NextSceneDelay(false));
    }

    IEnumerator NextSceneDelay(bool boss3Win) {
        yield return new WaitForSeconds(1.5f);

        if (boss3Win) {
            _transitionsHandler.LoadScene("8X_EndCutscene");
        }
        else
        {
            GoToCashout();
        }
        
    }

    virtual public void TakeDamage(float damage, bool dontUseSound)
    {
        BossHealth -= damage;
        HealthChanged?.Invoke(BossHealth);
        if(!dontUseSound)
            SoundManager.Instance.PlayOneShot(damageSound, gameObject.transform.position);
    }

    public void GoToCashout()
    {
        CalculateBossBountyMultiplier();

        _transitionsHandler.LoadScene(_cashoutSceneName);
    }

    protected void CalculateBossBountyMultiplier()
    {
        float percentageOfHealthLeft = BossHealth / MaxBossHealth;
        if(percentageOfHealthLeft <= 0)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 2;
        }
        else
        {
            double x = ((double)percentageOfHealthLeft);
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = (float)(math.remap(0, 1, 1.5, 1, x));
        }
        /*else if(percentageOfHealthLeft < 0.33f)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 4;
        }
        else if(percentageOfHealthLeft < 0.66f)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 2.5f;
        }*/
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == _playerTransform.gameObject)
        {
            PlayerCombat player = collider.gameObject.GetComponent<PlayerCombat>();
            player.TakeDamage(20000, false);
        }
    }

    public void PassEffect(EffectData effectData)
    {
        GetComponent<EffectManager>().PassEffect(effectData);
    }

}

[Serializable]
public class PhaseInfo {
    [SerializeField]
    public float Threshold;
    [SerializeField]
    public UnityEvent StartEvent;
    [SerializeField]
    public float StartDelay;
    [SerializeField]
    public float RepeatRate;
    [SerializeField]
    public GameObject[] AttackPrefabs;
}
