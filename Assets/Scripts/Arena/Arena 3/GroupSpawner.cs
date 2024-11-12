using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleGroups;
    [SerializeField] private GameObject _finalGroup;

    [Tooltip("Height of a single platform group (and how far up the next will be spawned from the last")]
    [SerializeField] private float _spawnHeightDistance;

    [SerializeField] private Boss3Camera _camera;
    public bool Activate = false; // Toggling this boolean starts everything (plat gen and camera movement)

    private float _currentSpawnHeight = 17;


    public bool IsFinalSpawn; // Toggling this boolean halts generation after spawning one final configuration

    private float _lastCamPosY;
    

    void Start()
    {
        Instantiate(selectPlatGroup(), new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Make the first and second set of platforms for a buffer
        _currentSpawnHeight += _spawnHeightDistance;
        Instantiate(selectPlatGroup(), new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); 
        _currentSpawnHeight += _spawnHeightDistance;

        _lastCamPosY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            _camera.SetCamMoving();
            if (!IsFinalSpawn)
            {

                if (_camera.transform.position.y > _spawnHeightDistance +_lastCamPosY)
                {
                    Instantiate(selectPlatGroup(), new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Create the platform
                    _currentSpawnHeight += _spawnHeightDistance;    // Change where the next one will spawn

                    _lastCamPosY += _spawnHeightDistance;
                }
            }
            else
            {
                GameObject lastPlat = Instantiate(_finalGroup, new Vector3(0, _currentSpawnHeight, 0), Quaternion.identity); // Create the last platform
                _camera.SetRestingPlace(lastPlat.transform.position.y + _spawnHeightDistance / 2);      // Send over the final y-value the camera needs to stop at 
                this.enabled = false; // Turn this off! No longer needed
            }
        }
        
        

    }

    private GameObject selectPlatGroup()
    {
        int index = Random.Range(0, _possibleGroups.Count);
        return _possibleGroups[index];
    }
}
