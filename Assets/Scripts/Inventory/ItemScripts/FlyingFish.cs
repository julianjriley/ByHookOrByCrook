using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.GetPlayerMovement().jumpImpulseUP += 8;
    }
}
