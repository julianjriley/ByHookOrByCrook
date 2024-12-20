using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class ShopInteractor : Interactor
{
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
    [Tooltip("Item SpriteRenderer")]
    [SerializeField] private SpriteRenderer _itemSprite;
    [Tooltip("Interact bubble animator")]
    [SerializeField] private Animator _shopAnim;
    [Tooltip("The NPC selling the item")]
    [SerializeField] private NPCInteractor _npc;
    [SerializeField] private EventReference purchaseSound;
    [SerializeField] private EventReference tooPoorSound;

    [Tooltip("If NPC is of shop typ nothing, then after purchase will spwan a fortune")]
    [SerializeField] private GameObject _whishNPC;

    public delegate void OnShopEnter();
    public static event OnShopEnter onShopEnter;
    public delegate void OnShopPurchase(int cost);
    public static event OnShopPurchase onShopPurchase;
    public delegate void OnShopExit();
    public static event OnShopExit onShopExit;

    public delegate void OnBaitPurchase();
    public static event OnBaitPurchase onBaitPurchase;

    private bool shopActiveCoroutine;

    new void Start()
    {
        base.Start();
        ShopSpawn();
        if (this.isActiveAndEnabled)
        {
            ShopCost();
            _priceText.text = "S " + _currentCost;
        }
        if (GameManager.Instance.GamePersistent.IsTutorialHub && GoodsSold != ShopType.Nothing)
        {
            Destroy(gameObject);
        }

    }

    private void OnEnable()
    {
        onBaitPurchase += BaitCost;
    }

    private void OnDisable()
    {
        onBaitPurchase -= BaitCost;
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !_isActiveCoroutine && _canInteract && !shopActiveCoroutine) //added last bool to keep sound effect from triggering infinitely when holding button
        {

                StartCoroutine(DoInteractionShop());
        }
    }
    private IEnumerator DoInteractionShop()
    {
        shopActiveCoroutine = true;
        // Check if they have enough money
        if (GameManager.Instance.GamePersistent.Gill >= _currentCost)
        {
            // Remove the cost from their balance
            GameManager.Instance.GamePersistent.Gill -= _currentCost;
            onShopPurchase?.Invoke(_currentCost);
            if(GoodsSold != ShopType.Nothing)
                ShopConvo(); // Set the correct conversation
            ShopSell();  // Give them the item
            SoundManager.Instance.PlayOneShot(purchaseSound, gameObject.transform.position);

            if (!_multipurchase) // If it's one purchase per load, destroy this thing
            {
                //If the ShopType Is nothing
                if (GoodsSold == ShopType.Nothing)
                {
                    //Load in the Foutune Teller
                    _whishNPC.SetActive(true);
                }
                Destroy(gameObject); // Destroy on purchase
            }
            else // Essentially respawn/reset the shop
            {
                _col.enabled = false;
                ShopSpawn();
                if (this.isActiveAndEnabled)
                {
                    ShopCost();
                    _priceText.text = "S " + _currentCost;
                }
                yield return new WaitUntil(() => !_interactAction.IsPressed());
                _col.enabled = true;

            }


        }
        else
        {
            // If not, [LOUD INCORRECT BUZZER NOISE] -- got u lol -Andres
            // We can put a little sound effect and a little text box animation here to really
            // emphasize to the player that they are poor

            SoundManager.Instance.PlayOneShot(tooPoorSound, gameObject.transform.position);
            _shopAnim.Play("NotEnough", 0, 0);
            yield return new WaitUntil(() => !_interactAction.IsPressed());

            //yield return new WaitForSeconds(.5f);
            //_shopAnim.Play("Static", 0, 0);
        }
        shopActiveCoroutine = false;
        yield return null;

    }

    #region SHOP HELPER METHODS
    private void ShopSpawn() // Checks if we need to spawn this shop at all
    {
        // This is the only place in the game we will EVER need to check current sizes against a max,
        // so this is where the max rod level, max bait inventory size, and max battle inventory size are stored.
        int maxBaitSlots = 10;
        int maxBattleSlots = 8;
        if (GoodsSold == ShopType.Rod)
        {
            if (GameManager.Instance.GamePersistent.RodLevel >= 2)
            {
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }

        }
        else if (GoodsSold == ShopType.BaitSpace)
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
            onBaitPurchase?.Invoke();
        }
        else if (GoodsSold == ShopType.AttackBait)
        {
            GameManager.Instance.GamePersistent.AttackBait = true;
            onBaitPurchase?.Invoke();
        }
        else if (GoodsSold == ShopType.SupportBait)
        {
            GameManager.Instance.GamePersistent.SupportBait = true;
            onBaitPurchase?.Invoke();
        }
        else if (GoodsSold == ShopType.MovementBait)
        {
            GameManager.Instance.GamePersistent.MovementBait = true;
            onBaitPurchase?.Invoke();
        }
    }

    private void ShopCost() // Sets the price, title, and desc. for the shop
    {
        int minBaitSlots = 3;
        int minBattleSlots = 2;
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
            BaitCost();
            _titleText.text = _titles[0];
            _descText.text = _descs[0];
            _itemSprite.sprite = _sprites[0];
        }
    }

    private void ShopConvo() // Assigns the relevant conversation to the attached NPC
    {
        int minBaitSlots = 3;
        int minBattleSlots = 2;
        if (GoodsSold == ShopType.Rod)
        {
            _npc.SetConversation(_convos[GameManager.Instance.GamePersistent.RodLevel], -1);
        }
        else if (GoodsSold == ShopType.BaitSpace)
        {
            _npc.SetConversation(_convos[GameManager.Instance.GamePersistent.BaitInventorySize - minBaitSlots], -1);
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

    private void BaitCost()
    {
        // If this is a bait shop, we need to recalc
        if (GoodsSold == ShopType.WeaponBait || GoodsSold == ShopType.MovementBait || GoodsSold == ShopType.AttackBait || GoodsSold == ShopType.SupportBait)
        {
            int baitCount = 0;
            if(GameManager.Instance.GamePersistent.WeaponBait == true)
                baitCount++;
            if (GameManager.Instance.GamePersistent.AttackBait == true)
                baitCount++;
            if (GameManager.Instance.GamePersistent.SupportBait == true)
                baitCount++;
            if (GameManager.Instance.GamePersistent.MovementBait == true)
                baitCount++;

            if(baitCount < 4)
                _currentCost = _costs[baitCount];

            _priceText.text = "S " + _currentCost;
        }
    }
    #endregion

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        onShopEnter?.Invoke();
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        onShopExit?.Invoke();
    }
}
