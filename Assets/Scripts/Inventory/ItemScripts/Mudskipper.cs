using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mudskipper : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.canInvincibleDash = true;
    }
}
