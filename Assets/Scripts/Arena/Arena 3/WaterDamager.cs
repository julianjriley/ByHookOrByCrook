using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDamager : MonoBehaviour
{
    [SerializeField] private float _launchSpeed = 30f;
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("OceanMotion", 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamageLikeAGoodBoy(); // Damage the player
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, _launchSpeed, 0), ForceMode.Impulse);
        }

    }
}
