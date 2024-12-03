using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles transferring event of animation end to the disabling image & raycast.
/// </summary>
public class TransitionEnterEvent : MonoBehaviour
{
    [SerializeField, Tooltip("Used to disable the image at the end of animation.")]
    private Image _transitionImage;

    /// <summary>
    /// Called by the animator event to disable image & raycast.
    /// </summary>
    public void AnimationComplete()
    {
        _transitionImage.enabled = false;
    }
}
