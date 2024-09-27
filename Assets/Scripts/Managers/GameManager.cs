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
    // Bait, Inventory, Loadout, etc. (saved between scenes)
    [Serializable]
    public class ScenePersistentData
    {
        public Inventory CaughtFish;   
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
        ScenePersistentData newInventory = new ScenePersistentData();

        // TODO: INITIALIZE DEFAULT VALUES FOR INVENTORY DATA

        // Apply reset/initialized Inventory data to Instance
        Instance.ScenePersistent = newInventory;
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
        public int RodNumber;           // What # rod the player is on (0, 1, 2 - starter, upgrade 1, etc...)
        public bool AttackBait;         // Booleans for each bait type
        public bool MovementBait;       // Not a list for ease of access at point of sale / hub shops
        public bool SupportBait;
        public bool WeaponBait;

        // Hub related stats
        public List<bool> IsConvoHad; // TODO: Each NPC will need one of these lists.
    }

    // private stored save data
    private GamePersistentData _gameData;

    // public accessor for save data
    public GamePersistentData SaveData
    {
        get
        {
            // initialize if necessary and possible
            if (_gameData == null)
            {
                InitializeSaveData();
            }

            return _gameData;
        }
        private set
        {
            _gameData = value;
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
        newSaveData.BossNumber = 0;
        newSaveData.LossCounter = 3;

        newSaveData.Gill = 0;
        newSaveData.BaitInventorySize = 3;
        newSaveData.BattleInventorySize = 3;
        newSaveData.RodNumber = 0;
        newSaveData.AttackBait = false;
        newSaveData.MovementBait = false;
        newSaveData.SupportBait = false;
        newSaveData.WeaponBait = false;

        newSaveData.IsConvoHad = new List<bool>();

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
        Instance.SaveData = newSaveData;
    }

    private void OnApplicationQuit()
    {
        // TODO: SAVE PersistentData to PlayerPrefs

        /*****************************************************************
        // JSON functionality. To be replaced with PlayerPrefs

        string json = JsonUtility.ToJson(SaveData);
        File.WriteAllText(Application.persistentDataPath + "\\savedata.json", json);
        *****************************************************************/
    }
    #endregion
}