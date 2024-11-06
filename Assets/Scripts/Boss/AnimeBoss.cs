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

        UpdateLaserRotation();
    }

    private void SetSpawnLocation()
    {
        _spawnLocation = _newSpawnLocation;
    }

    public override void SpawnAttackOnce(GameObject gameObj)
    {
        laserbeamPrefab = gameObj;
        InvokeRepeating("StartLaserBeam", 1f, 25f);
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
                laser.transform.Rotate(0, 0, 1f);
            }
        }

    }
    private void StartLaserBeam()
    {
    
        // TODO: fix how the laserbeam is spawning
        Debug.Log("Setting target at location " + _spawnLocation);
        SetNewTarget(_spawnLocation, 13f);
        
        StartCoroutine(SpawnLaserBeam());
    }

    IEnumerator SpawnLaserBeam()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject laser = Instantiate(laserbeamPrefab, _spawnLocation);
            laserList.Add(laser);
            laser.transform.Rotate(0, 0, .5f); 
            yield return new WaitForSeconds(4f);
        }
       
    }

    override protected void AttackLogic()
    {
        // for instantiating attacks separate from the boss (like projectiles)

        base.AttackLogic();
    }
}
