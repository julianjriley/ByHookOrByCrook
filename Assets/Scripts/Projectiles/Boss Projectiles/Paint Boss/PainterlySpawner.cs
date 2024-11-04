using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles behavior of 'enemy' spawners in painterly boss.
/// When spawned, they will glow to indicate a planned attack on a particular canvas.
/// When the boss is close to the spawner, the paint effect will play, and then after a delay the 'enemy' will spawn.
/// </summary>
public class PainterlySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Renderer with paint effect.")]
    private SpriteRenderer _paintEffect;
    [SerializeField, Tooltip("Projectile/Enemy prefab to create once painting is complete")]
    private GameObject _enemyToSpawn;

    /// <summary>
    /// Enables visual paint effect on spawner.
    /// </summary>
    public void StartPaint()
    {
        _paintEffect.enabled = true;
    }

    /// <summary>
    /// Starts process of creating paint effect before spawning 'enemy' and destroying itself.
    /// </summary>
    public void Spawn()
    {
        Instantiate(_enemyToSpawn, transform.position, transform.rotation);

        // destroy painterlySpawner on spawn
        Destroy(gameObject);
    }
}
