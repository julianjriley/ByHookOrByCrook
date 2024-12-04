using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles delay between portal indicating projectile about to spawn and then actually instantiating projectile.
/// </summary>
public class CannonProjectileSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Projectile prefab to spawn.")]
    private GameObject _projectilePrefab;

    private void Start()
    {
        InvokeRepeating("Spawn", 1f, 3f);
        GameManager.Instance.GamePersistent.IsInvulnerable = true;
    }

    void Spawn()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }

    public void ResetInvulnerability()
    {
        GameManager.Instance.GamePersistent.IsInvulnerable = false;
    }
}
