using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles cycling behavior of an individual Galaga Ship projectile.
/// </summary>
public class GalagaShip : Projectile
{
    [SerializeField, Tooltip("Duration of each downward movement phase. each horizontal phase is some multiple of this.")]
    private float _movePhaseDuration;

    Vector2 _moveDir = Vector3.right;
    bool _isRightNext = false;
    int _stepCounter = 4;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        // ensure gravity is disabled
        _rb.useGravity = false;

        // after the first move phase ends, start looping direction switching
        InvokeRepeating("SwitchDirection", _movePhaseDuration, _movePhaseDuration);
    }

    // FixedUpdate is called once per physics frame
    override protected void FixedUpdate()
    {
        // set speed to match move direction
        _rb.velocity = _moveDir * _speed;
    }

    private void SwitchDirection()
    {
        // swap from down to right/left
        if (_moveDir == Vector2.down)
        {
            // set it properly to right/left state
            _moveDir = _isRightNext ? Vector2.right : Vector2.left;

            // switch direction for next time
            _isRightNext = !_isRightNext;
        }
        // swap from right/left to down
        else
        {
            // ensure horizontal phases multiple steps longer
            if (_stepCounter == 7)
            {
                _moveDir = Vector2.down;
                _stepCounter = 0;
            }
            else
                _stepCounter++; ;
        }
    }
}
