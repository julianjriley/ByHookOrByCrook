using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenuAttribute(fileName ="New item", menuName ="Assets/Item")]
public class Item : ScriptableObject
{
    [SerializeField] protected int _itemID;
    [SerializeField] protected string _itemName;
    [SerializeField] protected string _itemDescription;
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected GameObject _itemPrefab;
    

    public int GetItemID()
    {
        return _itemID;
    }

    public string GetItemName()
    {
        return _itemName;
    }

    public string GetItemDescription()
    {
        return _itemDescription;
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }

}
