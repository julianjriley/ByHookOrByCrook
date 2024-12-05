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
    [SerializeField, Tooltip("Cannon anim")]
    private Animator _anim;


    private void Start()
    {
        InvokeRepeating("Spawn", 1f, 3f);
        GameManager.Instance.GamePersistent.IsInvulnerable = true;
    }

    void Spawn()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
        _anim.Play("Fire", 0, 0);
    }

    public void ResetInvulnerability()
    {
        GameManager.Instance.GamePersistent.IsInvulnerable = false;
    }
}
