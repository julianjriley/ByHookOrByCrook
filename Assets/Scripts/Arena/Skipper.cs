using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skipper : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;
    private float _distanceToPlayer;
    private Vector3 _offset;
    private Vector3 _desiredLocation;
    private Rigidbody _rb;
    [SerializeField] private float speed = 2f;

    [SerializeField] private Transform _aimPivot;
    private bool _facingLeft = false;
    private Vector3 _weaponDirection;

    [SerializeField] WeaponInstance _skipperGlock;



    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _boss = GameObject.FindWithTag("Boss");
        _offset = new Vector3(-1.2f, 0, 0);
        _rb = GetComponent<Rigidbody>();
        _weaponDirection = (_boss.transform.position - _aimPivot.transform.position).normalized;
        StartCoroutine(BeginFiring());

    }
    private void FixedUpdate()
    {
        _desiredLocation = _player.transform.position + _offset;
        _distanceToPlayer = Mathf.Abs((_desiredLocation - transform.position).magnitude);
        _rb.AddForce((_desiredLocation - transform.position) * _distanceToPlayer * speed);

        _weaponDirection = (_boss.transform.position - _aimPivot.transform.position).normalized;
        UpdateRotation(_weaponDirection);
        _skipperGlock.SetAim(_weaponDirection);

    }

    private void Update()
    {
        if (_boss.transform.position.x < gameObject.transform.position.x)
        {
            _facingLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            _facingLeft = false;
        }
    }

    public void UpdateRotation(Vector2 lookAt)
    {
        if (!_facingLeft)
            _aimPivot.transform.rotation = Quaternion.FromToRotation(Vector3.right, lookAt);
        else
            _aimPivot.transform.rotation = Quaternion.FromToRotation(-Vector3.right, lookAt);
    }

    IEnumerator BeginFiring()
    {
        yield return new WaitForSeconds(0.5f);
        _skipperGlock.Fire(_weaponDirection);

    }
}
