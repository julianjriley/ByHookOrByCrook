using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public List<Item> items = new List<Item>();

    public void AddIten(Item item)
    {
        items.Add(item);
    }
}
