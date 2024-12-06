using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
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

    [Header("SFX")]
    [SerializeField] private EventReference transitionLeadup;
    [SerializeField] private EventReference anime2Track;
    private EventInstance leadupInstance;
    private EventInstance music;
    private bool isLeadingUp;
    private float leadupDistance;

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

    // Booleans for the major phase change
    private bool _inPhaseTwoPos; // True when the boss has entered position for phase 2
    private bool _phaseTwoChangeInProgress; // Blocker boolean for when the phase 2 change is happening
    override protected void Start()
    {
        base.Start();
        _bossAnim = GetComponent<Animator>();
        _bwsAnim = _bigWhiteScreen.GetComponent<Animator>();
        _wandAnim.speed = 0;
        //music = SoundManager.Instance.musicEventInstance;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (_phaseCounter >= 3)
        {
            if (!_phaseTwoChangeInProgress) // If the phase change needs to start, start it
            {
                StartCoroutine(DoMajorPhaseChange());
            }
            if(isLeadingUp)
            {
                leadupDistance = gameObject.transform.position.y - _playerTransform.position.y;
                if (leadupDistance < 40)
                {
                    SoundManager.Instance.SetParameter(leadupInstance, "Distance", leadupDistance / 4);
                }
            }
        }
        music.getParameterByName("Phase1Over", out float volume);
    }

    #region Attack Logic
    override protected void AttackLogic()
    {
        if(_phaseCounter >= 3)
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

        if (chosenAttack.gameObject.CompareTag("Miku") || chosenAttack.gameObject.CompareTag("LoveLetter")) // Miku needs to spawn under a different parent
        {
            StartCoroutine(DoPhase1Spawn(chosenAttack)); // this function does the pause and wand wiggle
        }
        else if (chosenAttack.gameObject.CompareTag("AnimeInk"))
        {
            Instantiate(chosenAttack, _laserAttackEmpty);
        }
        else
        {
            StartCoroutine(DoCastLaser(chosenAttack)); // this function does the pause and wand wiggle
        }
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

    private IEnumerator DoPhase1Spawn(GameObject chosenAttack)
    {
        SetSpeed(0);
        Instantiate(chosenAttack, _mikuAttackEmpty);
        yield return new WaitForSeconds(_pauseTime / 1.5f);
        SetDefaultSpeed();
        yield return null;
    }
    #endregion

    #region MAJOR PHASE CHANGE
    [Header("Major Phase Change")]
    [SerializeField, Tooltip("Visual effect accompanying phase 1 end screen wipe")]
    private GameObject _flashEffect;
    [SerializeField, Tooltip("Collision layers used to destroy all enemy projectiles.")]
    private LayerMask _enemyProjectileMask;

    private IEnumerator DoMajorPhaseChange()
    {
        // When we get to the second half of the fight...
        _phaseTwoChangeInProgress = true;

        // Trigger the spawn for the final platform
        _gSpawner.IsFinalSpawn = true;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.enabled = false;

        // clear all enemy projectiles
        ScreenWipe();

        // Fly on up to this new point
        yield return new WaitForSeconds(.1f);
        SetNewTarget(_gSpawner.getFinalSection());
        SetSpeed(50f);

        // Stop the music
        SoundManager.Instance.musicEventInstance.setParameterByName("Phase1Over", 1);

        // Start Transformation Leadup
        leadupInstance = SoundManager.Instance.CreateInstance(transitionLeadup);
        leadupInstance.start();
        yield return new WaitForSeconds(3f);
        isLeadingUp = true;

        // Wait for the camera to catch up
        yield return new WaitUntil(() => _b3Cam.GetFullStop());
        SoundManager.Instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SoundManager.Instance.musicEventInstance.release();
        leadupInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        leadupInstance.release();

        if (!_skipCutscene)
        {
            // Play the transformation sequence
            _bossAnim.Play("Transfoooorm", 0, 0);
            SoundManager.Instance.InitializeMusic(anime2Track);
            _bigWhiteScreen.SetActive(true);
            _bwsAnim.Play("FadeWhite", 0, 0);
            yield return new WaitForSeconds(6.67f);
            _wand.SetActive(true);

            _inPhaseTwoPos = true; ; // this needs to be here for the boss health bar to update appropriate

            _bwsAnim.Play("FadeClear", 0, 0);
            //_bossAnim.Play("Idle", 0, 0);
            _bossAnim.Play("Idle2",0,0);
            _bossAnim.speed = 0;
            yield return new WaitForSeconds(3f);
            _bigWhiteScreen.SetActive(false);
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

        yield return null;
    }

    /// <summary>
    /// Wipes the screen of hazards at the end of major phase 1
    /// </summary>
    void ScreenWipe()
    {
        // visual effect
        GameObject flashEffect = Instantiate(_flashEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(flashEffect, 2f);

        // actual projectile clearing
        Collider[] colliders;
        colliders = Physics.OverlapSphere(gameObject.transform.position, 60, _enemyProjectileMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// Returns health threshold at which first major boss phase ends.
    /// </summary>
    public float GetMajorPhaseThreshold()
    {
        return _phases[3].Threshold;
    }

    /// <summary>
    /// Returns true if the boss has completed phase 1.
    /// Does NOT return true if phase change is currently in progress.
    /// </summary>
    public bool IsInMajorPhaseTwo()
    {
        return _inPhaseTwoPos;
    }
    #endregion
}
