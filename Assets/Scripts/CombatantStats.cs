using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStats : MonoBehaviour
{
    public float visionRange;
    public float lootingRange;
    public float lootTime;
    public float hitPoints;
    public float moveSpeed;
    
    public float attackPower;
    public float attackRange;
    public float attackSpeed;
    

    public void ApplyStatChanges(Buff buff, bool subtract)
    {
        float posNeg = 1;
        if (subtract == true)
        {
            posNeg *= -1;
        }

        attackPower += buff.attackPowerBonus * posNeg;
        attackRange += buff.attackRangeBonus * posNeg;
        visionRange += buff.visionRangeBonus * posNeg;
        hitPoints += buff.addToHealth * posNeg;
    }
    
    

}
