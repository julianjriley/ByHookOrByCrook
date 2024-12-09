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
    [SerializeField, Tooltip("Used to pause repositioner motion")]
    private BossTargetRepositioner _repositioner;
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

    [Header("Absorbing Damage")]
    [SerializeField, Tooltip("VFX prefab for when the boss absorbs damage.")]
    private GameObject _absorbDamagePrefab;

    [HideInInspector]
    public bool IsInvincible;
    [HideInInspector]
    public bool IsTransformingInvincible; // whether the boss is invincible BECAUSE they are transforming

    // Booleans for the major phase change
    private bool _inPhaseTwoPos; // True when the boss has entered position for phase 2
    private bool _phaseTwoChangeInProgress; // Blocker boolean for when the phase 2 change is happening
    private bool _isPhaseTwoBegin = false; // becomes true as soon as the screen becomes fully white during anime transformation
    override protected void Start()
    {
        base.Start();
        _bossAnim = GetComponent<Animator>();
        _bwsAnim = _bigWhiteScreen.GetComponent<Animator>();
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

    override public void TakeDamage(float damage, bool dontUseSound)
    {
        if (IsInvincible)
        {
            // REMOVED: boss is invincible, but no longer heals
            /*
            // heal the boss
            BossHealth += damage;
            // don't heal over max phase health
            if (BossHealth > GetMajorPhaseThreshold())
                BossHealth = GetMajorPhaseThreshold();
            */

            // heart visual effect to show healing
            Instantiate(_absorbDamagePrefab, transform);

            // TODO: unique sound for boss absorbing damage?            
        }
        else
            base.TakeDamage(damage, dontUseSound);
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
        _repositioner.FreezeMotion();
        _wandAnim.SetTrigger("Wiggle");
        Instantiate(chosenAttack, _laserAttackEmpty);
        yield return new WaitForSeconds(_pauseTime);
        SetDefaultSpeed();
        _repositioner.UnfreezeMotion();
        yield return null;
    }

    private IEnumerator DoPhase1Spawn(GameObject chosenAttack)
    {
        SetSpeed(0);
        _repositioner.FreezeMotion();
        Instantiate(chosenAttack, _mikuAttackEmpty);
        yield return new WaitForSeconds(_pauseTime / 1.5f);
        _repositioner.UnfreezeMotion();
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

    override protected void PhaseSwitch()
    {
        // skip phase switch logic until AFTER transformation
        if (_phaseCounter == 3)
            return;

        base.PhaseSwitch();
    }

    private IEnumerator DoMajorPhaseChange()
    {
        // When we get to the second half of the fight...
        _phaseTwoChangeInProgress = true;

        // Trigger the spawn for the final platform
        _gSpawner.IsFinalSpawn = true;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.enabled = false; // prevent boss from running over player as soon as it shoots up

        // clear all enemy projectiles
        ScreenWipe();

        // Fly on up to this new point
        yield return new WaitForSeconds(.1f);
        SetNewTarget(_gSpawner.getFinalSection());
        SetSpeed(500f); // GET THE BOSS OUTTA HERE (looks like it teleports out during flash)

        // Stop the music
        SoundManager.Instance.musicEventInstance.setParameterByName("Phase1Over", 1);

        // Start Transformation Leadup
        leadupInstance = SoundManager.Instance.CreateInstance(transitionLeadup);
        leadupInstance.start();
        yield return new WaitForSeconds(3f);
        isLeadingUp = true;
        col.enabled = true; // re-enable collider so it can be shot again during phase 2

        SetSpeed(50f); // make sure the boss resumes stable bouncing at goal position

        // enable boss invincibility
        IsInvincible = true;
        IsTransformingInvincible = true; // prevents invincibility being overriden to OFF state

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

            _isPhaseTwoBegin = true; ; // this needs to be here for the boss health bar to update appropriate

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

        // handle officially starting attacks of new phase
        base.PhaseSwitch();

        // Allow boss to be hittable again
        IsInvincible = false;
        IsTransformingInvincible = false; // prevents invincibility being overriden to OFF state

        _inPhaseTwoPos = true; ; // this needs to be here for the boss health bar to update appropriate

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
    /// Becomes true when the boss health bar needs it to update visually.
    /// </summary>
    public bool IsInMajorPhaseTwo()
    {
        return _isPhaseTwoBegin;
    }

    /// <summary>
    /// Returns true if the boss is in major phase 2
    /// </summary>
    public bool CanBossUseInvincibility()
    {
        return _phaseCounter >= 3;
    }
    #endregion

    /// <summary>
    /// Returns the number phase that the boss is currently in.
    /// </summary>
    public int GetCurrPhase()
    {
        return _phaseCounter;
    }
}
