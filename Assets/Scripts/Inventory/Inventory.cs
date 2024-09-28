using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    // TODO: implement .Equals() function for Inventory class so that it compares data (not reference).
    // Necessary for the IsItemUnique(Item) function to work properly!!
    // this may not be required depending on how out references to scriptable objects are being handled (i.e. references vs. value copies).

    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsNewItem(Item item)
    {
        // check for duplicates
        foreach (Item curr in items)
        {
            if (curr.Equals(item))
                return false;
        }

        // item is unique
        return true;
    }
}
