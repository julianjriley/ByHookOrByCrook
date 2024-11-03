using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brickfish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.BaseHealth *= 2;
    }
}
