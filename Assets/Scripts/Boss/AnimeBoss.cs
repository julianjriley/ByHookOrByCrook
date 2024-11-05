using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    [SerializeField]
    private float _laserBeamDuration = 10;
    [SerializeField]
    private Transform _pivotPoint;
    [SerializeField]
    private Transform _newSpawnLocation;

    private GameObject laserbeamPrefab;

    // Phase 2
    // TODO: Make boss static
    // TODO: Laser Beam - part of Start Event() 
    // set target to player, then stop
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
        InvokeRepeating("StartLaserBeam", 0, _laserBeamDuration);
       
    }
    private void StartLaserBeam()
    {
        // need to make it spawn in the middle with set new target function
        Debug.Log("setting new location ");
        SetNewTarget(_spawnLocation, _laserBeamDuration);
        SpawnLaserBeam();
    }

    private void SpawnLaserBeam()
    {
        Instantiate(laserbeamPrefab, _spawnLocation);
    }
    override protected void AttackLogic()
    {
        // for instantiating attacks separate from the boss (like projectiles)

        base.AttackLogic();
    }
}
