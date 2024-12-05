using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipseStarfish : PassiveItemInstance
{
    public static event Action SunfishInteraction;

    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach (Item item in _player.GetInventory().items)
        {
            if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                weapon.overheatDamageBonus = true;
            }
        }
        SunfishInteraction?.Invoke();
    }
}
