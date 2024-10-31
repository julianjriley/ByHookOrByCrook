using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossPrototype
{
    public float StartDelay = 0f;
    public float DelayBetween = 0f;
    //_playerTransform
    private Transform _spriteTransform;
    void Start() {
        base.Start();
        InvokeRepeating("ChargeAttack", 2f, 2f);
    }

    void ChargeAttack() {
        Debug.Log("Charge Attack");
        //transform.forward = new Vector3(0, 1, 0);
        float angle = Vector3.Angle(new Vector3(1, 0, 0), _playerTransform.position);
        Debug.Log("angle = " + angle);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //transform.LookAt(_playerTransform);
        //transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z);
    }
}
