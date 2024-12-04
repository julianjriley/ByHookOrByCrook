using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles delay between portal indicating projectile about to spawn and then actually instantiating projectile.
/// </summary>
public class CannonProjectileSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Time it takes for this projectile to spawn.")]
    private float _spawnTime;
    [SerializeField, Tooltip("Projectile prefab to spawn.")]
    private GameObject _projectilePrefab;

    private float _spawnTimer;

    private void Start()
    {
        InvokeRepeating("Spawn", 1f, 3f);
    }

    void Spawn()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
