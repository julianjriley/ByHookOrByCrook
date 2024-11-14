using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Passive Item", menuName = "Assets/Item/Passive Item")]
public class PassiveItem : Item
{
    public override void SetPlayer(PlayerCombat player)
    {
        base.SetPlayer(player);
        CreatePrefabOnPlayer();
    }

    public void CreatePrefabOnPlayer()
    {
        _player.AppendItemToPassiveInstances(_itemPrefab);
    }

}
