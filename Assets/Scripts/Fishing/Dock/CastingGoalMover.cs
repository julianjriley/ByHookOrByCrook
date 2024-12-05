using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CastingGoalMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Leftmost x local position that goal can move to.")]
    private float _minXPos;
    [SerializeField, Tooltip("Rightmost x local position that goal can move to.")]
    private float _maxXPos;

    [Header("Score Calculation")]
    [SerializeField, Tooltip("Used for determining how close the player got to the goal.")]
    private GameObject _bobber;
    [SerializeField, Tooltip("Distance threshold from casting goal where fishing goal is treated as perfect (max score of 1).")]
    private float _maxPerfectDistance;
    [SerializeField, Tooltip("Distance threshold from casting goal where fishing goal is treated as COMPLETE failure (min score of 0).")]
    private float _maxFailureDistance;

    [Header("Accessibility")]
    [SerializeField, Tooltip("Accessibility distance threshold from casting goal where fishing goal is treated as perfect (max score of 1).")]
    private float _maxPerfectDistanceAccessibility;
    [SerializeField, Tooltip("Accessibility distance threshold from casting goal where fishing goal is treated as COMPLETE failure (min score of 0).")]
    private float _maxFailureDistanceAccessibility;

    /// <summary>
    /// Returns casting score based on distance from casting goal
    /// </summary>
    public float GetCastingScore()
    {
        // calculate distance from goal
        float distance = Mathf.Abs(_bobber.transform.localPosition.x - transform.localPosition.x);

        // account for accessibility bobber
        float maxPerfect = GameManager.Instance.GamePersistent.IsBobber ? _maxPerfectDistanceAccessibility : _maxPerfectDistance;
        float maxFailure = GameManager.Instance.GamePersistent.IsBobber ? _maxFailureDistanceAccessibility : _maxFailureDistance;
        
        // perfect cast
        if (distance < maxPerfect)
            return 1f;

        // complete failure cast
        if (distance > maxFailure)
            return 0f;

        // Return some variable score based on where between thresholds the cast was.
        return math.remap(maxPerfect, maxFailure, 1, 0, distance);
    }

    /// <summary>
    /// Shifts casting goal position to random place within acceptable range
    /// </summary>
    public void RandomizeCastingGoal()
    {
        Vector3 randPos = transform.localPosition;
        randPos.x = UnityEngine.Random.Range(_minXPos, _maxXPos);
        transform.localPosition = randPos;

        // TODO: integrate this with some fade-in / fade-out visual effect?
    }

    // Start is called before the first frame update
    void Start()
    {
        // start at random location
        RandomizeCastingGoal();
    }

    /// <summary>
    /// Whether the bobber was so far it is a max failure.
    /// </summary>
    /// <returns></returns>
    public bool IsSuperFar()
    {
        // account for accessibility bobber
        float maxFailure = GameManager.Instance.GamePersistent.IsBobber ? _maxFailureDistanceAccessibility : _maxFailureDistance;

        return Mathf.Abs(_bobber.transform.localPosition.x - transform.localPosition.x) > maxFailure;
    }
}
