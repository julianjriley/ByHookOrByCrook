using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossPrototype
{
    [Header ("Charge Attack")]
    public float StartDelay = 0f;
    public float DelayBetween = 0f;
    public Transform AboveRight;
    public Transform AboveLeft;
    public Transform Right;
    public Transform Right_2;
    public Transform LowerRight;
    public Transform LowerRight_2;
    public Transform Left;
    public Transform Left_2;
    public Transform LowerLeft;
    public Transform LowerLeft_2;
    private Animator _anim;

    private Transform _spriteTransform;
    private BossTargetRepositioner _targetRepositioner;
    private CapsuleCollider _capsule;
    void Start() {
        base.Start();
        _anim = GetComponent<Animator>();
        _capsule = GetComponent<CapsuleCollider>();
        _targetRepositioner = GameObject.Find("BossTargetRepositioner").GetComponent<BossTargetRepositioner>();
        StartCharging();
    }

    public void StartCharging() {
        InvokeRepeating("ChargeAttack", StartDelay, DelayBetween);
    }

    void ChargeAttack() {
        //if (transform.position.x >= _playerTransform.position.x) {
            //StartCoroutine(RightCharge());
        //} else {
            StartCoroutine(LeftCharge());
        //}
    }
    IEnumerator LeftCharge() { //yield return new WaitForSeconds(rand);
        SetNewTarget(AboveLeft, -1);
        yield return new WaitForSeconds(0.5f);
        //adjust capsule collider to fit drill
        _capsule.direction = 0;
        _capsule.center = new Vector3(0f, -0.45f, 0f);

        SetNewTarget(Left, -1);
        yield return new WaitForSeconds(2f);
        _anim.SetBool("drillIn", true);
        SetNewTarget(Left_2, -1);
        yield return new WaitForSeconds(0.7f);
        _anim.SetBool("drill", true);
        SetSpeed(125f);
        Debug.Log("Speed" + Speed);
        SetNewTarget(Right, 2.5f);
        yield return new WaitForSeconds(1.75f);
        //reset capsule collider to normal
        _capsule.direction = 1;
        _capsule.center = Vector3.zero;

        _anim.SetBool("drillOut", true);
        SetSpeed(50f);
        _anim.SetBool("drillIn", false);
        _anim.SetBool("drill", false);
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("drillOut", false);
        _targetRepositioner.NewBossTarget(0, 16);
    }
    //IEnumerator RightCharge() {
        //yield return new WaitForSeconds(0.5f);
    //}
}
