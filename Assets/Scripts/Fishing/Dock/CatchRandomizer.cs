using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// Handles random selection of junk, weapon, attack buff, support buff, or movement buff when a fish is caught.
/// Accounts for casting and reeling score, bait type, and fishing rod upgrade tiers.
/// </summary>
public class CatchRandomizer : MonoBehaviour
{
    [Header("Randomization Parameters")]
    [SerializeField, Tooltip("Trash catch rate (percent chance) when you fail both tasks as badly as possible.")]
    private float _maxTrashRate;
    [SerializeField, Tooltip("Trash catch rate (percent chance) WITH ACCESSIBILITY BOBBER when you fail both tasks as badly as possible.")]
    private float _maxAccessibilityTrashRate;
    [SerializeField, Tooltip("Catch rate of a specific fish type, when the corresponding bait is being used.")]
    private float _specialBaitWeighting;

    [Header("Weapons")]
    [SerializeField, Tooltip("Weapons that can be fished up while player has tier 1 fishing rod.")]
    private Item[] _tier1Weapons;
    [SerializeField, Tooltip("Weapons added to the pool by having the tier 2 fishing rod.")]
    private Item[] _tier2Weapons;
    [SerializeField, Tooltip("Weapons added to the pool by having the tier 3 fishing rod.")]
    private Item[] _tier3Weapons;

    [Header("Attack Buffs")]
    [SerializeField, Tooltip("Attack buffs that can be fished up while player has tier 1 fishing rod.")]
    private Item[] _tier1Attack;
    [SerializeField, Tooltip("Attack buffs added to the pool by having the tier 2 fishing rod.")]
    private Item[] _tier2Attack;
    [SerializeField, Tooltip("Attack buffs added to the pool by having the tier 3 fishing rod.")]
    private Item[] _tier3Attack;

    [Header("Support Buffs")]
    [SerializeField, Tooltip("Support buffs that can be fished up while player has tier 1 fishing rod.")]
    private Item[] _tier1Support;
    [SerializeField, Tooltip("Support buffs added to the pool by having the tier 2 fishing rod.")]
    private Item[] _tier2Support;
    [SerializeField, Tooltip("Support buffs added to the pool by having the tier 3 fishing rod.")]
    private Item[] _tier3Support;

    [Header("Movement Buffs")]
    [SerializeField, Tooltip("Movement buffs that can be fished up while player has tier 1 fishing rod.")]
    private Item[] _tier1Movement;
    [SerializeField, Tooltip("Movement buffs added to the pool by having the tier 2 fishing rod.")]
    private Item[] _tier2Movement;
    [SerializeField, Tooltip("Movement buffs added to the pool by having the tier 3 fishing rod.")]
    private Item[] _tier3Movement;

    [Header("Junk")]
    [SerializeField, Tooltip("Total pool of junk items.")]
    private Item[] _junk;

    /// <summary>
    /// Handles randomization of caught fish and interfacing with inventory and bait list of game manager accordingly.
    /// </summary>
    /// <param name="score">0 to 1 value for proficiency on casting & reeling tasks (0 = worst fail, 1 = perfect success)</param>
    public void CatchFish(float score)
    {
        // determine whether to use accessibility values
        float maxRate = GameManager.Instance.GamePersistent.IsBobber ? _maxAccessibilityTrashRate : _maxTrashRate;

        // determine percent chance of catching trash based on performance
        // 0 score = max trash rate
        // 1 score = 0% trash rate
        float trashRate = math.remap(0, 1, maxRate, 0, score);
        Debug.Log("Rate: " + trashRate);
        // Determine Trash vs. Fish odds
        float random = UnityEngine.Random.Range(0f, 1f);
        // Junk
        if(random < trashRate)
        {
            CatchRandomJunk();
        }
        // Fish (any type)
        else
        {
            // Determine whether to ignore bait type
            random = UnityEngine.Random.Range(0f, 1f);

            // Ignore bait type
            if (random > _specialBaitWeighting)
            {
                CatchAnyItem();
            }
            // Catch based on bait type
            else
            {
                switch (GameManager.Instance.PeekBait())
                {
                    case GameManager.BaitType.Empty:
                        throw new System.Exception("static CatchFish() function of CatchRandomizer should ONLY be called if the player still has more bait.");
                    case GameManager.BaitType.Default:
                        CatchAnyItem();
                        break;
                    case GameManager.BaitType.Weapon:
                        CatchRandomFromLists(_tier1Weapons, _tier2Weapons, _tier3Weapons);
                        break;
                    case GameManager.BaitType.Attack:
                        CatchRandomFromLists(_tier1Attack, _tier2Attack, _tier3Attack);
                        break;
                    case GameManager.BaitType.Support:
                        CatchRandomFromLists(_tier1Support, _tier2Support, _tier3Support);
                        break;
                    case GameManager.BaitType.Movement:
                        CatchRandomFromLists(_tier1Movement, _tier2Movement, _tier3Movement);
                        break;
                }
            }
        }

        // Consume bait always
        GameManager.Instance.ConsumeBait();
    }

    /// <summary>
    /// Interfaces with game manager to add random junk item.
    /// Junk allows duplicates.
    /// </summary>
    private void CatchRandomJunk()
    {
        // select random junk item
        int rand = UnityEngine.Random.Range(0, _junk.Length);
        Item item = _junk[rand];
         
        // add item to inventory
        GameManager.Instance.AddCaughtFish(item);
    }

    /// <summary>
    /// Attempts to catch any item at all from the total pool, with no favoring of any particular type.
    /// Used by generic bait AND as a fallback when no more catches of a specific type remain.
    /// </summary>
    private void CatchAnyItem()
    {
        // all tier 1 items
        List<Item> tier1 = new List<Item>(_tier1Weapons);
        tier1.AddRange(_tier1Attack);
        tier1.AddRange(_tier1Support);
        tier1.AddRange(_tier1Movement);
        Item[] tier1List = tier1.ToArray();

        // all tier 2 items
        List<Item> tier2 = new List<Item>(_tier2Weapons);
        tier2.AddRange(_tier2Attack);
        tier2.AddRange(_tier2Support);
        tier2.AddRange(_tier2Movement);
        Item[] tier2List = tier2.ToArray();

        // all tier 3 items
        List<Item> tier3 = new List<Item>(_tier3Weapons);
        tier3.AddRange(_tier3Attack);
        tier3.AddRange(_tier3Support);
        tier3.AddRange(_tier3Movement);
        Item[] tier3List = tier3.ToArray();

        CatchRandomFromLists(tier1List, tier2List, tier3List, true);
    }

    /// <summary>
    /// General item randomization function that picks a random item from input lists.
    /// Accounts for rod upgrade level to determine item pool.
    /// Prevents adding of duplicate items; If no more unique items detected, try to catch a different item type - otherwise junk.
    /// </summary>
    private void CatchRandomFromLists(Item[] tier1Items, Item[] tier2Items, Item[] tier3Items, bool includesAll = false)
    {
        // Determine list of items to randomize within
        List<Item> items = new List<Item>(tier1Items);
        if (GameManager.Instance.GamePersistent.RodLevel >= 2)
            items.AddRange(tier2Items);
        if (GameManager.Instance.GamePersistent.RodLevel >= 3)
            items.AddRange(tier3Items);

        // Prevent infinite loop if player already has ALL possible items of this type
        while(items.Count > 0)
        {
            // select random item
            int rand = UnityEngine.Random.Range(0, items.Count);
            Item item = items[rand];

            // add item and end execution if a unique item is selected
            if (GameManager.Instance.IsNewCatch(item))
            {
                GameManager.Instance.AddCaughtFish(item);
                return; // catch randomizer complete
            }
            else
                // remove dupe from options
                items.Remove(item);
        }

        // no more of any item remain - so just catch some junk
        if (includesAll)
            CatchRandomJunk();
        // no more items of the selected type remain - so catch some other item instead
        else
            CatchAnyItem();
    }
}
