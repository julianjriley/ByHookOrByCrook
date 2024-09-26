using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// Handles random selection of junk, weapon, attack buff, support buff, or movement buff when a fish is caught.
/// Accounts for casting and reeling scores, bait type, and fishing rod upgrade tiers.
/// </summary>
public class CatchRandomizer : MonoBehaviour
{
    [Header("Randomization Parameters")]
    [SerializeField, Tooltip("Trash catch rate (percent chance) when you fail both tasks as badly as possible.")]
    private float _maxTrashRate;
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
    /// <param name="castScore">0 to 1 value for proficiency on casting task (0 = worst fail, 1 = perfect success)</param>
    /// <param name="reelScore">0 to 1 value for proficiency on reeling task (0 = worst fail, 1 = perfect success)</param>
    public void CatchFish(float castScore, float reelScore)
    {
        // average cast/reel scores (equally weighted)
        float combinedScore = (castScore + reelScore) / 2f;

        // determine percent chance of catching trash based on performance
        // 0 score = max trash rate
        // 1 score = 0% trash rate
        float trashRate = math.remap(0, 1, _maxTrashRate, 0, combinedScore);

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
            switch(GameManager.Instance.PeekBait())
            {
                case GameManager.BaitType.Empty:
                    throw new System.Exception("static CatchFish() function of CatchRandomizer should ONLY be called if the player still has more bait.");
                case GameManager.BaitType.Default:

                    // randomly select one of four buff types
                    random = UnityEngine.Random.Range(0f, 1f);
                    if (random < 0.25f)
                        CatchRandomWeapon();
                    else if (random < 0.5f)
                        CatchRandomAttack();
                    else if (random < 0.75f)
                        CatchRandomSupport();
                    else
                        CatchRandomMovement();

                    break;
                case GameManager.BaitType.Weapon:
                    CatchRandomWeapon();
                    break;
                case GameManager.BaitType.Attack:
                    CatchRandomAttack();
                    break;
                case GameManager.BaitType.Support:
                    CatchRandomSupport();
                    break;
                case GameManager.BaitType.Movement:
                    CatchRandomMovement();
                    break;
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
        GameManager.Instance.ScenePersistent.CaughtFish.AddItem(item);
    }

    /// <summary>
    /// Interfaces with game manager to add random weapon item.
    /// </summary>
    private void CatchRandomWeapon()
    {
        CatchRandomItem(_tier1Weapons, _tier2Weapons, _tier3Weapons);
    }

    /// <summary>
    /// Interfaces with game manager to add random attack buff item.
    /// </summary>
    private void CatchRandomAttack()
    {
        CatchRandomItem(_tier1Attack, _tier2Attack, _tier3Attack);
    }

    /// <summary>
    /// Interfaces with game manager to add random support buff item.
    /// </summary>
    private void CatchRandomSupport()
    {
        CatchRandomItem(_tier1Support, _tier2Support, _tier3Support);
    }

    /// <summary>
    /// Interfaces with game manager to add random movement buff item.
    /// </summary>
    private void CatchRandomMovement()
    {
        CatchRandomItem(_tier1Movement, _tier2Movement, _tier3Movement);
    }

    /// <summary>
    /// General item randomization function called by specific CatchRandom[Type]() functions.
    /// Requires three tiers of item lists as inputs.
    /// Prevents adding of duplicate items; If no more unique items detected, player will catch junk instead.
    /// </summary>
    private void CatchRandomItem(Item[] tier1Items, Item[] tier2Items, Item[] tier3Items)
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
            if (GameManager.Instance.ScenePersistent.CaughtFish.IsNewItem(item))
            {
                GameManager.Instance.ScenePersistent.CaughtFish.AddItem(item);
                return; // catch randomizer complete
            }
            else
                // remove dupe from options
                items.Remove(item);
        }

        // No more unique items of this type remain. Just give them junk.
        CatchRandomJunk();
    }
}
