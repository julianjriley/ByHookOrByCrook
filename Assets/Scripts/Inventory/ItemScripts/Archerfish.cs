using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archerfish : PassiveItemInstance
{
    [SerializeField] private float _fireRateIncrease;
    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach(Item item in _player.GetInventory().items)
        {
            if(item is Weapon)
            {
                (item as Weapon).FireRate += _fireRateIncrease;
            }

        }
    }
}
