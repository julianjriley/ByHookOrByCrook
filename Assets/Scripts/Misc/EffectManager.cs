using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour 
{
    IDamageable _entity;
    public int poisonCounter;
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _entity = GetComponent<IDamageable>();
    }
    public void PassEffect(EffectData effectData)
    {
        switch (effectData.type)
        {
            case EffectData.EffectType.POISON:
                StartCoroutine(TakePoisonDamage(effectData.effectStrength));
                break;
        }

    }

    IEnumerator TakePoisonDamage(float damage)
    {
        if (poisonCounter > 0)
            yield break;
        poisonCounter += 1;
        for (int i = 0; i < 7; i++)
        {
            _entity.TakeDamage(damage);
            yield return new WaitForSeconds(damage);
        }

        poisonCounter -= 0; 
    }
}
