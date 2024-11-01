using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles unique behavior of painterly boss.
/// Overrides movement inputs to favor targetting 'glowing' paintings, if any exist (by latest phase this is likely most of the boss movement).
/// </summary>
public class PaintBoss : BossPrototype
{
    override protected void FixedUpdate()
    {
        // using SetNewTarget(), I'll need to modify the function or make an alternate one that will move to a spot, then wait a duration before changing back.

        // move, defeat, and phase switch logic.
        base.FixedUpdate();
    }
}