using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleGroups;
    [SerializeField] private GameObject _finalGroup;
    [Tooltip("How long it takes before a new set of platforms is spawned (should be = to platGroup lifetime)")]
    [SerializeField] private float _spawnDelay = 15f; // How long it takes for a new set of platforms to spawn

    [Tooltip("Height of a single platform group (and how far up the next will be spawned from the last")]
    [SerializeField] private float _spawnHeightDistance;

    [SerializeField] private Boss3Camera _camera;

    private float _currentSpawnHeight = 17;
    private float _currentTimeToSpawn;

    public bool IsFinalSpawn;
    

    void Start()
    {
        Instantiate(selectPlatGroup(), new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Make the first set of platforms for a buffer
        _currentSpawnHeight += _spawnHeightDistance;

        _currentTimeToSpawn = _spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFinalSpawn)
        {
            _currentTimeToSpawn -= Time.deltaTime;
            if (_currentTimeToSpawn <= 0)
            {
                Instantiate(selectPlatGroup(), new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Create the platform
                _currentSpawnHeight += _spawnHeightDistance;    // Change where the next one will spawn

                _currentTimeToSpawn = _spawnDelay;
            }
        }
        else
        {
            GameObject lastPlat = Instantiate(_finalGroup, new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Create the last platform
            _camera.SetRestingPlace(lastPlat.transform.position.y + _spawnHeightDistance / 2);      // Send over the final y-value the camera needs to stop at 
            this.enabled = false; // Turn this off! No longer needed
        }
        

    }

    private GameObject selectPlatGroup()
    {
        int index = Random.Range(0, _possibleGroups.Count);
        return _possibleGroups[index];
    }
}
