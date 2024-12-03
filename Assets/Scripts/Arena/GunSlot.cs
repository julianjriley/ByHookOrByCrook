using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    [SerializeField] Image _gunImage;
    [SerializeField] RectTransform _overheat;
    WeaponInstance _weapon;

    private void Update()
    {
        if (_weapon != null)
            _overheat.sizeDelta = new Vector2(100, _weapon.GetHeatLevel());
    }

    public void AssignWeapon(WeaponInstance weapon)
    {
        _weapon = weapon;
        _gunImage.sprite = _weapon.GetWeapon().GetSprite();
    }

    public void Grow()
    {
        transform.localScale = Vector3.one;
    }

    public void EnSmallen()
    {
        transform.localScale = Vector3.one / 1.5f;
    }
}
