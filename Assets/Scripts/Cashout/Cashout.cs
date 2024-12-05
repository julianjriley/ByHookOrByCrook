using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles all initialization and functionality of cashout scene.
/// </summary>
public class Cashout : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("Prefab used to spawn fish that were caught during fishing.")]
    private GameObject _fishDisplayPrefab;
    [SerializeField, Tooltip("Transform where fish displays are created under")]
    private Transform _fishContentsTransform;
    [SerializeField, Tooltip("Text for setting summary amount (before multiplier).")]
    private TMP_Text _summaryText;
    [SerializeField, Tooltip("Text for setting the boss bounter multiplier amount.")]
    private TMP_Text _bossBountyText;
    [SerializeField, Tooltip("Text for setting the total amount (after multiplier).")]
    private TMP_Text _totalText;
    [SerializeField, Tooltip("Used to call scene transitions.")]
    private SceneTransitionsHandler _transitionsHandler;

    [Header("Editor Only")]
    [SerializeField, Tooltip("Items to use for testing if starting unity in cashout scene.")]
    private List<Item> _editorFish;

    // Start is called before the first frame update
    void Start()
    {
        int summary = 0;

        // Initialize cashout fish displays for caught fish
        foreach (Item item in GameManager.Instance.ScenePersistent.CaughtFish)
        {
            CreateCashoutFishDisplay(item);
            
            summary += item.GetCost();
        }

#if UNITY_EDITOR
        // Initialize testing fish without going through other scenes
        if (GameManager.Instance.ScenePersistent.CaughtFish.Count == 0) // only use editor fish if no fish are found in caught fish list.
        {
            foreach (Item item in _editorFish)
            {
                CreateCashoutFishDisplay(item);

                summary += item.GetCost();
            }
        }
#endif

        // initialize summary text
        _summaryText.text = summary.ToString();

        // initialize boss bounty text
        float bossBounty = GameManager.Instance.ScenePersistent.BossPerformanceMultiplier;
        _bossBountyText.text = "x " + bossBounty.ToString("F2");

        // initialize total text
        int total = Mathf.RoundToInt(summary * (Mathf.Round(bossBounty * 100f) / 100f));
        _totalText.text = total.ToString();

        // increment actual saved currency
        GameManager.Instance.GamePersistent.Gill += total;

        // For the NPCs
        if (bossBounty < 2)
        {
            GameManager.Instance.GamePersistent.LossCounter += 1;
        }
        else
        {
            GameManager.Instance.GamePersistent.LossCounter = 0;
        }
    }

    /// <summary>
    /// Handles initialization of a single cashout fish display based on provided item
    /// </summary>
    private void CreateCashoutFishDisplay(Item item)
    {
        // instantiate prefab
        GameObject newFishDisplay = Instantiate(_fishDisplayPrefab, _fishContentsTransform);

        // initialize values of new instance
        if (!newFishDisplay.TryGetComponent(out CashoutFishDisplay fishDisplay))
            throw new System.Exception("Cashout fish display prefab MUST have CashoutFishDisplay component.");
        fishDisplay.Initialize(item.GetItemName(), item.GetCost().ToString(), item.GetSprite());
    }

    public void GoToHub(string hubSceneName)
    {
        // Clear all scene persistent data (bait, caught fish, loadout) for next run
        GameManager.Instance.ResetScenePersistentData();

        // actually load scene
        _transitionsHandler.LoadScene(hubSceneName);
    }
}
