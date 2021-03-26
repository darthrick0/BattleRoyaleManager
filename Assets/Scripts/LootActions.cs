using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootActions : MonoBehaviour
{
    public Buff currentWeapon;
    public GameObject weapon;
    //public bool currentlyLooting = false;
    //public float lootTime;
    //public float lootRange;

    private CombatantStats unitStats;
    private Equipment unitEquipment;
    private BuildDB unitDB;
    private CombatantStateMachine unitSM;
    private void Awake()
    {
        unitStats = GetComponent<CombatantStats>();
        unitEquipment = GetComponent<Equipment>();
        unitDB = GetComponent<BuildDB>();
        unitSM = GetComponent<CombatantStateMachine>();
    }
    
    public void InitiateLooting(Transform lootTarget)
    {
        StartCoroutine(CheckForLoot(lootTarget));
    }
    
    
    private IEnumerator CheckForLoot(Transform lootTarget)
    {
        unitSM.currentlyLooting = true;
        print("CheckForLoot called, currentlyLooting = " + unitSM.currentlyLooting);
        yield return new WaitForSeconds(unitStats.lootTime);
        GameObject potentialBuffGameObject = lootTarget.gameObject.GetComponent<Resource>().LootResource();

        if (potentialBuffGameObject.GetComponent<ObjectInfo>().isNothing == false)
        {
            Resource lootTargetResource = lootTarget.gameObject.GetComponent<Resource>();
            Buff potentialBuff = potentialBuffGameObject.GetComponent<Buff>();
            LootAction(potentialBuff, potentialBuffGameObject, lootTargetResource);
            lootTargetResource.AddGameObjectToKnowledgeOfLoot(gameObject);
        }
        unitDB.RebuildDB();
        unitSM.currentlyLooting = false;
        print("CheckForLoot exiting, currentlyLooting = " + unitSM.currentlyLooting);
    }
    
    private void LootAction(Buff potentialBuff, GameObject potentialBuffGameObject, Resource lootTargetResource)
    {

        if (potentialBuff.isWeapon == true && currentWeapon == null)
        {
            unitStats.ApplyStatChanges(potentialBuff, false);
            currentWeapon = potentialBuff;
            unitEquipment.DisableWeaponComponents(potentialBuffGameObject);
            lootTargetResource.ConfirmLootTaken();
        }
        else if (potentialBuff.isWeapon == true && potentialBuff.attackRangeBonus > currentWeapon.attackRangeBonus)
        {
            unitStats.ApplyStatChanges(currentWeapon, true);
            unitEquipment.EnableWeaponComponents();
            unitStats.ApplyStatChanges(potentialBuff, false);
            currentWeapon = potentialBuff;
            unitEquipment.DisableWeaponComponents(potentialBuffGameObject);
                
            lootTargetResource.ConfirmLootTaken();
        }
        else if (potentialBuff.isWeapon == false && potentialBuffGameObject.GetComponent<ObjectInfo>().isNothing != true)
        {
            unitEquipment.UnpackBuff(potentialBuffGameObject);
            unitStats.ApplyStatChanges(potentialBuff, false);
            Destroy(potentialBuffGameObject);
            lootTargetResource.ConfirmLootTaken();
        }
    }





}
