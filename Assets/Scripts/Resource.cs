using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Resource : MonoBehaviour
{
    public bool hasBeenLooted = false;
    public bool hasLootLeft = true;

    public GameObject knife;
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject assaultRifle;
    public GameObject healthPack;
    public GameObject bodyArmor;
    public GameObject nothing;
    
    public GameObject determinedLoot;
    [SerializeField] List<GameObject> combatantKnowledgeOfLoot;

    public GameObject LootResource()
    {
        if (hasBeenLooted == false)
        {
            determinedLoot.SetActive(true);
            hasBeenLooted = true;
            return determinedLoot; 
        }
        return nothing;
    }

    public void AddGameObjectToKnowledgeOfLoot(GameObject combatantWithKnowledge)
    {
        combatantKnowledgeOfLoot.Add(combatantWithKnowledge);
    }

    public bool DoesCombatantKnowOfLoot(GameObject combatant)
    {
        if (combatantKnowledgeOfLoot.Contains(combatant)) return false;
        return true;
    }

    public void ConfirmLootTaken()
    {
        GetComponent<ObjectInfo>().ChangeTextToEmpty();
    }

    public void StartBattleRoyale()
    {
        determinedLoot = GetLootFromTable();
        determinedLoot = Instantiate(determinedLoot,transform);
        determinedLoot.transform.position = transform.position;
        determinedLoot.SetActive(false);
    }

    private GameObject GetLootFromTable()
    {
        float rollRandomNumber = Random.Range(0, 100f);
        if (rollRandomNumber <= 15)       //15%   0-15
        {
            return healthPack;
        }
        else if (rollRandomNumber <= 30)   //15%   15-30
        {
            return bodyArmor;
        }
        else if (rollRandomNumber <= 40)   //10%   30-40
        {
            return knife;
        }
        else if (rollRandomNumber <= 70)   //30%   40-70
        {
            return pistol;
        }
        else if (rollRandomNumber <= 90)   //20%   70-90
        {
            return shotgun;
        }
        else                              //10%   90-100
        {
            return assaultRifle;
        }
    }


}
