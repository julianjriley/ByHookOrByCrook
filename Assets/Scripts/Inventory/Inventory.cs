using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [DEPRECATED] NO LONGER USED IN GAME MANAGER

[Serializable]
public class Inventory
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
    }
}
