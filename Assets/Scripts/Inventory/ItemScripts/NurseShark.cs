using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseShark : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        _player.BaseHealth += _player.Health/3;
    }
}
