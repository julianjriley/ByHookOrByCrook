using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingPufferfish : PassiveItemInstance
{
    //private float _damage = 1;
    //private float _waitTime = 1;
    private float _speed = 10;

    private GameObject _pivot;

    [SerializeField]
    GameObject prefab;


    public override void ItemEffect()
    {
        base.ItemEffect();



        // Spawn another orbiting pufferfish
        OrbitingPufferfishProjectile orbitingPufferfishProjectile = Instantiate(prefab, transform.position, Quaternion.identity).GetComponentInChildren<OrbitingPufferfishProjectile>();
        orbitingPufferfishProjectile.AssertPlayer(_player.gameObject);
        //_pivot = _player.gameObject;

    }

    private void Update()
    {
        
        // Orbiting around the player
        //transform.RotateAround(_player.transform.position, new Vector3(0, 0, 1), _speed * Time.deltaTime);

    }
    private void FixedUpdate()
    {
        
    }



}
