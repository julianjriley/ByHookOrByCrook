using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CrosshairOverheat : MonoBehaviour
{
    Image _circle;
    [SerializeField] Color _heatingColor;
    [SerializeField] Color _overheatedColor;
    [SerializeField] GameObject _overheatedImage;
    [SerializeField] RectTransform _rect;
    PlayerCombat _player;
    WeaponInstance[] _weaponInstances;
    WeaponInstance _currentWeapon;

    private Vector3 _baseScale;

    bool _active = false;

    private void OnEnable()
    {
        PlayerCombat.WeaponSwitched += ChangeWeapon;
    }
    private void OnDisable()
    {
        PlayerCombat.WeaponSwitched -= ChangeWeapon;
    }
    private void Start()
    {
        _circle = GetComponent<Image>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        StartCoroutine(StartFunctions());

        _baseScale = _rect.localScale;
    }
    private void Update()
    {
        if (!_active)
            return;
        _circle.fillAmount = _currentWeapon.GetHeatLevel() / 100f;
        if (_currentWeapon.GetOverHeatedState())
        {
            _circle.color = _overheatedColor;
            _overheatedImage.SetActive(true);
        }
        else
        {
            _circle.color = _heatingColor;
            _overheatedImage.SetActive(false);
        }
        transform.position = Mouse.current.position.ReadValue();

        // update scale to match crosshair scale from settings
        _rect.localScale = _baseScale * GameManager.Instance.GamePersistent.CrosshairSizeMultiplier;
    }

    void ChangeWeapon(int index)
    {
        _currentWeapon = _weaponInstances[index];
    }

    IEnumerator StartFunctions()
    {
        yield return new WaitForSeconds(0.6f);
        _weaponInstances = _player.GetWeaponsTransform().GetComponentsInChildren<WeaponInstance>();
        _currentWeapon = _weaponInstances[0];
        _active = true;
    }
}
