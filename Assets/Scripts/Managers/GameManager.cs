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