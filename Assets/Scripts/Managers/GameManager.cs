using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and manages player data saved between scenes.
/// Stores and manages save data saved between sessions.
/// </summary>
public class GameManager : MonoBehaviour
{
    // private singleton instance
    private static GameManager _instance;

    // public accessor of instance
    public static GameManager Instance
    {
        get
        {
            // setup GameManager as a singleton class
            if (_instance == null)
            {
                // create new game manager object
                GameObject newManager = new();
                newManager.name = "Game Manager";
                newManager.AddComponent<GameManager>();
                DontDestroyOnLoad(newManager);
                _instance = newManager.GetComponent<GameManager>();
            }
            // return new/existing instance
            return _instance;
        }
    }

    #region SCENE PERSISTENT DATA
    // used for storing scene persistent BaitType data
    public enum BaitType
    {
        Empty = -1,
        Default = 0,
        Weapon = 1,
        Attack = 2,
        Support = 3,
        Movement = 4
    }

    // Bait, Inventory, Loadout, etc. (saved between scenes)
    [Serializable]
    public class ScenePersistentData
    { 
        public List<BaitType> BaitList;
        public List<Item> CaughtFish;
        public List<Item> Loadout;
        public float BossPerformanceMultiplier;
    }

    // private stored inventory
    private ScenePersistentData _scenePersistent;

    // public accessor of inventory
    public ScenePersistentData ScenePersistent
    {
        get
        {
            // initialize if necessary and possible
            if (_scenePersistent == null)
            {
                ResetScenePersistentData();
            }
            // return new/existing inventory
            return _scenePersistent;
        }
        private set
        {
            _scenePersistent = value;
        }
    }

    /// <summary>
    /// initializes base stats of inventory.
    /// Used to reset inventory between runs.
    /// </summary>
    public void ResetScenePersistentData()
    {
        ScenePersistentData newScenePersistent = new ScenePersistentData();

        //Reset All Gun Stats
        if(_scenePersistent is not null) // ensure no issues on first loop or in editor
        {
            foreach (Item item in _scenePersistent.Loadout)
            {
                if (item is Weapon)
                    (item as Weapon).ResetStats();
            }
        }

        // Initialize default values for scene persistent data
        newScenePersistent.BaitList = new List<BaitType>();
        newScenePersistent.CaughtFish = new List<Item>();
        newScenePersistent.Loadout = new List<Item>();
        newScenePersistent.BossPerformanceMultiplier = 1;

        // Apply reset/initialized Inventory data to Instance
        Instance.ScenePersistent = newScenePersistent;
    }

    /// <summary>
    /// Returns the type of bait that is queued up next in the bait list.
    /// Returns BaitType.Empty if the list is empty.
    /// </summary>
    public BaitType PeekBait()
    {
        // return last bait in list
        if (ScenePersistent.BaitList.Count >= 1)
            return ScenePersistent.BaitList[ScenePersistent.BaitList.Count - 1];
        // bait list is empty
        else
            return BaitType.Empty;
    }

    /// <summary>
    /// Removes last bait item from the bait list.
    /// </summary>
    public void ConsumeBait()
    {
        if (ScenePersistent.BaitList.Count >= 1)
            ScenePersistent.BaitList.RemoveAt(ScenePersistent.BaitList.Count - 1);
        else
            throw new Exception("Invalid use of ConsumeBait() in GameManager. There is no bait to remove.");
    }

    /// <summary>
    /// Adds new bait type to the end of the bait list.
    /// </summary>
    public void AddBait(BaitType newBait)
    {
        ScenePersistent.BaitList.Add(newBait);
    }

    /// <summary>
    /// Returns item present on the top of the CaughtFish list (most recent).
    /// </summary>
    /// <returns></returns>
    public Item PeekCaughtFish()
    {
        return ScenePersistent.CaughtFish[ScenePersistent.CaughtFish.Count - 1];
    }

    /// <summary>
    /// Adds new fish item to the end of the caught fish list.
    /// </summary>
    public void AddCaughtFish(Item item)
    {
        ScenePersistent.CaughtFish.Add(item);
    }

    /// <summary>
    /// Returns true if newItem is NOT already contained in the CaughtFish list.
    /// </summary>
    public bool IsNewCatch(Item newItem)
    {
        // check for duplicates
        foreach (Item curr in ScenePersistent.CaughtFish)
        {
            if (curr.GetItemID() == newItem.GetItemID())
                return false;
        }

        // item is unique
        return true;
    }

    /// <summary>
    /// Adds new fish item to the end of the loadout list
    /// </summary>
    public void AddLoadoutItem(Item item)
    {
        ScenePersistent.Loadout.Add(item);
    }
    #endregion

    #region GAME PERSISTENT DATA
    // permanent upgrades, settings, etc. (saved between sessions)
    [Serializable]
    public class GamePersistentData
    {
        // TODO: ADD SAVE DATA HERE
        // i.e. UPGRADES, CURRENCY (ECONOMY), SETTINGS
        
        // Boss-related stats
        public int BossNumber;  // Whether the player is on the first, second, or third boss (0, 1, 2)
        public int LossCounter; // How many times you've lost to a boss (resets on victory)

        // Money
        public int Gill; // How much $ the player has

        // Upgrades
        public int BaitInventorySize;   // Current sizes of inventories
        public int BattleInventorySize;
        public int RodLevel;            // What # rod the player is on (0, 1, 2 - starter, upgrade 1, etc...)
        public bool AttackBait;         // Booleans for each bait type
        public bool MovementBait;       // Not a list for ease of access at point of sale / hub shops
        public bool SupportBait;
        public bool WeaponBait;

        // Accessibility
        public bool IsSkipper;
        public bool IsBobber;

        // Hub related stats
        public List<bool> IsConvoHadRod;
        public List<bool> IsConvoHadBait;
        public List<bool> IsConvoHadBag;

        // Accessibility Settings
        public CustomCrosshair Crosshair;
        public Sprite crosshairSprite;
    }

    // private stored save data
    private GamePersistentData _gamePersistent;

    // public accessor for save data
    public GamePersistentData GamePersistent
    {
        get
        {
            // initialize if necessary and possible
            if (_gamePersistent == null)
            {
                InitializeSaveData();
            }

            return _gamePersistent;
        }
        private set
        {
            _gamePersistent = value;
        }
    }

    /// <summary>
    /// initializes base stats of save data (used for first time playing).
    /// Used both for reading existing save data AND for creating new save data if none is found.
    /// </summary>
    private void InitializeSaveData()
    {
        // initialize and load save data
        GamePersistentData newSaveData = new GamePersistentData();

        // TODO: INITIALIZE DEFAULT VALUES FOR SAVE DATA
        // default data in case player prefs not found
        string filePath = Application.persistentDataPath + "/GameData.json";
        if (System.IO.File.Exists(filePath))
        {
            string saveData = System.IO.File.ReadAllText(filePath);
            newSaveData = JsonUtility.FromJson<GamePersistentData>(saveData);
            Instance.GamePersistent = newSaveData;
            return;
        }
        
        newSaveData.BossNumber = 0;
        newSaveData.LossCounter = 3;

        newSaveData.Gill = 0;
        newSaveData.BaitInventorySize = 3;
        newSaveData.BattleInventorySize = 2;
        newSaveData.RodLevel = 0;
        newSaveData.AttackBait = false;
        newSaveData.MovementBait = false;
        newSaveData.SupportBait = false;
        newSaveData.WeaponBait = false; 

        newSaveData.IsSkipper = false;
        newSaveData.IsBobber = false;

        newSaveData.IsConvoHadRod = new List<bool>();
        newSaveData.IsConvoHadBait = new List<bool>();
        newSaveData.IsConvoHadBag = new List<bool>();

        // TODO: read existing save data (if it exists) from PlayerPrefs

        /*****************************************************************
        // JSON functionality. To be replaced with PlayerPrefs

        string path = Application.persistentDataPath + "\\savedata.json";
        if (File.Exists(path))
        {
            // read json file into data object
            string json = File.ReadAllText(path);
            newSaveData = JsonUtility.FromJson<PersistentData>(json);
        }
        *****************************************************************/

        // Apply read/initialized data to instance
        Instance.GamePersistent = newSaveData;
    }

    private void OnApplicationQuit()
    {
        // TODO: SAVE PersistentData to PlayerPrefs
        string saveData = JsonUtility.ToJson(GamePersistent);
        string filePath = Application.persistentDataPath + "/GameData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, saveData);
        /*****************************************************************
        // JSON functionality. To be replaced with PlayerPrefs

        string json = JsonUtility.ToJson(SaveData);
        File.WriteAllText(Application.persistentDataPath + "\\savedata.json", json);
        *****************************************************************/
    }
    #endregion

 
}