using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles unique behavior of the turtle projectile - being able to destroy any breakable OR non-breakable boss projectile on impact.
/// </summary>
public class TurtleProjectile : Projectile
{
    [Header("Turtle Projectile")]
    [SerializeField, Tooltip("Whether the projectile destroys or disables on hit.")]
    private bool _isRespawning;
    [SerializeField, Tooltip("Duration it takes for respawning turtle to come back.")]
    private float _respawnDelay;
    [SerializeField, Tooltip("Used to disable turtle collider.")]
    private Collider _collider;
    [SerializeField, Tooltip("Used to disable sprite.")]
    private SpriteRenderer _sprite;

    /// <summary>
    /// Re-enables sprite and collider.
    /// </summary>
    private void Respawn()
    {
        _collider.enabled = true;
        _sprite.enabled = true;
    }

    /// <summary>
    /// destroys turtle (if projectile) OR disables projectile (if shield).
    /// </summary>
    private void DestroyOrDisable()
    {
        if (_isRespawning)
        {
            _collider.enabled = false;
            _sprite.enabled = false;

            // call to re-enable after delay
            Invoke("Respawn", _respawnDelay);
        }
        else
            Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        // apply damage to boss normally
        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collider.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);

            DestroyOrDisable();
        }

        // destroy BOTH breakable and non-breakable projectiles
        if (collider.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collider.gameObject.layer == LayerMask.NameToLayer("BossProjectile"))
        {
            // destroy projectile regardless of its health value
            Destroy(collider.gameObject);

            DestroyOrDisable();
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        // apply damage to boss normally
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.gameObject.GetComponent<BossPrototype>().TakeDamage(_damage);

            DestroyOrDisable();
        }

        // destroy BOTH breakable and non-breakable projectiles
        if (collision.gameObject.layer == LayerMask.NameToLayer("BreakableBossProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("BossProjectile"))
        {
            // destroy projectile regardless of its health value
            Destroy(collision.gameObject);

            DestroyOrDisable();
        }
    }
}
