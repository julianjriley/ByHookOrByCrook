using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccessInteractor : Interactor
{
    [Header("Accessibility Interaction Variables")]

    [Tooltip("Description of the thing, both when it's held and when it's not")]
    [SerializeField][TextArea(1, 2)] private string _descPlayerHas;
    [SerializeField][TextArea(1, 2)] private string _descPlayerHasnt;
    [Tooltip("Obtain text for the thing, both when it's held and when it's not")]
    [SerializeField][TextArea(1, 2)] private string _obtainPlayerHas;
    [SerializeField][TextArea(1, 2)] private string _obtainPlayerHasnt;
    [Tooltip("Display of feature desc")]
    [SerializeField] private TextMeshPro _descText;
    [Tooltip("Display of feature obtain text")]
    [SerializeField] private TextMeshPro _obtainText;
    [Tooltip("Accessbility SpriteRenderer")]
    [SerializeField] private SpriteRenderer _accSprite;
    [Tooltip("Interact bubble animator")]
    [SerializeField] private Animator _accessAnim;

    new void Start()
    {
        base.Start();
        AccessDisplaySprite();
        AccessDisplayText();
        if (GameManager.Instance.GamePersistent.IsTutorialHub)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactAction.IsPressed() && !_isActiveCoroutine && _canInteract)
        {

            StartCoroutine(DoInteractionAccessibility());
        }
    }

    private IEnumerator DoInteractionAccessibility()
    {

        AccessToggle(); // Give them the acccessibility thing or let them return it
        AccessDisplaySprite();

        // Wait until the interact key is up so they don't spam toggle it
        _col.enabled = false;
        yield return new WaitForSeconds(1f);
        AccessDisplayText();
        //yield return new WaitUntil(() => !_interactAction.IsPressed());
        _col.enabled = true;

            
        yield return null;

    }

    #region ACCESS HELPER METHODS
    private bool AccessState() // Check what the state of this accessibility point is (checked out or ready to be taken)
    {
        if (GoodsSold == ShopType.Skipper)
        {
            return GameManager.Instance.GamePersistent.IsSkipper;
        }
        else if (GoodsSold == ShopType.Bobber)
        {
            return GameManager.Instance.GamePersistent.IsBobber;
        }
        return false;
    }

    private void AccessToggle() // Toggle the state of this accessibility point
    {
        if (GoodsSold == ShopType.Skipper)
        {
            GameManager.Instance.GamePersistent.IsSkipper = !GameManager.Instance.GamePersistent.IsSkipper;
        }
        else if (GoodsSold == ShopType.Bobber)
        {
            GameManager.Instance.GamePersistent.IsBobber = !GameManager.Instance.GamePersistent.IsBobber;
        }
    }

    private void AccessDisplaySprite() // Sets the price, title, and desc. for the shop
    {
        if (!AccessState()) // If they don't have it
        {
            // Enable the sprite
            _accSprite.enabled = true;
        }
        else // If they DO have it
        {
            // Disable the sprite
            _accSprite.enabled = false;
        }
        
    }
    private void AccessDisplayText() // Sets the price, title, and desc. for the shop
    {
        if (!AccessState()) // If they don't have it
        {
            // Set the text to the "do you want this" state
            _descText.text = _descPlayerHasnt;
            _obtainText.text = _obtainPlayerHasnt;
        }
        else // If they DO have it
        {
            // Set the text to the "wanna put it back"
            _descText.text = _descPlayerHas;
            _obtainText.text = _obtainPlayerHas;
        }

    }

    #endregion

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
