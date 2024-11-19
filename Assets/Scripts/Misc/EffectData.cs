using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectData
{
    public enum EffectType { POISON, DEFENSEDOWN, SLOW};
    public EffectType type;
    public float effectStrength;

    public EffectData(EffectType _type, float _effectStrength)
    {
        type = _type;
        effectStrength = _effectStrength;
    }
}
