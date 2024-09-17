using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutSelection : MonoBehaviour
{
    // This script will be used for both the Bait Selection Screen and Weapon Loadout Screen


   
    void Start()
    {
        // get reference from game manager to access
        //      1. list of catches from fishing minigame
        //      2. list of catches you choose to bring into battle
    }

    void Update()
    {
        /// If we are on bait selection scene, run bait stuff 
        ///     5 barrels of bait-- each are a button the player clicks on
        ///     clicking an UNLOCKED barrel puts bait icon into bait slots
        ///         ON CLICK: call a function that indicates what goes into bait slots
        ///     display CONTINUE
        /// 

        /// If we are on weapon loadout scene, run loadout stuff
        ///     shows all items the player has caught-- each are a button the player clicks on
        ///         determined by how much bait the player casts
        ///     clicking on an item puts it into your loadout slots-- determined by backpack upgrade
        ///         ON HOVER: display description+effect in a pop-up textbox below the item
        ///         ON CLICK: call a function that greys out the item AND move it into the correct slot
        ///     display PRACTICE? or FIGHT!    


    }
}
