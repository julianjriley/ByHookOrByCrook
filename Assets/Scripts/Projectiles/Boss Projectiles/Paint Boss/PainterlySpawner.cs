using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement and behavior of 'enemy' spawners in painterly boss.
/// When spawned, they will glow to indicate a planned attack on a particular canvas.
/// When the boss is close to the spawner, the paint effect will play, and then after a delay the 'enemy' will spawn.
/// </summary>
public class PainterlySpawner : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField, Tooltip("Position painting is moving towards before it is ready to be painted on.")]
    private Vector2 _goalPos;
    [SerializeField, Tooltip("Snappinesss of portrait moving to goal position.")]
    private float _movementSharpness;
    [SerializeField, Tooltip("Distance from goal at which painting snaps to goal")]
    private float _snappingDistance;

    [Header("Painting/Spawning")]
    [SerializeField, Tooltip("Renderer with paint effect.")]
    private SpriteRenderer _paintEffect;
    [SerializeField, Tooltip("Projectile/Enemy prefab to create once painting is complete")]
    private GameObject _enemyToSpawn;

    [HideInInspector]
    public bool isReady = false;

    private void FixedUpdate()
    {
        // skip movement checks if it's already there
        if (isReady)
            return;

        // snap to goal
        if(Vector3.Distance(transform.position, (Vector3) _goalPos) < _snappingDistance)
        {
            transform.position = _goalPos;
            isReady = true;
        }
        // smoothly lerp towards goal
        else
        {
            transform.position = Vector3.Lerp(transform.position, _goalPos, 1f - Mathf.Exp(-_movementSharpness * Time.fixedDeltaTime));
        }
    }

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
