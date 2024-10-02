using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    private Animator _anim;

    private int _recordedGill; // The last amount of Gill the UI remembers us having
    private bool _activeCoroutine;

    private bool _isShowingMoney;
    private bool _atShop;
    private float _moneyTimer;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _recordedGill = GameManager.Instance.GamePersistent.Gill;
        _moneyDisplay.text = "G " + GameManager.Instance.GamePersistent.Gill;
    }

    private void OnEnable()
    {
        Interactor.onShopEnter += ShowGill;
        Interactor.onShopExit += LeaveShop;
    }

    private void OnDisable()
    {
        Interactor.onShopEnter -= ShowGill;
        Interactor.onShopExit -= LeaveShop;
    }

    // Update is called once per frame
    void Update()
    {
        if(_recordedGill != GameManager.Instance.GamePersistent.Gill && !_activeCoroutine) // A way for us to track if there's been a change
        {
            StartCoroutine(DoIncrementalIncrease());
        }
        if(_isShowingMoney && _moneyTimer > 0 && !_atShop)
        {
            _moneyTimer -= Time.deltaTime;
        }
        if(_isShowingMoney && _moneyTimer <= 0)
        {
            HideGill();
        }
    }

    private void ShowGill()
    {
        _anim.Play("GoDown", 0, 0);
        _isShowingMoney = true;
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

    private IEnumerator DoIncrementalIncrease()
    {
        _activeCoroutine = true;
        int diff = _recordedGill - GameManager.Instance.GamePersistent.Gill;
        if(diff > 0) // We lost money
        {
            for(int i = 0; i < diff; i++)
            {
                _moneyDisplay.text = "G " + _recordedGill;
                _recordedGill--;
                yield return new WaitForSeconds(.01f);
            }
        }
        else // We gained money
        {
            for (int i = 0; i < Mathf.Abs(diff); i++)
            {
                _moneyDisplay.text = "G " + _recordedGill;
                _recordedGill++;
                yield return new WaitForSeconds(.01f);
            }
        }
        _moneyDisplay.text = "G " + GameManager.Instance.GamePersistent.Gill;
        _recordedGill = GameManager.Instance.GamePersistent.Gill;
        _activeCoroutine=false;
        yield return null;
    }
}
