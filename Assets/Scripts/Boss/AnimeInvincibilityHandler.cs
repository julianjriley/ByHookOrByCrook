using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeInvincibilityHandler : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField, Tooltip("Used to toggle functional invincibility.")]
    private AnimeBoss _boss;
    [SerializeField, Tooltip("Minimum time between when the boss can use invincibility again.")]
    private float _minInterval;
    [SerializeField, Tooltip("Maximum time between when the boss can use invincibility again.")]
    private float _maxInterval;
    [SerializeField, Tooltip("Used to determine the status of THE ORB.")]
    private InvincibilityOrb _theOrb;

    private float _activationTimer = 0;

    [Header("Visuals")]
    [SerializeField, Tooltip("Used to enable invincibility particles.")]
    private GameObject _particles;
    [SerializeField, Tooltip("Used to cycle color of sprite")]
    private SpriteRenderer _renderer;
    [SerializeField, Tooltip("Saturation value used during invincibility cycle.")]
    private float _invincibilitySaturation;
    [SerializeField, Tooltip("Speed at which hue cycles during invincibility.")]
    private float _hueCycleSpeed;

    private float _currHue;

    private void Start()
    {
        // random starting hue
        _currHue = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // only handle functionality if boss can actually use it
        if (_boss.CanBossUseInvincibility())
            HandleFunctionality();

        HandleVisuals();
    }

    /// <summary>
    /// Handles timer for activating invincibility ability periodically
    /// </summary>
    private void HandleFunctionality()
    {
        // check for de-activation
        if (_boss.IsInvincible)
        {
            // end invincibility state when orb is destroyed
            if (_theOrb.IsOrbDestroyed())
            {
                _boss.IsInvincible = false;
            }
        }
        // check for activation
        else
        {
            if (_activationTimer < 0)
            {
                _boss.IsInvincible = true;

                // activate THE ORB
                _theOrb.RespawnOrb();

                // reset timer for next time
                _activationTimer = Random.Range(_minInterval, _maxInterval);
            }
            else
                _activationTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Updates invincibility visuals to match actual state
    /// </summary>
    private void HandleVisuals()
    {
        if (_boss.IsInvincible)
        {
            _particles.SetActive(true);

            // color cycling
            _currHue += _hueCycleSpeed * Time.deltaTime;
            if (_currHue > 1) // re-loop hues
                _currHue = 0;
            Color newColor = Color.HSVToRGB(_currHue, _invincibilitySaturation, 1f);
            _renderer.color = newColor;
        }
        else // not invincible
        {
            _particles.SetActive(false);

            // default appearance
            _renderer.color = Color.white;
        }
    }
}
