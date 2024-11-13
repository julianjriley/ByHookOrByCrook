using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

public class Boss1 : BossPrototype
{
    [Header ("Charge Attack")]
    public float StartDelay = 0f;
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

    [Header("Galaga Attacks")]
    [SerializeField, Tooltip("Galaga attack with one rows")]
    private GameObject _galaga1;
    [SerializeField, Tooltip("Galaga attack with two rows")]
    private GameObject _galaga2;
    [SerializeField, Tooltip("Galaga attack with three rows")]
    private GameObject _galaga3;

    private Transform _spriteTransform;
    private BossTargetRepositioner _targetRepositioner;
    private CapsuleCollider _capsule;
    private bool _isDrilling = false;

    override protected void Start() {
        base.Start();

        _anim = GetComponent<Animator>();
        _capsule = GetComponent<CapsuleCollider>();
        _targetRepositioner = GameObject.Find("BossTargetRepositioner").GetComponent<BossTargetRepositioner>();
    }

    public void StartCharging(float delayBetween) {
        // allows next phase to reset the invoke repeating to happen with a different delay
        CancelInvoke("ChargeAttack");
        // start repeating attack
        InvokeRepeating("ChargeAttack", StartDelay, delayBetween);        
    }

    void ChargeAttack() {
        if (transform.position.x >= _playerTransform.position.x) {
            //charge to the right
            int topOrBottom = Random.Range(0, 2); 
            if (topOrBottom == 0) { //0 means top row charge
                StartCoroutine(ChargeLogic(AboveRight, Right, Right_2, Left, 180, 16));
            } else {
                StartCoroutine(ChargeLogic(AboveRight, LowerRight, LowerRight_2, LowerLeft, 180, 16));
            }
            
        } else {
            //charge to the left
            int topOrBottom = Random.Range(0, 1);
            if (topOrBottom == 0) { //0 means top row charge
                StartCoroutine(ChargeLogic(AboveLeft, Left, Left_2, Right, 0, 16));
            } else {
                StartCoroutine(ChargeLogic(AboveLeft, LowerLeft, LowerLeft_2, LowerRight, 0, 16));
            }
        }
    }
    public override void Move() {
        if (_rb == null) {
            Debug.Log("No rigidbody");
            return;
        }
        if (_target == null) {
            Debug.Log("No target");
            return;
        }
        //if (Mathf.Abs(_rb.velocity.x) < 13) {
        if (!_isDrilling) {
            
            float rotationVal = remap(-13, 13, 20, -20, _rb.velocity.x);
            if (_renderer.flipX == false) {
                transform.rotation = Quaternion.Euler(rotationVal, 0f, 0f);
            } else {
                transform.rotation = Quaternion.Euler(-rotationVal, 0f, 0f);
            }
            
        }
            //transform.rotation = Quaternion.Euler(_rb.velocity.x * 2, 0f, 0f); //Vector3.Magnitude(
        //}
        //Debug.Log("Rigidbody velocity = " + _rb.velocity);
        _rb.AddForce((_target.position - transform.position).normalized * Speed, ForceMode.Force);
        if (!(_checkingSwap)) { //ensure only one check is happening at a time
            SpriteSwapCheck();
        } 
    }

    //no longer in use, see function below this one
    /*IEnumerator LeftCharge() { //yield return new WaitForSeconds(rand);
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
    }*/
    
    public IEnumerator ChargeLogic(Transform target1, Transform target2, Transform target3, Transform target4, int rotation, int scale) {
        //this function changes the bosses' targets to make it charge across the screen
        SetNewTarget(target1, -1);
        yield return new WaitForSeconds(0.5f);

        SetNewTarget(target2, -1);
        yield return new WaitForSeconds(2f);

        //adjust capsule collider to fit drill better
        _capsule.direction = 0;
        _capsule.center = new Vector3(0f, -0.45f, 0f);

        _isDrilling = true;
        //disable portal children bc drill animation includes the portal
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        _anim.SetBool("drillIn", true);


        yield return new WaitForSeconds(0.7f);
        _anim.SetBool("drill", true);

        // drill for 2 seconds before actually starting motion as a strong visual cue
        yield return new WaitForSeconds(2f);

        // pull back before lunging across
        SetNewTarget(target3, -1);
        yield return new WaitForSeconds(1f);

        SetSpeed(125f);
        //Debug.Log("Speed" + Speed);
        SetNewTarget(target4, 2.5f);
        yield return new WaitForSeconds(1.75f);
        //reset capsule collider to normal
        _capsule.direction = 1;
        _capsule.center = Vector3.zero;

        _anim.SetBool("drillOut", true);
        SetSpeed(50f);
        _anim.SetBool("drillIn", false);
        _anim.SetBool("drill", false);
        yield return new WaitForSeconds(0.2f);
        _isDrilling = false;
        _anim.SetBool("drillOut", false);
        _targetRepositioner.NewBossTarget(rotation, scale);
        yield return new WaitForSeconds(0.4f);
        //reenable portals
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    /// <summary>
    /// Used at start of phase 1
    /// </summary>
    public void SpawnGalaga1()
    {
        SpawnAttackOnce(_galaga1);
    }

    /// <summary>
    /// Used at start of phase 2
    /// </summary>
    public void SpawnGalaga2()
    {
        SpawnAttackOnce(_galaga2);
    }

    /// <summary>
    /// Used at start of phase 3
    /// </summary>
    public void SpawnGalaga3()
    {
        SpawnAttackOnce(_galaga3);
    }
}
