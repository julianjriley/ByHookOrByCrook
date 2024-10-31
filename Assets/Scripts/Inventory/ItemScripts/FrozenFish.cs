using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenFish : PassiveItemInstance
{
    [SerializeField] private float _coolRateIncrease;
    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach (Item item in _player.GetInventory().items)
        {
            if (item is Weapon)
            {
                (item as Weapon).CoolingTime += _coolRateIncrease;
            }

        }
    }
}
