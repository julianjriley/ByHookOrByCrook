using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    [Header("Arena")]
    [SerializeField, Tooltip("The platform group spawner")]
    private GroupSpawner _gSpawner;
    [SerializeField, Tooltip("The camera")]
    private Boss3Camera _b3Cam;

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

    // Booleans for the major phase change
    private bool _inPhaseTwoPos; // True when the boss has entered position for phase 2
    private bool _phaseTwoChangeInProgress; // Blocker boolean for when the phase 2 change is happening
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
        yield return new WaitUntil(() => _inPhaseTwoPos == true);
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
        if(_phaseCounter >= 2)
        {
            if (_inPhaseTwoPos) // If the phase change has completed, resume normal attack patterns
            {
                AttackLogicMethod();
            }
            else if (!_phaseTwoChangeInProgress) // If the phase change needs to start, start it
            {
                StartCoroutine(DoMajorPhaseChange());
            }
            else
            {
                // If the phase change is in progress, nothing happens! We don't even need this else statement!
            }
        }
        else // Otherwise we can be normal
        {
            AttackLogicMethod();
        }
    }

    private void AttackLogicMethod() // Condensing most of the attack logic into a separate method to facilitate how this boss works
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

    private IEnumerator DoMajorPhaseChange()
    {
        // When we get to the second half of the fight...
        _phaseTwoChangeInProgress = true;

        // Trigger the spawn for the final platform
        _gSpawner.IsFinalSpawn = true;

        // Fly on up to this new point
        yield return new WaitForSeconds(.1f);
        SetNewTarget(_gSpawner.getFinalSection());
        SetSpeed(50f);

        // Wait for the camera to catch up
        yield return new WaitUntil(() => _b3Cam.GetFullStop());

        // And once that's done, resume normal attacking
        SetDefaultTarget();
        SetDefaultSpeed();

        _inPhaseTwoPos = true;
        yield return null;
    }


}
