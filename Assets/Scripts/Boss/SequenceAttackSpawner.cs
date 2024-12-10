using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for queuing up some combination of attacks based on timed intervals.
/// </summary>
public class SequenceAttackSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("List of attacks to spawn with each cycle tick of the spawner.")]
    private AttackTurn[] _attackGroupPrefabs;
    [SerializeField, Tooltip("Time between each prefab spawn from the list.")]
    private float _spawnInterval;

    private int _listIndex = 0;
    private AnimeBoss _boss;
    private int _currBossPhase;

    [System.Serializable]
    class AttackTurn
    {
        public GameObject[] AttackPrefabs;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnNext", 0, _spawnInterval);

        // Find object
        GameObject bossObj = GameObject.FindWithTag("Boss");
        if (bossObj is null)
            throw new System.Exception("Why is there nothing tagged with Boss in this combat scene?");

        // Find component
        if (!bossObj.TryGetComponent(out AnimeBoss bossComp))
            throw new System.Exception("Why is something tagged as Boss (in anime scene) but does not have Anime Boss component.");
        _boss = bossComp;

        _currBossPhase = _boss.GetCurrPhase();
    }

    private void Update()
    {
        // Stop spawning if next phase is entered
        if (_boss.GetCurrPhase() != _currBossPhase)
            Destroy(gameObject);
    }

    private void SpawnNext()
    {
        // All have been spawned, destroy me now
        if (_listIndex >= _attackGroupPrefabs.Length)
            Destroy(gameObject);
        // spawn if there is anything to spawn, otherwise skip it
        else if (_attackGroupPrefabs[_listIndex].AttackPrefabs.Length != 0)
        {
            // spawn it at the same attack container it would otherwise be at (the parent of this object)
            foreach (GameObject thingToSpawn in _attackGroupPrefabs[_listIndex].AttackPrefabs)
                Instantiate(thingToSpawn, transform.parent);
        }

        _listIndex++;
    }
}
