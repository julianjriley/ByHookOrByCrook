using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piranha : PassiveItemInstance
{
    public static event Action SunfishInteraction;

    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.useShortRangeDamage = true;
        SunfishInteraction?.Invoke();
    }
}
