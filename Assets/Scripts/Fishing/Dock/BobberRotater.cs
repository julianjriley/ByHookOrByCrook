using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BobberRotater : MonoBehaviour
{
    [SerializeField, Tooltip("Used to get velocity.")]
    private Rigidbody2D _rb;
    [SerializeField, Tooltip("object rotated by the function.")]
    private GameObject _bobberSprite;
    [SerializeField, Tooltip("Max horizontal speed used in the remap.")]
    private float _maxSpeed;
    [SerializeField, Tooltip("Max rotation used in remap.")]
    private float _maxRotation;

    // Update is called once per frame
    void Update()
    {
        float rotationVal = math.remap(-_maxSpeed, _maxSpeed, -_maxRotation, _maxRotation, _rb.velocity.x);
        rotationVal = Mathf.Clamp(rotationVal, -_maxRotation, _maxRotation);
        _bobberSprite.transform.rotation = Quaternion.Euler(0f, 0f, rotationVal);
    }
}
