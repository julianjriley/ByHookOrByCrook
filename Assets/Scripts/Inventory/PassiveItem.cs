using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Passive Item", menuName = "Assets/Item/Passive Item")]
public class PassiveItem : Item
{
    public enum PassiveType {ATTACK, SUPPORT, MOVEMENT};
    private PassiveType _passiveType;

    public PassiveType GetPassiveType()
    {
        return _passiveType;
    }

}
