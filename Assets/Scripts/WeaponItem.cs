using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/WeaponItem")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;

    public AmmoType ammoType;
    public int maxAmmo;
}
public enum AmmoType
{
    _7_62MM,
    _5_56MM,
}
