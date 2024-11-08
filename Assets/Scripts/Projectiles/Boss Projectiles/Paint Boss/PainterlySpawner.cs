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
    [SerializeField, Tooltip("Speed of portrait moving to goal position.")]
    private float _movementSpeed;
    [SerializeField, Tooltip("Higher value = faster move speed while farther away")]
    private float _movementDistanceFactor;

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

        // Motion towards goal - faster speed when farther away
        float distanceFactor = Vector3.Distance(transform.position, _goalPos) * _movementDistanceFactor;
        if (distanceFactor < 1) // prevent approaching but never reaching the target
            distanceFactor = 1;
        transform.position = transform.position + Vector3.down * _movementSpeed * distanceFactor * Time.fixedDeltaTime;

        // snap to goal if it would overshoot
        if (transform.position.y < _goalPos.y)
        {
            transform.position = _goalPos;
            isReady = true;
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
