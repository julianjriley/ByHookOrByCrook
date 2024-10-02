using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bait : MonoBehaviour
{
    private string _baitType;
    public Image _sprite;
    public BaitSelector baitSelector;

    private List<Sprite> _spriteList;
    void Start()
    {
        baitSelector = FindObjectOfType<BaitSelector>();
        _spriteList = baitSelector._baitSprites;
    }

    public void AssignBaitType(string barrelType)
    {
        if (_baitType == "Basic Bait")
        {
            _sprite.sprite = _spriteList[0];
        }
        else if (_baitType == "Weapon Bait")
        {
            _sprite.sprite = _spriteList[1];
        }
        else if (_baitType == "Attack Bait")
        {
            _sprite.sprite = _spriteList[2];
        }
        else if (_baitType == "Support Bait")
        {
            _sprite.sprite = _spriteList[3];
        }
        else if (_baitType == "Movement Bait")
        {
            _sprite.sprite = _spriteList[4];
        }
    }

    // Function to click on bait and despawn it
    public void PutBack()
    {
        baitSelector.increaseBaitSlot();
        StartCoroutine(Waiting(this.gameObject));
    }

    IEnumerator Waiting(GameObject _baitGameObject)
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        Destroy(_baitGameObject);
    }
}
