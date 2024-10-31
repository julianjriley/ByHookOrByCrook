using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.GetPlayerMovement().maxNumberOfJumps += 1;
    }
}
