using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossPrototype : MonoBehaviour
{
    public float BossHealth;
    private int _phaseCounter = 0;
    private bool _defeated = false;
    private Transform spawnLocation;
    [SerializeField]
    private PhaseInfo[] _phases;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = GameObject.Find("AttackHolderEmpty").GetComponent<Transform>();
        //currentEvent = Phases[0];
        PhaseSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_defeated) {
            if (BossHealth <= 0) {
                DefeatLogic();
            } else if (_phaseCounter == _phases.Length - 1) { 
                return; //if it's the last phase, don't check ahead and avoid error
            } else if (BossHealth < _phases[_phaseCounter + 1].threshold) {
                _phaseCounter++; //if under next phase threshold meet, up the phase counter and switch phases
                PhaseSwitch();
            }
        } 
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
    public float threshold;
    [SerializeField]
    public float StartDelay;
    [SerializeField]
    public float RepeatRate;
    [SerializeField]
    public GameObject[] AttackPrefabs;
}
