using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTarg : MonoBehaviour, IDamageable
{
    private BoxCollider _boxCol;
    [SerializeField] private Animator _anim;

    void Start()
    {
        _boxCol = GetComponent<BoxCollider>();
        gameObject.AddComponent<EffectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Damage parameter is useless here but helps interfaces
    public void TakeDamage(float damage, bool dontUseSound = false)
    {
        _anim.Play("Bobble", 0, 0);
    }

    public void PassEffect(EffectData effectData)
    {
        GetComponent<EffectManager>().PassEffect(effectData);
    }
}
