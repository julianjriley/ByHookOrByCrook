using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour 
{
    IDamageable _entity;
    public int poisonCounter = 0;
    [SerializeField]
    SpriteRenderer _spriteRenderer;


    private void Start()
    {
        if (TryGetComponent(out SpriteRenderer renderer))
            _spriteRenderer = renderer;

        _entity = GetComponent<IDamageable>();
    }
    public void PassEffect(EffectData effectData)
    {
        switch (effectData.type)
        {
            case EffectData.EffectType.POISON:
                if (poisonCounter > 3)
                    break;
                StartCoroutine(TakePoisonDamage(effectData));
                break;
        }

    }

    IEnumerator TakePoisonDamage(EffectData effectData)
    {
        GameObject poisonVisual = Instantiate(effectData.effectVisual, gameObject.transform);
        poisonCounter += 1;
        _spriteRenderer.color = new Color((255f - 85f * poisonCounter)/255f, 1, (255f - 85f * poisonCounter)/255f);
        for (int i = 0; i < 14; i++)
        {
            yield return new WaitForSeconds(0.5f);
            _entity.TakeDamage(effectData.effectStrength, true);
        }
        Destroy(poisonVisual);
        poisonCounter -= 1;
        _spriteRenderer.color = new Color((255f - 85f * poisonCounter) / 255f, 1, (255f - 85f * poisonCounter) / 255f);
    }
}
