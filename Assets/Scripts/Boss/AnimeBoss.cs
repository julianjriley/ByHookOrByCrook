using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    [SerializeField]
    private float _laserBeamDuration = 10;
    [SerializeField]
    private Transform _newSpawnLocation;

    private GameObject laserbeamPrefab;

    private float currentTime = 0;
    private float _numberOflasers = 4;

    [SerializeField]
    private List<GameObject> laserList;
    // Phase 2
    // TODO: MAKE BOSS STATIC while A o
    // TODO: Heart AoE - part of Attack prefabs
    //set target to player, then stop and have a damage radius when exploding
    // TODO: Can't spawn ink while in this phase

    override protected void Start()
    {
        base.Start();
        SetSpawnLocation();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        UpdateLaserRotation();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void SetSpawnLocation()
    {
        _spawnLocation = _newSpawnLocation;
    }

    public override void SpawnAttackOnce(GameObject gameObj)
    {
        laserbeamPrefab = gameObj;
        InvokeRepeating("StartLaserBeam", 1f, 30f);
    }

    private void UpdateLaserRotation()
    {
        if (laserList.Count == 0)
        {
            return;
        }
        foreach (GameObject laser in laserList)
        {
            if (laser)
            {
                laser.transform.Rotate(0, 0, .7f);
            }
        }

    }
    private void StartLaserBeam()
    {
    
        // TODO: fix how the laserbeam is spawning
        Debug.Log("Setting target at location " + _spawnLocation);
        SetNewTarget(_spawnLocation, 20f);
        
        StartCoroutine(SpawnLaserBeam());
    }

    IEnumerator SpawnLaserBeam() 
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject laser = Instantiate(laserbeamPrefab, _spawnLocation);
            laserList.Add(laser);
            laser.transform.Rotate(0, 0, .7f); 
            yield return new WaitForSeconds(1.5f);
        }
       
    }

    override protected void AttackLogic()
    {
        // for instantiating attacks separate from the boss (like projectiles)

        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        switch (_phaseCounter)
        {
            case 0:
                ChooseAttack(ref chosenAttack, 0); //pass in a reference to chosenAttack and the phase #
                if (chosenAttack.GetComponent<HeartAoE>()) StartCoroutine(HeartAttack(chosenAttack));
                else Instantiate(chosenAttack, _spawnLocation);
                break;
            case 1:
                ChooseAttack(ref chosenAttack, 1); // spawning hearts
                if (chosenAttack.GetComponent<HeartAoE>()) StartCoroutine(HeartAttack(chosenAttack));
                else Instantiate(chosenAttack, _spawnLocation);
                break;
        }
    }
    void SpawnHearts(GameObject heartPrefab)
    {
        // When spawned, spawn multiple in a line based on player's position
        // if player is on left side, spawn 3 to the right
        // if player is on right side, spawn 3 to the left
       

        if (_playerTransform.position.x < Screen.width / 2)
        {
            // SetNewTarget() // Left side
            GameObject newHearts;
            newHearts = Instantiate(heartPrefab, GameObject.Find("AttackHolderEmpty").GetComponent<Transform>());

            newHearts.transform.position = new(newHearts.transform.position.x + 10, _playerTransform.position.y, newHearts.transform.position.z);

        }
        else
        {
            // SetNewTarget() // Right side
            GameObject newHearts;
            newHearts = Instantiate(heartPrefab, GameObject.Find("AttackHolderEmpty").GetComponent<Transform>());

            newHearts.transform.position = new(newHearts.transform.position.x - 10, _playerTransform.position.y, newHearts.transform.position.z);
        }

    }
    IEnumerator HeartAttack(GameObject heartPrefab)
    {
        SetNewTarget(_spawnLocation, 6f);
        Debug.Log("Starting Heart Attack");
        SpawnHearts(heartPrefab);
        yield return new WaitForSeconds(1);
    }
}
