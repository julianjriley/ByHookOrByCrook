using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityOrb : MonoBehaviour, IDamageable
{
    [Header("THE ORB")]
    [SerializeField, Tooltip("Max health of THE ORB.")]
    private float _maxHealth;
    [SerializeField, Tooltip("Object containing visuals and colliders of THE ORB.")]
    private GameObject _orbObject;
    [SerializeField, Tooltip("Collider to disable when deactivated.")]
    private Collider _orbCollider;

    [Header("Destruction")]
    [SerializeField, Tooltip("Destroy effect to play on destroying the orb.")]
    private GameObject _destroyEffect;
    [SerializeField, Tooltip("Time before destroy effect is destroyed.")]
    private float _destroyTime;

    [Header("Fade-in Effect")]
    [SerializeField, Tooltip("Used to make it faded at first when spawning.")]
    SpriteRenderer _orbRenderer;
    [SerializeField, Tooltip("Opacity of orb renderer when spawning.")]
    private float _spawningOpacity;
    [SerializeField, Tooltip("Glowing beams to enable only on spawn not spawning.")]
    private GameObject _beams;

    private float _currHealth;

    public void PassEffect(EffectData effect)
    {
        GetComponent<EffectManager>().PassEffect(effect);
    }

    public void TakeDamage(float damage, bool dontUseSound = false)
    {
        _currHealth -= damage;
        if (_currHealth < 0)
        {
            // "despawn" orb (you killed it!)
            _orbObject.SetActive(false);
            _orbCollider.enabled = false;
            _beams.SetActive(false);

            // destroy effect
            GameObject deathEffect = Instantiate(_destroyEffect, transform.position, Quaternion.identity);
            Destroy(deathEffect, _destroyTime);
        }
    }

    public void RespawnOrb()
    {
        // restore color to full
        Color currColor = _orbRenderer.color;
        currColor.a = 1;
        _orbRenderer.color = currColor;

        _currHealth = _maxHealth;
        
        // actually enable collider
        _orbCollider.enabled = true;

        _beams.SetActive(true);
    }

    public void ShowOrbSpawning()
    {
        // activate for visuals but collider still disabled
        _orbObject.SetActive(true);

        // make color faded
        Color currColor = _orbRenderer.color;
        currColor.a = _spawningOpacity;
        _orbRenderer.color = currColor;
    }

    public bool IsOrbDestroyed()
    {
        return _currHealth < 0;
    }

    #region Contact Damage
    //The two collision functions are exactly the same its just safety honestly (some projectiles are triggers and others aren't)

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IDamageable>(out IDamageable component) && collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            component.TakeDamage(1, false);

            // orb is unaffected by contact with player
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable component) && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            component.TakeDamage(1, false);

            // orb is unaffected by contact with player
        }
    }
    #endregion
}
