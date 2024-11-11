using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GunUIHolder : MonoBehaviour
{
    private PlayerCombat _player;
    [SerializeField] GameObject _gunSlot;
    int _gunSlotEquippedIndex = 0;

    private void OnEnable()
    {
        PlayerCombat.WeaponSwitched += MakeIconBigger;
    }
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        StartCoroutine(LateStart());
    }

    public void AssignAllTheGuns()
    {
        for (int i = 0; i < _player.GetWeaponsTransform().childCount; i++)
        { 
            GunSlot gunSlot = Instantiate(_gunSlot, transform).GetComponent<GunSlot>();
            gunSlot.AssignWeapon(_player.GetWeaponsTransform().GetChild(i).GetComponent<WeaponInstance>());
        }

        MakeIconBigger(0);
    }

    public void MakeIconBigger(int equippedWeapon)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == equippedWeapon)
            {
                transform.GetChild(i).GetComponent<GunSlot>().Grow();
            }
            else
                transform.GetChild(i).GetComponent<GunSlot>().EnSmallen();
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.12f);
        AssignAllTheGuns();
    }

    private void OnDisable()
    {
        PlayerCombat.WeaponSwitched -= MakeIconBigger;
    }
}
