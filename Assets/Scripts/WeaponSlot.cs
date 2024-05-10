using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public Transform parentOverride;
    public WeaponItem currentWeapon;
    private GameObject _currentWeaponModel;

    public void UnloadWeaponAndDestroy()
    {
        if (_currentWeaponModel)
        {
            Destroy(_currentWeaponModel);
        }

        currentWeapon = null;
    }

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        GameObject model = Instantiate(weaponItem.modelPrefab);
        if (model)
        {
            model.transform.parent = parentOverride ? parentOverride : transform;

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
        _currentWeaponModel = model;
        currentWeapon = weaponItem;
    }
}
