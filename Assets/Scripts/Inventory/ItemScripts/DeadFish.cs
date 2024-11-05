using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.canRevive = true;
    }
}
