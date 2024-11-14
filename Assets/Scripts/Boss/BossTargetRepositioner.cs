using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTargetRepositioner : MonoBehaviour
{
    [Header ("Parameters")]
    //public float RotationMin;
    //public float RotationMax;
    public float ScaleMin;
    public float ScaleMax;
    public float TimeBetweenRepositions;
    public float Duration;
    private BoxCollider _bounds;
    private GameObject _rotater;
    private Transform _rotaterTransform;
    private GameObject _currentTarget;
    private Transform _currentTargetTransform;
    private Coroutine _lerpRotation;
    private Coroutine _lerpScale;
    private Transform _playerTransform;
    private bool _hasProtectedTarget = false;
    
    void Start()
    {
        _bounds = GameObject.Find("TargetBoundingBox").GetComponent<BoxCollider>();
        _rotater = GameObject.Find("Rotater");
        _rotaterTransform = _rotater.GetComponent<Transform>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        InvokeRepeating("MakeNewTarget", TimeBetweenRepositions, TimeBetweenRepositions);
    }
    void FixedUpdate() {
        //_rotaterTransform.localPosition = Vector3.zero; //fix bug where rotator/circle does not stay with the player as a child
        transform.localPosition = _playerTransform.localPosition;

        if (_currentTargetTransform != null) {
            //_currentTargetTransform.localPosition = Vector3.zero;
            //Debug.Log("CurrentTargetTransform != null");
            if (_bounds.bounds.Contains(_currentTargetTransform.GetChild(0).position) != true && !_hasProtectedTarget) { //if target goes outside of bounds, regenerate a new one and restart the invokerepeating
                StopCoroutine(_lerpScale);
                StopCoroutine(_lerpRotation);
                CancelInvoke();
                MakeNewTarget();
                InvokeRepeating("MakeNewTarget", TimeBetweenRepositions, TimeBetweenRepositions);
            }
        }
    }

    public void MakeNewTarget() {
        _hasProtectedTarget = false;
        //make new rotator and target
        GameObject newTarget;
        newTarget = (GameObject) Instantiate((Object) _rotater, transform);
        Transform newTransform = newTarget.GetComponent<Transform>();

        //randomly scale new rotator and target
        float randScale = Random.Range(ScaleMin, ScaleMax);
        Vector3 newScale = new Vector3(randScale, randScale, randScale);
        newTransform.localScale = newScale;

        //randomly set rotation of new rotator and target
        float randRotation = Random.Range(0f, 360f);
        newTransform.Rotate(0f, 0f, randRotation, Space.Self);

        //check if position of new target is in the bounds
        //BUG: I think if it keeps generating targets that our out of bounds, it overloads the stack
        if (_bounds.bounds.Contains(newTransform.GetChild(0).position) != true) {
            //Debug.Log("Target outside of bounds, making new target");
            MakeNewTarget();
            Destroy(newTarget);
            return;
        }
        //if code reaches this point, change the target and delete old one
        GameObject oldTarget = _currentTarget;
        _currentTarget = newTarget;
        _currentTargetTransform = _currentTarget.GetComponent<Transform>();
        _currentTarget.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(oldTarget);

        //lerp rotater to newTarget scale and rotation over time
        float rotaterZ = _rotaterTransform.eulerAngles.z;
        float targetZ = _currentTarget.GetComponent<Transform>().eulerAngles.z;
        _lerpRotation = StartCoroutine(LerpRotation(rotaterZ, targetZ, Duration));

        Vector3 rotaterScale = _rotaterTransform.localScale;
        Vector3 targetScale = _currentTargetTransform.localScale;
        _lerpScale = StartCoroutine(LerpScale(rotaterScale, targetScale, Duration));
    }
    IEnumerator LerpRotation(float start, float end, float duration) 
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (3f - 2f * t); //equation helps smooth the interpolation
            _rotaterTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, start), Quaternion.Euler(0, 0, end), t); //only rotates on y axis
            time += Time.deltaTime;
            yield return null;
        }
        _rotaterTransform.rotation = Quaternion.Euler(0, 0, end); //ensure it fully reaches target angle
    }
    IEnumerator LerpScale(Vector3 start, Vector3 end, float duration) 
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (3f - 2f * t); //equation helps smooth the interpolation
            _rotaterTransform.localScale = Vector3.Lerp(start, end, t); 
            time += Time.deltaTime;
            yield return null;
        }
        _rotaterTransform.localScale = end; 
    }
    public void NewBossTarget(int rotation, int scale) {
        CancelInvoke();
        StopCoroutine(_lerpScale);
        StopCoroutine(_lerpRotation);
        _hasProtectedTarget = true;
        //make new rotator and target
        GameObject newTarget;
        newTarget = (GameObject) Instantiate((Object) _rotater, transform);
        Transform newTransform = newTarget.GetComponent<Transform>();

        //scale new rotator and target
        Vector3 newScale = new Vector3(scale, scale, scale);
        newTransform.localScale = newScale;

        //set rotation of new rotator and target
        newTransform.localRotation = Quaternion.Euler(0, 0, 0);
        newTransform.Rotate(0f, 0f, rotation, Space.World);

        //if code reaches this point, change the target and delete old one
        GameObject oldTarget = _currentTarget;
        _currentTarget = newTarget;
        _currentTargetTransform = _currentTarget.GetComponent<Transform>();
        _currentTarget.GetComponent<SpriteRenderer>().color = Color.red;
        _currentTargetTransform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(oldTarget);

        //lerp rotater to newTarget scale and rotation over time
        float rotaterZ = _rotaterTransform.eulerAngles.z;
        float targetZ = _currentTarget.GetComponent<Transform>().eulerAngles.z;
        _lerpRotation = StartCoroutine(LerpRotation(rotaterZ, targetZ, Duration/2));

        Vector3 rotaterScale = _rotaterTransform.localScale;
        Vector3 targetScale = _currentTargetTransform.localScale;
        _lerpScale = StartCoroutine(LerpScale(rotaterScale, targetScale, Duration/2));
        // Debug.Log("NewBossTarget finished");
        //restart invoke repeating
        InvokeRepeating("MakeNewTarget", TimeBetweenRepositions, TimeBetweenRepositions);
    }
}
