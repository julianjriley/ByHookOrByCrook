using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sunfish : PassiveItemInstance
{

    WeaponInstance[] tempArray;

    public override void ItemEffect()
    {
        base.ItemEffect();
        foreach (Item item in _player.GetInventory().items)
        {
            if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                weapon.CoolingTime = 0;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(HeatLevelOverride());
    }

    IEnumerator HeatLevelOverride()
    {
        yield return new WaitForSeconds(0.3f);
        tempArray = _player.GetWeaponsTransform().GetComponentsInChildren<WeaponInstance>();
        foreach (WeaponInstance weapon in tempArray)
        {
            weapon.SetHeatLevel(150);
        }
    }
}
