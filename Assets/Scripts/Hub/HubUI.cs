using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    private Animator _anim;

    private int _recordedGill; // The last amount of Gill the UI remembers us having
    private int _diff;
    private bool _activeCoroutine;

    private bool _isShowingMoney;
    private bool _atShop;
    private float _moneyTimer;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _recordedGill = GameManager.Instance.GamePersistent.Gill;
        _moneyDisplay.text = "S " + GameManager.Instance.GamePersistent.Gill;
    }

    private void OnEnable()
    {
        ShopInteractor.onShopEnter += ShowGill;
        ShopInteractor.onShopPurchase += AddToDiff;
        ShopInteractor.onShopExit += LeaveShop;
    }

    private void OnDisable()
    {
        ShopInteractor.onShopEnter -= ShowGill;
        ShopInteractor.onShopPurchase -= AddToDiff;
        ShopInteractor.onShopExit -= LeaveShop;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(_recordedGill != GameManager.Instance.GamePersistent.Gill && !_activeCoroutine) // A way for us to track if there's been a change
        {
            StartCoroutine(DoIncrementalIncrease());
        }*/

        if(_isShowingMoney && _moneyTimer > 0 && !_atShop && !_activeCoroutine)
        {
            _moneyTimer -= Time.deltaTime;
        }
        if(_isShowingMoney && _moneyTimer <= 0)
        {
            HideGill();
        }
    }

    #region DISPLAY METHODS
    private void ShowGill()
    {
        if (!_isShowingMoney)
        {
            _anim.Play("GoDown", 0, 0);
            _isShowingMoney = true;
        }
        
        _atShop = true;
        _moneyTimer = 3f;
    }

    private void LeaveShop()
    {
        _atShop = false;
    }

    private void HideGill()
    {
        _anim.Play("GoUp", 0, 0);
        _isShowingMoney = false;
    }
    #endregion

    private void AddToDiff(int cost)
    {
        _diff += cost;
        if (!_activeCoroutine)
        {
            StartCoroutine(DoIncrementalIncrease());
        }
    }

    private IEnumerator DoIncrementalIncrease()
    {
        _activeCoroutine = true;

        float timeSlice = .002f;

        if (_diff > 0) // We lost money
        {
            for(int i = 0; i < _diff; i++)
            {
                _moneyDisplay.text = "S " + _recordedGill;
                _recordedGill--;
                yield return new WaitForSeconds(timeSlice * (i / _diff));
            }
        }
        /*else // We gained money
        {
            for (int i = 0; i < Mathf.Abs(_diff); i++)
            {
                _moneyDisplay.text = "G " + _recordedGill;
                _recordedGill++;
                yield return new WaitForSeconds(.01f);
            }
        }*/
        _moneyDisplay.text = "S " + GameManager.Instance.GamePersistent.Gill;
        _recordedGill = GameManager.Instance.GamePersistent.Gill;
        _activeCoroutine=false;
        _diff = 0;
        yield return null;
    }
}
