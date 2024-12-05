using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PracticeTarg : MonoBehaviour, IDamageable
{
    private BoxCollider _boxCol;
    [SerializeField] private Animator _anim;

    [Header("SFX")]
    [SerializeField] EventReference damageSound;

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
        if (!dontUseSound)
            SoundManager.Instance.PlayOneShot(damageSound, gameObject.transform.position);
    }

    public void PassEffect(EffectData effectData)
    {
        GetComponent<EffectManager>().PassEffect(effectData);
    }
}
