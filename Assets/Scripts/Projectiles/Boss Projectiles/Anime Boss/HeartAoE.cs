using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAoE : Projectile
{
    private Color _heartColor;

    // TODO: plug in animation
    // TODO: slowly appears then fully appears (timer of some sort is needed)

    protected override void Start()
    {
        base.Start();

        // When spawned, spawn multiple in a line based on player's position
        // if player is on left side, spawn 3 to the right
        // if player is on right side, spawn 3 to the left


        _heartColor = GetComponent<SpriteRenderer>().color;
        _heartColor.a = 0;

        StartCoroutine(HeartAttack());
    }
    void SpawnHearts()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newHearts = Instantiate(this.gameObject, GameObject.Find("AttackHolderEmpty").GetComponent<Transform>());

            if (_playerCombat.gameObject.GetComponent<Transform>().position.x < Screen.width / 2)
            {
                newHearts.transform.position = new(newHearts.transform.position.x + 10 * i, _playerCombat.GetComponent<Transform>().position.y, newHearts.transform.position.z);
            }
            else
            {
                newHearts.transform.position = new(newHearts.transform.position.x - 10 * i, _playerCombat.GetComponent<Transform>().position.y, newHearts.transform.position.z);
            }
        }

    }
    IEnumerator HeartAttack()
    {
        SpawnHearts();

        _heartColor.a = Mathf.MoveTowards(1, 0, 5f);

        yield return new WaitForSeconds(1);
    }

}
