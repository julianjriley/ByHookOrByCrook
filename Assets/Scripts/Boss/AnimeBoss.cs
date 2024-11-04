using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeBoss : BossPrototype
{
    private float _laserBeamDuration = 10;
    [SerializeField]
    private Transform _pivotPoint;

    Transform _bossPosition;
    public Transform test;

    // Phase 2
    // TODO: Make boss static
    // TODO: Laser Beam
    // set target to player, then stop
    // TODO: Heart AoE
    //set target to player, then stop and have a damage radius when exploding
    // TODO: Can't spawn ink while in this phase

    override protected void Start()
    {
        base.Start();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();


    }

    public override void SpawnAttackOnce(GameObject gameObj)
    {
        base.SpawnAttackOnce(gameObj);

        StartLaserBeam();

    }
    public IEnumerator StartLaserBeam()
    {
        Debug.Log(test);
        SetNewTarget(test, _laserBeamDuration);

        yield return new WaitForSeconds(_laserBeamDuration);
    }

    override protected void AttackLogic()
    {
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        switch (_phaseCounter)
        {
            case 0:
                ChooseAttack(ref chosenAttack, 0); //pass in a reference to chosenAttack and the phase #
                break;
            case 1:
                ChooseAttack(ref chosenAttack, 1);
                Debug.Log("in attack logid " + test);
                SetNewTarget(test, _laserBeamDuration);
                break;
        }
        //Debug.Log("Instantiating: " + chosenAttack.name);
        Instantiate(chosenAttack, _spawnLocation);
        
    }
}
