using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Buff currentWeapon = null;
    public GameObject weapon;
    private CombatantStats unitStats;
    
    private void Awake()
    {
        unitStats = GetComponent<CombatantStats>();
    }
    

    public void EnableWeaponComponents()
    {
        Resource weaponResource = currentWeapon.GetComponent<Resource>();
        currentWeapon.transform.SetParent(null);
        weaponResource.enabled = true;
        weaponResource.determinedLoot = weapon;
        weaponResource.hasBeenLooted = false;
        ObjectInfo weaponObjectInfo = currentWeapon.GetComponent<ObjectInfo>();
        weaponObjectInfo.enabled = true;
        currentWeapon.GetComponent<Collider2D>().enabled = true;
    }

    public void UnpackBuff(GameObject newItem)
    {
        Buff newItemBuff = newItem.GetComponent<Buff>();
        unitStats.ApplyStatChanges(newItemBuff, false);
        if (newItemBuff.isWeapon == true)
        {
            currentWeapon = newItemBuff;
            DisableWeaponComponents(newItem);
        }
        else
        {
            Destroy(newItem);
        }
    }

    public void DisableWeaponComponents(GameObject newItem)
    {
        newItem.GetComponent<Resource>().enabled = false;
        newItem.GetComponent<ObjectInfo>().enabled = false;
        if (newItem.GetComponent<Collider2D>() != null) newItem.GetComponent<Collider2D>().enabled = false;
        newItem.transform.SetParent(transform);
        newItem.transform.position = transform.position;
    }
}
