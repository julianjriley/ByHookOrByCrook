using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CrosshairOverheat : MonoBehaviour
{
    Image _circle;
    [SerializeField] Color _heatingColor;
    [SerializeField] Color _overheatedColor;
    [SerializeField] GameObject _overheatedImage;
    PlayerCombat _player;
    WeaponInstance[] _weaponInstances;
    WeaponInstance _currentWeapon;

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
