using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class BossPrototype : MonoBehaviour
{
    [Header ("Boss Movement")]
    private Transform _target;
    private Rigidbody _rb;
    public float Speed = 50f;
    private bool _right = true;
    private bool _checkingSwap = false;
    private Transform _playerTransform;
    private SpriteRenderer _renderer;

    [Header ("Boss Phases + Attacks")]
    public float BossHealth;
    public float MaxBossHealth;
    private int _phaseCounter = 0;
    private bool _defeated = false;
    private Transform _spawnLocation;
    [SerializeField]
    private PhaseInfo[] _phases;
    private int _lastChosenAttack = -1;

    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _spawnLocation = GameObject.Find("AttackHolderEmpty").GetComponent<Transform>();
        _target = GameObject.Find("Boss Target").GetComponent<Transform>();
        PhaseSwitch();
        Debug.Log("Phase Counter = " + _phaseCounter);
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();

        PlayerCombat.playerDeath += GoToCashout;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_defeated) {
            Move();
            //check for phase change
            if (BossHealth <= 0) {
                DefeatLogic();
            } else if (_phaseCounter == _phases.Length - 1) { 
                return; //if it's the last phase, don't check ahead and avoid error
            } else if (BossHealth < _phases[_phaseCounter + 1].Threshold) {
                Debug.Log("Initiating new phase");
                _phaseCounter++; //if under next phase Threshold meet, up the phase counter and switch phases
                PhaseSwitch();
            }
        } 
    }

    void Move() {
        _rb.AddForce((_target.position - transform.position).normalized * Speed, ForceMode.Force);
        if (!(_checkingSwap)) { //ensure only one check is happening at a time
            SpriteSwapCheck();
        } 
    }

    void SpriteSwapCheck() {
        Debug.Log("_right = " + _right);
        if (transform.position.x >= _playerTransform.position.x) { //if boss is on the right of player
            if (_right == false) { //and was just on the left
                StartCoroutine(SpriteSwapCheckTimer(_right)); 
            }// else {
                _right = true;
            //}
        } else { //if boss is on the left
            if (_right == true) { //and was just on the right
                StartCoroutine(SpriteSwapCheckTimer(_right)); 
            } //else {
                _right = false;
            //}
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

    void AttackLogic() {
        //random choosing
        Debug.Log("phasecounter = " + _phaseCounter);
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        switch (_phaseCounter) {
            case 0:
                ChooseAttack(ref chosenAttack, 0); //pass in a reference to chosenAttack and the phase #
            break;
            case 1:
                ChooseAttack(ref chosenAttack, 1);
            break;
        }
        Debug.Log("Instantiating: " + chosenAttack.name);
        Instantiate(chosenAttack, _spawnLocation);
    }

    public void SpawnAttackOnce(GameObject gameObj) {
        Instantiate(gameObj, _spawnLocation);
    }

    void ChooseAttack(ref GameObject choice, int phaseNum) {
        int rand = UnityEngine.Random.Range(0, _phases[phaseNum].AttackPrefabs.Length); //special attack
        if (rand == _lastChosenAttack && _phases[phaseNum].AttackPrefabs.Length > 1) { //avoid choosing the same attack if there is more than 1 option
            ChooseAttack(ref choice, phaseNum);
            return;
        }
        _lastChosenAttack = rand;
        Debug.Log("last chosen attack = " + _lastChosenAttack);
        choice = _phases[phaseNum].AttackPrefabs[rand];
    }

    void PhaseSwitch() {
        CancelInvoke();
        _phases[_phaseCounter].StartEvent.Invoke();
        InvokeRepeating("AttackLogic", _phases[_phaseCounter].StartDelay, _phases[_phaseCounter].RepeatRate);
        _lastChosenAttack = -1; //reset last chosen attack bc it's a new phase now and no attacks have been chosen
    }
    void DefeatLogic() {
        Debug.Log("Defeated!");
        _defeated = true;
        CancelInvoke();
        foreach (Transform child in _spawnLocation) { //delete all attacks to ensure player doesn't die after defeating the boss
            Destroy(child.gameObject);
        }
        CalculateBossBountyMultiplier();
        GoToCashout();
    }

    public void TakeDamage(float damage)
    {
        BossHealth -= damage;
    }

    //ONLY FOR THE PROTOTYPE
    public void GoToCashout()
    {
        
        SceneManager.LoadScene("PROTO_Cashout");
    }

    protected void CalculateBossBountyMultiplier()
    {
        float percentageOfHealthLeft = BossHealth / MaxBossHealth;
        if(percentageOfHealthLeft <= 0)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 6;
        }
        else if(percentageOfHealthLeft < 0.33f)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 4;
        }
        else if(percentageOfHealthLeft < 0.66f)
        {
            GameManager.Instance.ScenePersistent.BossPerformanceMultiplier = 2.5f;
        }
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
