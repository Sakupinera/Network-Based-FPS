using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMgr : BaseManager<WeaponMgr>
{
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    public WeaponMgr()
    {

    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
    }
}
