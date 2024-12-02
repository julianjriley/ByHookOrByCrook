using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    [SerializeField] Image _gunImage;
    [SerializeField] RectTransform _overheat;
    [SerializeField] Image _overheatBarImage;
    WeaponInstance _weapon;

    [SerializeField] Color _heatingUpColor;
    [SerializeField] Color _overheatedColor;
    [SerializeField] GameObject _overheatedImage;
    private void Update()
    {
        if (_weapon != null)
        {
            _overheat.sizeDelta = new Vector2(100, _weapon.GetHeatLevel());
            if(_weapon.GetOverHeatedState())
            {
                _overheatBarImage.color = _overheatedColor;
                if(!_overheatedImage.activeSelf)
                    _overheatedImage.SetActive(true);
            }
            else
            {
                _overheatBarImage.color = _heatingUpColor;
                if (_overheatedImage.activeSelf)
                    _overheatedImage.SetActive(false);
            }

        }
            

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
        transform.localScale = Vector3.one / 2;
    }
}
