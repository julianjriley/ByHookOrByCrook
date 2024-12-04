using FMODUnity;
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
    [SerializeField, Tooltip("Used to trigger shake animation negative feedback")]
    private Animator _anim;

    [Header("Tooltip")]
    [SerializeField, Tooltip("Used to enable/disable tooltip popup")]
    private GameObject _tooltipObject;
    [SerializeField, Tooltip("Used for setting item name in tooltip.")]
    private TextMeshProUGUI _itemName;
    [SerializeField, Tooltip("Used for setting item practical description in tooltip.")]
    private TextMeshProUGUI _practicalDescription;
    [SerializeField, Tooltip("Used for setting item fun description in tooltip.")]
    private TextMeshProUGUI _funDescription;

    [Header("Sounds")]
    [SerializeField] EventReference select;
    [SerializeField] EventReference deselect;
    [SerializeField] EventReference tooFull;

    private int _itemType;

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

        string[] itemDescription = fishItem.GetItemDescription().Split('|');
        _practicalDescription.text = itemDescription[0];
        if (itemDescription.Length > 1) // avoid error for improperly formatted item descriptions
            _funDescription.text = itemDescription[1];

        _sprite.sprite = fishItem.GetSprite();
        _itemType = (int)fishItem.GetItemType();
        _iconSprite.sprite = _iconSprites[_itemType];
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

                // play add to loadout audio
                SoundManager.Instance.PlayOneShot(select, gameObject.transform.position);

                // if a gun -> unlock bear's first fish toggle
                if (_itemType == 3) // weapon is 3
                {
                    _loadoutSelection.NumOfWeaponsPicked++;
                    _loadoutSelection.EnablePlushToggle();
                }
            }
            else
            {
                _anim.SetTrigger("Shake");

                // play negative feedback audio
                SoundManager.Instance.PlayOneShot(tooFull, gameObject.transform.position);
            }
        }
        // Swap from loadout to caught fish
        else
        {
            transform.SetParent(_loadoutSelection.CaughtFishParent.transform);

            _selectedCount--;

            // hide tooltip
            _tooltipObject.SetActive(false);

            // play remove from loadout audio
            SoundManager.Instance.PlayOneShot(deselect, gameObject.transform.position);

            // if a gun -> lock bear's first fish toggle (IF IT WAS THE ONLY GUN)
            if (_itemType == 3) // weapon is 3
            {
                _loadoutSelection.NumOfWeaponsPicked--;

                if(_loadoutSelection.NumOfWeaponsPicked == 0) // if none remainin selected fish
                    _loadoutSelection.DisablePlushToggle();
            }
        }

        // cancel confirmation popups since a different button was pressed.
        _loadoutSelection.CancelCombatConfirmationPopup();
        _loadoutSelection.CancelPracticeConfirmationPopup();
    }
}
