using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PassiveItemInstance : MonoBehaviour
{
    [SerializeField] protected PassiveItem _passiveItem;
    protected PlayerCombat _player;

    public virtual void ItemEffect()
    {

    }

    protected virtual void Start()
    {
        _player = _passiveItem.GetPlayer();
        StartCoroutine(WaitForApplication());
    }

    IEnumerator WaitForApplication()
    {
        yield return new WaitForSeconds(0.2f);
        ItemEffect();
    }


}
