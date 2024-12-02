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

    [Header("Boss")]
    [SerializeField, Tooltip("The wand")]
    private GameObject _wand;
    [SerializeField, Tooltip("The wand animator")]
    private Animator _wandAnim;
    [SerializeField, Tooltip("The big white screen")]
    private GameObject _bigWhiteScreen;
    [SerializeField, Tooltip("Skips phase change cutscene for testing")]
    private bool _skipCutscene;
    private Animator _bossAnim;
    private Animator _bwsAnim;


    [Header("Miku Attack")]
    [SerializeField, Tooltip("The empty that Miku spawns under")]
    private Transform _mikuAttackEmpty;

    [Header("Laser Attack")]
    [SerializeField, Tooltip("A laser")]
    private GameObject _laserbeamPrefab;
    [SerializeField, Tooltip("Spawn location for the lasers")]
    private Transform _laserAttackEmpty;
    [SerializeField, Tooltip("How long the boss stops to cast lasers")]
    private float _pauseTime = .5f;
    private bool _laserActive; // Prevents other things from happening during laser time

    // Booleans for the major phase change
    private bool _inPhaseTwoPos; // True when the boss has entered position for phase 2
    private bool _phaseTwoChangeInProgress; // Blocker boolean for when the phase 2 change is happening
    override protected void Start()
    {
        base.Start();
        _bossAnim = GetComponent<Animator>();
        _bwsAnim = _bigWhiteScreen.GetComponent<Animator>();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (_phaseCounter >= 2)

            if (!_phaseTwoChangeInProgress) // If the phase change needs to start, start it
            {
                StartCoroutine(DoMajorPhaseChange());
            }

        }
    #region LASER BEAM METHODS

    public override void SpawnAttackOnce(GameObject gameObj) // This has just been converted into the "do the laser method"
    {
        //_laserbeamPrefab = gameObj;
        InvokeRepeating("StartLaserBeam", 1f, 30f); // Ahh, so this begins the loop of doing lasers every 30 sec
    }

    private void StartLaserBeam()
    {
        SetNewTarget(_laserAttackEmpty, 20f);

        // Actually spawn the lasers
        
        StartCoroutine(SpawnLaserBeam());
    }

    private IEnumerator SpawnLaserBeam() 
    {
        yield return new WaitUntil(() => _inPhaseTwoPos == true);
        for (int i = 0; i < 4; i++)
        {
            GameObject laser = Instantiate(_laserbeamPrefab, _laserAttackEmpty);
            yield return new WaitForSeconds(1.5f);
        }
    }
    #endregion

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
        else if (chosenAttack.gameObject.CompareTag("Laser")) // Special laser casting conditions
        {
            DoCastLaser(chosenAttack);
        }
        else
        {
            Instantiate(chosenAttack, _spawnLocation);
        }
    }

    private IEnumerator DoMajorPhaseChange()
    {
        // When we get to the second half of the fight...
        _phaseTwoChangeInProgress = true;

        // Trigger the spawn for the final platform
        _gSpawner.IsFinalSpawn = true;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.enabled = false;

        // Fly on up to this new point
        yield return new WaitForSeconds(.1f);
        SetNewTarget(_gSpawner.getFinalSection());
        SetSpeed(50f);

        // Wait for the camera to catch up
        yield return new WaitUntil(() => _b3Cam.GetFullStop());

        if (!_skipCutscene)
        {
            // Play the transformation sequence
            _bossAnim.Play("Transfoooorm", 0, 0);
            _bigWhiteScreen.SetActive(true);
            _bwsAnim.Play("FadeWhite", 0, 0);
            yield return new WaitForSeconds(6.67f);
            _wand.SetActive(true);
            _bwsAnim.Play("FadeClear", 0, 0);
            _bossAnim.Play("Idle", 0, 0);
            _bossAnim.speed = 0;
            yield return new WaitForSeconds(3f);
        }
        else
        {
            _wand.SetActive(true);
        }
        
        _bossAnim.speed = 1;

        // And once that's done, resume normal attacking
        SetDefaultTarget();
        SetDefaultSpeed();
        col.enabled = true;

        _inPhaseTwoPos = true;
        yield return null;
    }

    private IEnumerator DoCastLaser(GameObject chosenAttack)
    {
        SetSpeed(0);
        _wandAnim.speed = 1;
        Instantiate(chosenAttack, _laserAttackEmpty);
        yield return new WaitForSeconds(_pauseTime);
        _wandAnim.speed = 0;
        SetDefaultSpeed();
        yield return null;
    }


}
