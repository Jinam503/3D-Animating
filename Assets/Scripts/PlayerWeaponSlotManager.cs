using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : MonoBehaviour
{
    private WeaponSlot _rightHandSlot;

    private void Awake()
    {
        _rightHandSlot = GetComponentInChildren<WeaponSlot>();
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        _rightHandSlot.LoadWeaponModel(weaponItem);
    }

    public void UnLoadWeaponOnSlot()
    {
        _rightHandSlot.UnloadWeaponAndDestroy();
    }
}
