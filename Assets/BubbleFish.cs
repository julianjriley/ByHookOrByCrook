using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.GetPlayerMovement().infiniJumpFish = true;
    }
}
