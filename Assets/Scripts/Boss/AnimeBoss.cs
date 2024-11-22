using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    

    [Header("Miku Attack")]
    [SerializeField, Tooltip("The empty that Miku spawns under")]
    private Transform _mikuAttackEmpty;

    [Header("Laser Attack")]
    [SerializeField, Tooltip("Spawn location for the lasers")]
    private Transform _laserAttackEmpty;
    [SerializeField, Tooltip("How long the laser attack lasts")]
    private float _laserBeamDuration = 10;
    [SerializeField, Tooltip("List of lasers")]
    private List<GameObject> laserList;

    private GameObject _laserbeamPrefab;
    private float currentTime = 0;
    private float _numberOflasers = 4;
    override protected void Start()
    {
        base.Start();
        //SetSpawnLocation();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        // Continues to give existing lasers a rotation
        UpdateLaserRotation();
        
    }

    private void SetSpawnLocation()
    {
        _spawnLocation = _laserAttackEmpty;
    }

    public override void SpawnAttackOnce(GameObject gameObj)
    {
        _laserbeamPrefab = gameObj;
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

        SetNewTarget(_laserAttackEmpty, 20f);
        
        // Actually spawn the lasers
        StartCoroutine(SpawnLaserBeam());
    }

    IEnumerator SpawnLaserBeam() 
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject laser = Instantiate(_laserbeamPrefab, _laserAttackEmpty);
            laserList.Add(laser);
            laser.transform.Rotate(0, 0, .7f); 
            yield return new WaitForSeconds(1.5f);
        }
    }

    override protected void AttackLogic()
    {
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten

        ChooseAttack(ref chosenAttack, _phaseCounter); // Pass in a reference to chosenAttack and the phase #

        if (chosenAttack.gameObject.CompareTag("Miku")) // Miku needs to spawn under a different parent
        {
            Instantiate(chosenAttack, _mikuAttackEmpty);
        }
        else if (chosenAttack.GetComponent<HeartAoE>())
        {
            // Stop the laserbeams if the attack is hearts next
            StopCoroutine(SpawnLaserBeam());
            StartCoroutine(HeartAttack(chosenAttack));
            Instantiate(chosenAttack, _spawnLocation);
        }
        else
        {
            Instantiate(chosenAttack, _spawnLocation);
        }
    }

    IEnumerator HeartAttack(GameObject heartPrefab)
    {
        SetNewTarget(_spawnLocation, 4f);
        yield return new WaitForSeconds(1);
    }
}
