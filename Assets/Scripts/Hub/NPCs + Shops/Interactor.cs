/*
 * Script for hub interaction. An interactor should show a prompt for
 * interaction when walked through, and be able to trigger a conversation
 * or manage a purchase.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

// TODO
// Figure out how to store an NPC's icon and script so they can be called up
// Set up UI for conversations
// Set up UI so that the amount of gold you have is displayed only when in front of a shop, DKC style
// Purchases will have to wait for the GameManager so I can understand how all of this is being stored.

public class Interactor : MonoBehaviour
{
    private InputAction _interactAction;
    private BoxCollider2D _col;
    private bool _canInteract;
    
    [Tooltip("The prompt to interact with the object (press E)")]
    [SerializeField] private GameObject _interactPrompt;
    [Tooltip("Whether this interaction point is for an NPC or a shop point.")]
    public bool IsNPC;
    
    // An NPC interaction point will lock the player into an interaction.
    // A shop interaction point will attempt to use the player's money to purchase something.
    [Header("NPC Interaction Variables")]
    [Tooltip("The text box that will show when the player is farther away from the NPC")]
    [SerializeField] private GameObject _cryerPrompt;
    [Tooltip("The text box and text item that will be used for talking")]
    [SerializeField] private GameObject _convoBubble;
    [SerializeField] private TextMeshPro _convoText;
    private Conversation _conversation;
    private int _convoIndex;
    [Tooltip("The main hub camera, and the camera focused on this specific interaction")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _interactCamera;
    [Tooltip("The player's movement script")]
    [SerializeField] private HubMovement _player;

    [Header("Shop Interaction Variables")]
    [Tooltip("Whether multiple things can be bought from this shop in the same scene")]
    [SerializeField] private bool _multipurchase;
    [Tooltip("Price of the item (or if successive, prices of the items)")]
    [SerializeField] private List<int> _costs;
    private int _currentCost;
    [Tooltip("Title of the item (or if successive, titles of the items)")]
    [SerializeField] private List<string> _titles;
    [Tooltip("Description of the item (or if successive, descriptions of the items)")]
    [SerializeField] private List<string> _descs;
    [Tooltip("Sprite of the item (or if successive, sprites of the items)")]
    [SerializeField] private List<Sprite> _sprites;
    [Tooltip("Convo you get for buying the item (or if successive, convos)")]
    [SerializeField] private List<Conversation> _convos;
    [Tooltip("Display of item price")]
    [SerializeField] private TextMeshPro _priceText;
    [Tooltip("Display of item title")]
    [SerializeField] private TextMeshPro _titleText;
    [Tooltip("Display of item desc")]
    [SerializeField] private TextMeshPro _descText;
    [Tooltip("Item sprite")]
    [SerializeField] private SpriteRenderer _itemSprite;
    [SerializeField] private Animator _shopAnim;
    [Tooltip("The NPC selling the item")]
    [SerializeField] private Interactor _npc;

    public enum ShopType { Rod, BaitSpace, BagSpace, WeaponBait, AttackBait, MovementBait, SupportBait};
    public ShopType GoodsSold;

    bool isActiveCoroutine;
    bool isSkippingLine;

    public delegate void OnShopEnter();
    public static event OnShopEnter onShopEnter;
    public delegate void OnShopExit();
    public static event OnShopExit onShopExit;

    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _col = GetComponent<BoxCollider2D>();
        if (!IsNPC)
        {
            ShopSpawn();
            if (this.isActiveAndEnabled)
            {
                ShopCost();
                _priceText.text = "G " + _currentCost;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !isActiveCoroutine && _canInteract)
        {
            if (IsNPC) // If E is pressed on an NPC
            {
                StartCoroutine(DoInteractionNPC());
            }
            else // If E is pressed on a shop
            {
                StartCoroutine(DoInteractionShop());
            }
        }
    }

    public void SetConversation(Conversation convo, int index)
    {
        _conversation = convo;
        _convoIndex = index;
    }

    private IEnumerator DoInteractionShop()
    {
        // Check if they have enough money
        if(GameManager.Instance.GamePersistent.Gill >= _currentCost)
        {
            // Remove the cost from their balance
            GameManager.Instance.GamePersistent.Gill -= _currentCost;

            ShopConvo(); // Set the correct conversation
            ShopSell();  // Give them the item

            if (!_multipurchase) // If it's one purchase per load, destroy this thing
            {
                Destroy(gameObject); // Destroy on purchase
            }
            else // Essentially respawn/reset the shop
            {
                _col.enabled = false;
                ShopSpawn();
                if (this.isActiveAndEnabled)
                {
                    ShopCost();
                    _priceText.text = "G " + _currentCost;
                }
                yield return new WaitUntil(() => !_interactAction.IsPressed());
                _col.enabled = true;

            }
            
            
        }
        else
        {
            // If not, [LOUD INCORRECT BUZZER NOISE]
            // We can put a little sound effect and a little text box animation here to really
            // emphasize to the player that they are poor

            _shopAnim.Play("NotEnough", 0, 0);
            yield return new WaitForSeconds(.5f);
            _shopAnim.Play("Static", 0, 0);
        }
        yield return null;

    }

    private List<bool> getIsConvoHad()
    {
        if (GoodsSold == ShopType.Rod)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadRod;
        }
        else if (GoodsSold == ShopType.BagSpace || GoodsSold == ShopType.BaitSpace)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBag;
        }
        else
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBait;
        }
    }

    #region SHOP HELPER METHODS
    private void ShopSpawn() // Checks if we need to spawn this shop at all
    {
        // This is the only place in the game we will EVER need to check current sizes against a max,
        // so this is where the max rod level, max bait inventory size, and max battle inventory size are stored.
        int maxBaitSlots = 5;
        int maxBattleSlots = 5;
        if (GoodsSold == ShopType.Rod)
        {
            if (GameManager.Instance.GamePersistent.RodLevel >= 2)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
               
        }
        else if(GoodsSold == ShopType.BaitSpace)
        {
            if (GameManager.Instance.GamePersistent.BaitInventorySize >= maxBaitSlots)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else if (GoodsSold == ShopType.BagSpace)
        {
            if (GameManager.Instance.GamePersistent.BattleInventorySize >= maxBattleSlots)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else if (GoodsSold == ShopType.WeaponBait)
        {
            if (GameManager.Instance.GamePersistent.WeaponBait == true)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else if (GoodsSold == ShopType.AttackBait)
        {
            if (GameManager.Instance.GamePersistent.AttackBait == true)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else if (GoodsSold == ShopType.SupportBait)
        {
            if (GameManager.Instance.GamePersistent.SupportBait == true)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else if (GoodsSold == ShopType.MovementBait)
        {
            if (GameManager.Instance.GamePersistent.MovementBait == true)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }

    private void ShopSell() // Handles providing the player with the correct item
    {
        if (GoodsSold == ShopType.Rod)
        {
            GameManager.Instance.GamePersistent.RodLevel += 1;
        }
        else if (GoodsSold == ShopType.BaitSpace)
        {
            GameManager.Instance.GamePersistent.BaitInventorySize += 1;
        }
        else if (GoodsSold == ShopType.BagSpace)
        {
            GameManager.Instance.GamePersistent.BattleInventorySize += 1;
        }
        else if (GoodsSold == ShopType.WeaponBait)
        {
            GameManager.Instance.GamePersistent.WeaponBait = true;
        }
        else if (GoodsSold == ShopType.AttackBait)
        {
            GameManager.Instance.GamePersistent.AttackBait = true;
        }
        else if (GoodsSold == ShopType.SupportBait)
        {
            GameManager.Instance.GamePersistent.SupportBait = true;
        }
        else if (GoodsSold == ShopType.MovementBait)
        {
            GameManager.Instance.GamePersistent.MovementBait = true;
        }
    }

    private void ShopCost() // Sets the price, title, and desc. for the shop
    {
        int minBaitSlots = 3;
        int minBattleSlots = 3;
        // If you have rod 0, this'll get the cost for rod 1
        if (GoodsSold == ShopType.Rod)
        {
            _currentCost = _costs[GameManager.Instance.GamePersistent.RodLevel]; 
            _titleText.text = _titles[GameManager.Instance.GamePersistent.RodLevel];
            _descText.text = _descs[GameManager.Instance.GamePersistent.RodLevel];
            _itemSprite.sprite = _sprites[GameManager.Instance.GamePersistent.RodLevel];
        }
        // The minus here should be whatever the default inven sizes are for these
        else if (GoodsSold == ShopType.BaitSpace)
        {
            _currentCost = _costs[GameManager.Instance.GamePersistent.BaitInventorySize - minBaitSlots];
            _titleText.text = _titles[GameManager.Instance.GamePersistent.BaitInventorySize - minBaitSlots];
            _descText.text = _descs[GameManager.Instance.GamePersistent.BaitInventorySize - minBaitSlots];
            _itemSprite.sprite = _sprites[GameManager.Instance.GamePersistent.BaitInventorySize - minBaitSlots];
        }
        else if (GoodsSold == ShopType.BagSpace)
        {
            _currentCost = _costs[GameManager.Instance.GamePersistent.BattleInventorySize - minBattleSlots];
            _titleText.text = _titles[GameManager.Instance.GamePersistent.BattleInventorySize - minBattleSlots];
            _descText.text = _descs[GameManager.Instance.GamePersistent.BattleInventorySize - minBattleSlots];
            _itemSprite.sprite = _sprites[GameManager.Instance.GamePersistent.BattleInventorySize - minBattleSlots];
        }
        else
        {
            _currentCost = _costs[0];
            _titleText.text = _titles[0];
            _descText.text = _descs[0];
            _itemSprite.sprite = _sprites[0];
        }
    }

    private void ShopConvo() // Assigns the relevant conversation to the attached NPC
    {
        int minBaitSlots = 3;
        int minBattleSlots = 3;
        if (GoodsSold == ShopType.Rod)
        {
            _npc.SetConversation(_convos[GameManager.Instance.GamePersistent.RodLevel], -1);
        }
        else if (GoodsSold == ShopType.BaitSpace)
        {
            _npc.SetConversation(_convos[GameManager.Instance.GamePersistent.BattleInventorySize - minBaitSlots], -1);
        }
        else if (GoodsSold == ShopType.BagSpace)
        {
            _npc.SetConversation(_convos[GameManager.Instance.GamePersistent.BattleInventorySize - minBattleSlots], -1);
        }
        else
        {
            _npc.SetConversation(_convos[0], -1);
        }
    }
    #endregion

    #region NPC COROUTINES

    private IEnumerator DoInteractionNPC()
    {
        isActiveCoroutine = true;
        _col.enabled = false;

        // Deactivate the interaction prompt
        _interactPrompt.SetActive(false);

        // Switch cameras
        _interactCamera.SetActive(true);
        _mainCamera.SetActive(false);

        // Stop player movement
        _player.IsIdle = true;

        // Show the discussion box
        _convoBubble.SetActive(true);

        // Set up the conversation
        if(_convoIndex >= 0)
            getIsConvoHad()[_convoIndex] = true;
        foreach(string line in _conversation.lines)
        {
            isSkippingLine = false;
            StartCoroutine(DoTextEscapeSubroutine());
            for(int i = 0; i < line.Length; i++)
            {
                if (!isSkippingLine)
                {
                    if (line[i].Equals(' '))
                    {
                        i++;
                    }
                    _convoText.text = line.Substring(0, i);

                    yield return new WaitForSeconds(.07f);
                }
                else { 
                    _convoText.text = line;
                    break;
                }
                
            }
            StopCoroutine(DoTextEscapeSubroutine());
            _convoText.text = line;
            yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => _interactAction.IsPressed());
        }
        // Hide the discussion box
        _convoBubble.SetActive(false);

        // Switch cameras back
        _interactCamera.SetActive(false);
        _mainCamera.SetActive(true);

        // Give player movement back
        _player.IsIdle = false;

        yield return new WaitForSeconds(3f);
        isActiveCoroutine = false;
        _col.enabled = true;
        yield return null;
    }

    private IEnumerator DoTextEscapeSubroutine()
    {
        yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
        yield return new WaitUntil(() => _interactAction.IsPressed());
        isSkippingLine = true;
    }

    #endregion

    #region TRIGGER PROXIMITY CHECKS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = true;
            _interactPrompt.SetActive(true);
            if (IsNPC)
                _cryerPrompt.SetActive(false);
            else
                onShopEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canInteract = false;
            _interactPrompt.SetActive(false);
          if(!IsNPC)
                onShopExit?.Invoke();
        }
    }
    #endregion

}
