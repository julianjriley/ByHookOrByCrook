using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashResetsJumpFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.GetPlayerMovement().canDashResetJump = true;
    }
}
