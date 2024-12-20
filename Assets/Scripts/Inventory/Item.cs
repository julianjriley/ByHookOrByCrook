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
    [SerializeField] protected int _cost;

    protected PlayerCombat _player;

    public enum ItemType { ATTACK = 0, SUPPORT = 1, MOVEMENT = 2, WEAPON = 3, JUNK = 4 };
    public ItemType itemType;


    public ItemType GetItemType()
    {
        return itemType;
    }

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

    public int GetCost()
    {
        return _cost;
    }


    public virtual void SetPlayer(PlayerCombat player)
    {
        _player = player;
    }

    public PlayerCombat GetPlayer()
    {
        return _player;
    }

}
