using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HeartAoE : Projectile
{
    private Animator _anim;
    private float _lerpSpeed = 3f;
    [SerializeField] ParticleSystem _ps;

    // TODO: plug in animation
    // TODO: slowly appears then fully appears (timer of some sort is needed)

    protected override void Start()
    {
        base.Start();

        // make them float up and down 
        //Float();
        StartCoroutine(DoAttack());
    }

    void Float()
    {
        Vector3 temp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        temp.y = Mathf.Lerp(transform.position.y, transform.position.y -3, _lerpSpeed);

        transform.position = temp;
    }
    
    IEnumerator DoAttack()
    {
        _anim.Play("HeartATK", 0, 0);
        yield return new WaitForSeconds(2f); // Initial anim plays
        _ps.Play();
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }
}
