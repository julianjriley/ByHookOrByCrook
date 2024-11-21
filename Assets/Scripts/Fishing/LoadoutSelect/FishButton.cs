using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains all functionality relating to an individual FishButton.
/// On hover, tooltip shows.
/// On button press, swaps which list button is contained in.
/// </summary>
public class FishButton : MonoBehaviour
{
    private static int _selectedCount;

    private LoadoutSelection _loadoutSelection;

    [HideInInspector]
    public Item Item;
    [SerializeField, Tooltip("Used for updating the sprite to match the selected fish item.")]
    private Image _sprite;
    [SerializeField, Tooltip("to update the icon sprite")]
    private Image _iconSprite;
    [SerializeField, Tooltip("The sprites to choose from for the icons")]
    private Sprite[] _iconSprites;

    [Header("Tooltip")]
    [SerializeField, Tooltip("Used to enable/disable tooltip popup")]
    private GameObject _tooltipObject;
    [SerializeField, Tooltip("Used for setting item name in tooltip.")]
    private TextMeshProUGUI _itemName;
    [SerializeField, Tooltip("Used for setting item description in tooltip.")]
    private TextMeshProUGUI _itemDescription;

    private void Start()
    {
        _selectedCount = 0;

        _loadoutSelection= FindObjectOfType<LoadoutSelection>();
    }

    /// <summary>
    /// Assigns the fish properties to the properties that appear in the scene.
    /// </summary>
    public void Initialize(Item fishItem)
    {
        Item = fishItem;

        _itemName.text = fishItem.GetItemName(); 
        _itemDescription.text = fishItem.GetItemDescription();
        _sprite.sprite = fishItem.GetSprite();
        _iconSprite.sprite = _iconSprites[(int) fishItem.GetItemType()];
    }

    /// <summary>
    /// Used to enable tooltip when hovering over button
    /// </summary>
    public void OnHover()
    {
        _tooltipObject.SetActive(true);
    }

    /// <summary>
    /// Used to disable tooltip when no longer hovering over button.
    /// </summary>
    public void OnExit()
    {
        _tooltipObject.SetActive(false);
    }

    /// <summary>
    /// Swaps the fish button EITHER from caught list to loadout list OR from loadout list to caught list.
    /// </summary>
    public void SwapFishButton()
    {
        // Swap from caught fish to loadout
        if(transform.parent.transform == _loadoutSelection.CaughtFishParent.transform)
        {
            // ensure loadout is not already full
            if(_selectedCount < GameManager.Instance.GamePersistent.BattleInventorySize)
            {
                transform.SetParent(_loadoutSelection.LoadoutFishParent.transform);

                _selectedCount++;

                // hide tooltip
                _tooltipObject.SetActive(false);
            }
            else
            {
                // TODO: negative feedback that you cannot add anymore items to the loadout since it is full
            }
        }
        // Swap from loadout to caught fish
        else
        {
            transform.SetParent(_loadoutSelection.CaughtFishParent.transform);

            _selectedCount--;

            // hide tooltip
            _tooltipObject.SetActive(false);
        }

        // cancel confirmation popups since a different button was pressed.
        _loadoutSelection.CancelCombatConfirmationPopup();
        _loadoutSelection.CancelPracticeConfirmationPopup();
    }
}
