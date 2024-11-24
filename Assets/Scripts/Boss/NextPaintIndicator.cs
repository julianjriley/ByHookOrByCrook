using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPaintIndicator : MonoBehaviour
{
    [SerializeField, Tooltip("Used to script behavior of paint indicator.")]
    private LineRenderer _lineRenderer;
    [SerializeField, Tooltip("Paint script to access paint boss's next target.")]
    private PaintBoss _paintBoss;

    void Start()
    {
        // off until needed
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        // disable line renderer if there is no next target yet
        if (_paintBoss.GetNextPaintingTransform() is null)
        {
            _lineRenderer.enabled = false;
            return;
        }

        // there is a transform target, so enable line renderer
        _lineRenderer.enabled = true;

        // make the start always match boss position
        _lineRenderer.SetPosition(0, _paintBoss.transform.position);

        // make the end match the current goal
        _lineRenderer.SetPosition(1, _paintBoss.GetNextPaintingTransform().position);
    }
}
