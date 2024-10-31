using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piranha : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.useShortRangeDamage = true;
    }
}
