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
        Empty,
        Default,
        Weapon,
        Attack,
        Support,
        Movement
    }

    // Bait, Inventory, Loadout, etc. (saved between scenes)
    [Serializable]
    public class ScenePersistentData
    {
        public Inventory CaughtFish;   

        public List<BaitType> BaitList;
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

        // Initialize default values for scene persistent data
        newInventory.BaitList = new List<BaitType>();

        // Apply reset/initialized Inventory data to Instance
        Instance.ScenePersistent = newInventory;
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
    /// Adds new bait item to the end of the bait list.
    /// </summary>
    public void AddBait(BaitType newBait)
    {
        ScenePersistent.BaitList.Add(newBait);
    }
    #endregion

    #region GAME PERSISTENT DATA
    // permanent upgrades, settings, etc. (saved between sessions)
    [Serializable]
    public class GamePersistentData
    {
        // TODO: ADD SAVE DATA HERE
        // i.e. UPGRADES, CURRENCY (ECONOMY), SETTINGS
        public int RodLevel;
        // Boss-related stats
        public int bossNumber;  // Whether the player is on the first, second, or third boss (0, 1, 2)
        public int lossCounter; // How many times you've lost to a boss (resets on victory)

        // Hub related stats
        public List<bool> IsConvoHad;
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
        newSaveData.RodLevel = 1;
        newSaveData.bossNumber = 0;
        newSaveData.lossCounter = 3;
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
        Instance.GamePersistent = newSaveData;
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