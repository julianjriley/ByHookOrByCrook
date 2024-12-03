using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles delay between portal indicating projectile about to spawn and then actually instantiating projectile.
/// </summary>
public class BossProjectileSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Time it takes for this projectile to spawn.")]
    private float _spawnTime;
    [SerializeField, Tooltip("Projectile prefab to spawn.")]
    private GameObject _projectilePrefab;

    private float _spawnTimer;

    private void Awake()
    {
        // start timer once initialized
        _spawnTimer = _spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        // ready to spawn
        if (_spawnTimer < 0)
        {
                // spawn projectile at rift position/rotation
                Instantiate(_projectilePrefab, transform.position, transform.rotation);

            

            // TODO: integrate this better with animations / fade out / etc.
            Destroy(gameObject);
        }
        // not read to spawn
        else
            _spawnTimer -= Time.deltaTime;
    }
}
