using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameManager;

/// <summary>
/// Handles creation and incremental removal of bait icons.
/// </summary>
public class RemainingBaitUI : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab used to instantiate each bait icon")]
    private GameObject _remainingBaitPrefab;
    [SerializeField, Tooltip("List of bait sprites for the possible types (in order)")]
    private List<Sprite> _baitSprites = new List<Sprite>();
    [SerializeField, Tooltip("List of icon sprites for the possible types (in order)")]
    private List<Sprite> _iconSprites = new List<Sprite>();

    private int baitLeft;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize to starting size of bait list
        baitLeft = GameManager.Instance.ScenePersistent.BaitList.Count;

#if UNITY_EDITOR
        // Allows testing even if you start testing in fishing scene.
        if(baitLeft == 0)
        {
            baitLeft = 5;
            GameManager.Instance.AddBait(BaitType.Default);
            GameManager.Instance.AddBait(BaitType.Weapon);
            GameManager.Instance.AddBait(BaitType.Attack);
            GameManager.Instance.AddBait(BaitType.Support);
            GameManager.Instance.AddBait(BaitType.Movement);
        }
#endif

        // Add a UI sprite for each bait in the bait list
        foreach (BaitType baitType in GameManager.Instance.ScenePersistent.BaitList)
        {
            GameObject newIcon = Instantiate(_remainingBaitPrefab, transform);

            // REQUIREMENT: Icon Prefab MUST have Image Component on child 0.
            if (!newIcon.transform.GetChild(0).TryGetComponent(out Image baitImg))
                throw new System.Exception("Remaining Bait Icon prefab MUST have Image component.");
            // REQUIREMENT: Icon Prefab MUST have Image Component on child 1.
            if (!newIcon.transform.GetChild(1).TryGetComponent(out Image iconImg))
                throw new System.Exception("Remaining Bait Icon prefab MUST have Image component.");

            // set proper bait sprite
            baitImg.sprite = _baitSprites[(int)baitType];
            iconImg.sprite = _iconSprites[(int)baitType];
            // check if no icon needed
            if (baitType == BaitType.Default) iconImg.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if a change needs to be made to UI
        if (GameManager.Instance.ScenePersistent.BaitList.Count < baitLeft)
        {
            baitLeft--;

            // Remove bait from the end of the list
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }

    public int GetBaitLeft()
    {
        return baitLeft;
    }
}
