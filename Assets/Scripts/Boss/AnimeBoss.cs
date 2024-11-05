using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    [SerializeField]
    private float _laserBeamDuration = 10;
    [SerializeField]
    private Transform _newSpawnLocation;

    private GameObject laserbeamPrefab;

    // Phase 2
    // TODO: Make boss static
    // TODO: Laser Beam - part of Start Event() 
    // set target to middle, then laser spin
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
    private void StartLaserBeam()
    {
        // need to make it spawn in the middle with set new target function

        // TODO: fix how boss doesn't move to point
        // TODO: fix how the laserbeam is spawning
        Debug.Log("Setting target at location " + _spawnLocation);
        SetNewTarget(_spawnLocation, -1);
        new WaitForSeconds(10);
        SpawnLaserBeam();
    }

    private void SpawnLaserBeam()
    {
        Debug.Log("Spawning laser beam at " + _spawnLocation);
        

        for (int i = 0; i < 5; i++)
        {
            GameObject laser = Instantiate(laserbeamPrefab, _spawnLocation);
            new WaitForSeconds(1);
            laser.GetComponent<Transform>().Rotate(0,0, 10*i);
        }
        new WaitForSeconds(15f);

        //SetDefaultTarget();
    }
    override protected void AttackLogic()
    {
        // for instantiating attacks separate from the boss (like projectiles)

        base.AttackLogic();
    }
}
