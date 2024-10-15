using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private Transform _gridParent;
    private PlayerCombat _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerCombat>();
        _player.HealthChanged += UpdateHealth;
        StartCoroutine(GetHealthStart());
    }

    IEnumerator GetHealthStart()
    {
        yield return new WaitForSeconds(0.12f);
        for (int i = 0; i < _player.Health; i++)
        {
            Instantiate(_heartPrefab, _gridParent);
        }
    }

    void UpdateHealth(int health)
    {
        foreach (Transform child in _gridParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _player.Health; i++)
        {
            Instantiate(_heartPrefab, _gridParent);
        }
    }


}
