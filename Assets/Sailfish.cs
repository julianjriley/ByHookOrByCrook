using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sailfish : PassiveItemInstance
{
    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach(Item item in _player.GetInventory().items)
        {
            if(item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                weapon.Speed *= 2f;
            }
        }
    }
}
