using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private GameObject _zombieHeartPrefab;
    [SerializeField] private Transform _gridParent;
    private PlayerCombat _player;

    private bool _isPlayerZombie = false;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerCombat>();
        _player.HealthChanged += UpdateHealth;
        PlayerCombat.PlayerIsZombie += ZombifiedHealth;
        StartCoroutine(GetHealthStart());
    }

    private void OnDisable()
    {
        _player.HealthChanged -= UpdateHealth;
        PlayerCombat.PlayerIsZombie -= ZombifiedHealth;
    }

    IEnumerator GetHealthStart()
    {
        yield return new WaitForSeconds(0.12f);
        for (int i = 0; i < _player.Health; i++)
        {
            Instantiate(_heartPrefab, _gridParent);
        }
    }

    void ZombifiedHealth(bool value)
    {
        _isPlayerZombie = value;
    }

    void UpdateHealth(int health)
    {
        foreach (Transform child in _gridParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _player.Health; i++)
        {
            if (_isPlayerZombie)
                Instantiate(_zombieHeartPrefab, _gridParent);
            else
                Instantiate(_heartPrefab, _gridParent);
        }
    }


}
