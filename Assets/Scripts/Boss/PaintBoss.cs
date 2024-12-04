using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles unique behavior of painterly boss.
/// Overrides movement inputs to favor targetting 'glowing' paintings, if any exist (by latest phase this is likely most of the boss movement).
/// </summary>
public class PaintBoss : BossPrototype
{
    [Header("Painterly Boss Parameters")]
    [SerializeField, Tooltip("Distance from spawner at which the boss will enter 'painting' mode.")]
    private float _paintDistance;
    [SerializeField, Tooltip("Speed that boss moves towards a painting.")]
    private float _paintingTrackSpeed;
    [SerializeField, Tooltip("Duration of 'painting' phase while boss spawns 'enemy'.")]
    private float _paintDuration;
    [SerializeField, Tooltip("Target used to make the boss track to a painting to spawn an 'enemy'.")]
    private CirclingTarget _paintingTarget;
    [SerializeField] EventReference paintingSound;

    // queue of spawners the boss must track to
    private List<PainterlySpawner> _preparingSpawners = new();
    private List<PainterlySpawner> _spawners = new();
    private bool _isPainting;

    override protected void FixedUpdate()
    {
        // move, defeat, and phase switch logic.
        base.FixedUpdate();

        // don't handle target tracking behavior if currently in the process of painting already.
        if (_isPainting)
            return;

        // check for adding preparing spawners to spawners list once they are ready
        for (int i = _preparingSpawners.Count - 1; i >= 0; i--)
        {
            if(_preparingSpawners[i].isReady)
            {
                _spawners.Add(_preparingSpawners[i]);
                _preparingSpawners.RemoveAt(i);
            }
        }

        // track to first spawner in list (like queue)
        if (_spawners.Count > 0 && !_isPainting)
        {
            // update where boss tracks towards
            if (_target != _paintingTarget.transform)
            {
                _paintingTarget.SetNewCenter(_spawners[0].transform.position.x, _spawners[0].transform.position.y);
                SetNewTarget(_paintingTarget.transform);
                SetSpeed(_paintingTrackSpeed);
            }
            
            // Check for in range to start 'painting' enemy
            Vector2 bossPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 targetPos = new Vector2(_target.position.x, _target.position.y);
            if(Vector2.Distance(bossPos, targetPos) < _paintDistance)
            {
                _isPainting = true;
                StartCoroutine(MakeSomeArt());
            }
        }
    }

    /// <summary>
    /// Handles process of boss entering painting phase and projectile spawning before boss returns to normal behavior.
    /// </summary>
    private IEnumerator MakeSomeArt()
    {
        // start paint effect on spawner
        _spawners[0].StartPaint();

        // painting sound
        SoundManager.Instance.PlayOneShot(paintingSound, gameObject.transform.position);
        // TODO: painting animation

        // paint delay
        yield return new WaitForSeconds(_paintDuration);


        // convert paint to 'enemy'
        _spawners[0].Spawn();
        // delete spawner from list
        _spawners.RemoveAt(0);

        // return to non-painting movement behavior
        SetDefaultTarget();
        SetDefaultSpeed();

        // TODO: return to idle animation

        _isPainting = false;
    }

    // overriden so that instantiated spawner can be tracked AFTER instantiation
    override protected void AttackLogic()
    {
        //random choosing
        GameObject chosenAttack = _phases[0].AttackPrefabs[0]; //default that will be overwritten
        ChooseAttack(ref chosenAttack, _phaseCounter);

        // create attack
        GameObject attack = Instantiate(chosenAttack, _spawnLocation);

        // Add object to spawners list ONLY if it is a PainterlySpawner (i.e. ignore ink portals)
        if (attack.TryGetComponent(out PainterlySpawner spawner))
            _preparingSpawners.Add(spawner);
    }

    /// <summary>
    /// Returns transform of the paint boss's next painting target.
    /// </summary>
    public Transform GetNextPaintingTransform()
    {
        // no next painting yet
        if (_spawners.Count == 0)
            return null;

        return _spawners[0].transform;
    }
}