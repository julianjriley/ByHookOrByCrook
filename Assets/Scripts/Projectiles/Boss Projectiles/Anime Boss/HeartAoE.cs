using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HeartAoE : Projectile
{
    private Color _heartColor;
    float _lerpSpeed = 3f;

    // TODO: plug in animation
    // TODO: slowly appears then fully appears (timer of some sort is needed)

    protected override void Start()
    {
        base.Start();

        _heartColor = GetComponent<SpriteRenderer>().color;
        _heartColor.a = 0;

        // make them float up and down 
        //Float();
        StartCoroutine(ChangeOpacity());
    }

    void Float()
    {
        Vector3 temp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        temp.y = Mathf.Lerp(transform.position.y, transform.position.y -3, _lerpSpeed);

        transform.position = temp;
    }
    
    IEnumerator ChangeOpacity()
    {
        _heartColor.a = Mathf.MoveTowards(1, 0, 5f);
        yield return new WaitForSeconds(1);
    }
}
