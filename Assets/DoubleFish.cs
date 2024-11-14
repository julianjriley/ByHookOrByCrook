using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.GetPlayerMovement().numberOfAirDashes = 2;
    }
}
