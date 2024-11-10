using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles turtle shield tracking based on the aim direction of the player.
/// Allows configuration for front or back shield.
/// </summary>
public class TurtleShield : PassiveItemInstance
{
    [Header("Turtle Shield")]
    [SerializeField, Tooltip("Whether it is front shield or back shield.")]
    private bool _isFront;
    [SerializeField, Tooltip("The shield itself. used to script its position based on player looking position.")]
    private GameObject _shield;
    [SerializeField, Tooltip("The distance form the player's origin that the shield tracks to.")]
    private float _trackingDistance;

    public override void ItemEffect()
    {
        base.ItemEffect();
    }

    private void FixedUpdate()
    {
        // calculate goal position / rotation
        Vector3 goalVec = (_isFront ? 1 : -1) * _player.GetAimDirection() * _trackingDistance;
        Vector3 goalPos = _player.transform.position + goalVec;
        Quaternion goalRot = Quaternion.FromToRotation(Vector3.up, goalVec);

        // update pos/rot
        _shield.transform.SetPositionAndRotation(goalPos, goalRot);
    }
}
