using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineController : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField]
    private Transform[] _linePoints;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, _linePoints[0].position);
    }

    void Update()
    {
        // for our current purposes, our fishing line never moves origins, so I don't need to update the point of the origin.
        // IF THIS CHANGES, simply create an iterative loop that updates all _linePoints in the Transform array
        
        _lineRenderer.SetPosition(1, _linePoints[1].position);

        // make sure the start of the line ALWAYS aligns with the rod end
        _lineRenderer.SetPosition(0, _linePoints[0].position);
    }
}
