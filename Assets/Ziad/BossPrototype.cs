using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]

public class BossPrototype : MonoBehaviour
{
    [Header ("Boss Movement")]
    private Transform target;
    private Rigidbody rb;
    public float Speed = 50f;

    [Header ("Boss Phases + Attacks")]
    public float BossHealth;
    private int _phaseCounter = 0;
    private bool _defeated = false;
    private Transform spawnLocation;
    [SerializeField]
    private PhaseInfo[] _phases;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnLocation = GameObject.Find("AttackHolderEmpty").GetComponent<Transform>();
        target = GameObject.Find("Boss Target").GetComponent<Transform>();
        PhaseSwitch();
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
                _phaseCounter++; //if under next phase Threshold meet, up the phase counter and switch phases
                PhaseSwitch();
            }
        } 
    }

    void Move() {
        Debug.Log("Target position: " + target.position);
        Debug.Log("Current position: " + transform.position);
        rb.AddForce((target.position - transform.position).normalized * Speed, ForceMode.Force);
    }

    void AttackLogic() {
        //add random choosing later that avoids repeats
        //check what phase
        //instantiate attack in [0] place
        GameObject chosenAttack = _phases[0].AttackPrefabs[0];
        switch (_phaseCounter) {
            case 0:
            chosenAttack = _phases[0].AttackPrefabs[0];
            break;
            case 1:
            chosenAttack = _phases[0].AttackPrefabs[0];
            break;
        }
        Instantiate(chosenAttack, spawnLocation);
    }
    void PhaseSwitch() {
        CancelInvoke();
        InvokeRepeating("AttackLogic", _phases[_phaseCounter].StartDelay, _phases[_phaseCounter].RepeatRate);
    }
    void DefeatLogic() {
        Debug.Log("Defeated!");
        _defeated = true;
        CancelInvoke();
        foreach (Transform child in spawnLocation) {
            Destroy(child.gameObject);
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
