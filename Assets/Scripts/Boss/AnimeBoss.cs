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

    private float currentTime = 0;
    private float _numberOflasers = 4;

    [SerializeField]
    private List<GameObject> laserList;

    override protected void Start()
    {
        base.Start();
        SetSpawnLocation();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        // Continues to give existing lasers a rotation
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
    
        SetNewTarget(_spawnLocation, 20f);
        
        // Actually spawn the lasers
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
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        switch (_phaseCounter)
        {
            case 0:
                ChooseAttack(ref chosenAttack, 0); //pass in a reference to chosenAttack and the phase #
                Instantiate(chosenAttack, _spawnLocation);
                break;
            case 1:
                ChooseAttack(ref chosenAttack, 1);
                if (chosenAttack.GetComponent<HeartAoE>())
                {
                    // Stop the laserbeams if the attack is hearts next
                    StopCoroutine(SpawnLaserBeam());
                    StartCoroutine(HeartAttack(chosenAttack));
                    Instantiate(chosenAttack, _spawnLocation);
                }
                else Instantiate(chosenAttack, _spawnLocation);
                break;
        }
    }

    IEnumerator HeartAttack(GameObject heartPrefab)
    {
        SetNewTarget(_spawnLocation, 4f);
        yield return new WaitForSeconds(1);
    }
}
