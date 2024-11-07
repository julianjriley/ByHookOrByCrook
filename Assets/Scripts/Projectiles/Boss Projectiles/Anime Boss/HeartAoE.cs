using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAoE : Projectile
{
    private Color _heartColor;
    
    // TODO: plug in animation
    // TODO: slowly appears then fully appears (timer of some sort is needed)

    protected override void Start()
    {
        base.Start();

        _heartColor = GetComponent<SpriteRenderer>().color;
        _heartColor.a = 0;

        StartCoroutine(ChangeOpacity());
    }
    
    IEnumerator ChangeOpacity()
    {
        _heartColor.a = Mathf.MoveTowards(1, 0, 5f);
        yield return new WaitForSeconds(1);
    }
}
