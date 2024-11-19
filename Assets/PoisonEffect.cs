using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private float _damage;
    [SerializeField] private float _repeatInterval;
    IDamageable _entity;
    SpriteRenderer _entitySpriteRenderer;
    private void Start()
    {

        
    }
    
    public void AssignValues(float damage)
    {
        _damage = damage;
        _entitySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        _entity = GetComponentInParent<IDamageable>();
        StartCoroutine(TickDamage());
    }

    IEnumerator TickDamage()
    {

        _entitySpriteRenderer.color = Color.green;
        for(int i = 0; i < 5; i++)
        {
            _entity.TakeDamage(_damage);
            yield return new WaitForSeconds(_repeatInterval);
        }
        _entitySpriteRenderer.color = Color.white;
        Destroy(gameObject);
    }
}
