using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : PassiveItemInstance
{
    

    //Example Item. Makes the player ZOOM
    public override void ItemEffect()
    {

        base.ItemEffect();
        _player.Speed += 10;
    }
}
