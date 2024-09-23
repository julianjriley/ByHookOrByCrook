using System;
using System.Collections;
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

    #region INVENTORY DATA
    // Inventory Data (saved between scenes)
    [System.Serializable]
    public class InventoryData
    {
        // STORE ALL BETWEEN SCENES DATA HERE
        // i.e. INVENTORY
        // also for bait data (for between bait selection->fishing)?
    }

    // private stored inventory
    private InventoryData _inventory;

    // public accessor of inventory
    public InventoryData Inventory
    {
        get
        {
            // initialize if necessary and possible
            if (_inventory == null)
            {
                ResetInventory();
            }
            // return new/existing inventory
            return _inventory;
        }
        private set
        {
            _inventory = value;
        }
    }

    /// <summary>
    /// initializes base stats of inventory.
    /// Used to reset inventory between runs.
    /// </summary>
    public void ResetInventory()
    {
        InventoryData newInventory = new InventoryData();

        // TODO: INITIALIZE DEFAULT VALUES FOR INVENTORY DATA

        // Apply reset/initialized Inventory data to Instance
        Instance.Inventory = newInventory;
    }
    #endregion

    #region SAVE DATA
    // save data (saved between sessions)
    [System.Serializable]
    public class PersistentData
    {
        // TODO: ADD SAVE DATA HERE
        // i.e. UPGRADES, CURRENCY (ECONOMY), SETTINGS
        // Boss-related stats
        public int bossNumber;  // Whether the player is on the first, second, or third boss (0, 1, 2)
        public int lossCounter; // How many times you've lost to a boss (resets on victory)
    }

    // private stored save data
    private PersistentData _saveData;

    // public accessor for save data
    public PersistentData SaveData
    {
        get
        {
            // initialize if necessary and possible
            if (_saveData == null)
            {
                InitializeSaveData();
            }

            return _saveData;
        }
        private set
        {
            _saveData = value;
        }
    }

    /// <summary>
    /// initializes base stats of save data (used for first time playing).
    /// Used both for reading existing save data AND for creating new save data if none is found.
    /// </summary>
    private void InitializeSaveData()
    {
        // initialize and load save data
        PersistentData newSaveData = new PersistentData();

        // TODO: INITIALIZE DEFAULT VALUES FOR SAVE DATA
        // default data in case player prefs not found
        newSaveData.bossNumber = 0;
        newSaveData.lossCounter = 0;

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