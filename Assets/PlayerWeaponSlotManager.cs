using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : MonoBehaviour
{
    private WeaponSlot rightHandSlot;

    private void Awake()
    {
        rightHandSlot = GetComponentInChildren<WeaponSlot>();
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        rightHandSlot.LoadWeaponModel(weaponItem);
    }

    public void UnLoadWeaponOnSlot()
    {
        rightHandSlot.UnloadWeaponAndDestroy();
    }
}
