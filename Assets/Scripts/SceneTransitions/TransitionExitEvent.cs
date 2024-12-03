using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles transferring event of animation end to the SceneTransitionsHandler.
/// </summary>
public class TransitionExitEvent : MonoBehaviour
{
    [SerializeField, Tooltip("Used to indicate when animation is complete.")]
    SceneTransitionsHandler _handler;

    /// <summary>
    /// marks animation as complete in the transition handler.
    /// </summary>
    public void AnimationComplete()
    {
        _handler.SetReadyToLoad();
    }
}
