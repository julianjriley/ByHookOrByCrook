using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Bait : MonoBehaviour
{
    private string _baitType;
    private Image _sprite;
    private BaitSelector _baitSelector;
    private List<Sprite> _spriteList;
 
    public void AssignBaitType(string barrelType)
    {
        // Updates the bait icons

        _baitSelector = FindObjectOfType<BaitSelector>();
        _sprite = gameObject.GetComponent<Image>();
        _baitType = barrelType;

        if (_baitType == "Basic Bait")
        {
            _sprite.sprite = _baitSelector._baitSprites[0];
        }
        else if (_baitType == "Weapon Bait")
        {
            _sprite.sprite = _baitSelector._baitSprites[1];
        }
        else if (_baitType == "Attack Bait")
        {
            _sprite.sprite = _baitSelector._baitSprites[2];
        }
        else if (_baitType == "Support Bait")
        {
            _sprite.sprite = _baitSelector._baitSprites[3];
        }
        else if (_baitType == "Movement Bait")
        {
            _sprite.sprite = _baitSelector._baitSprites[4];
        }
    }

    public void PutBack(int type)
    {
        // Function to click on bait, despawn it, and update scene persistent data

        GameManager.Instance.ScenePersistent.BaitList.Remove((GameManager.BaitType) type);
        _baitSelector.increaseBaitSlot();
        StartCoroutine(Waiting(this.gameObject));
    }

    IEnumerator Waiting(GameObject _baitGameObject)
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        Destroy(_baitGameObject);
    }
}
