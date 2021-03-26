using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    private CombatantStats unitStats;
    private Move unitMoveComponent;
    private BuildDB unitDB;
    private CombatantStateMachine unitSM;
    
    private void Awake()
    {
        unitStats = GetComponent<CombatantStats>();
        unitMoveComponent = GetComponent<Move>();
        unitDB = GetComponent<BuildDB>();
        unitSM = GetComponent<CombatantStateMachine>();

    }

    public void InitiateCombat(GameObject target)
    {
        StartCoroutine(CombatSequence(target));
    }
    
    private IEnumerator CombatSequence(GameObject attackTarget)
    {
        
        unitSM.currentlyAttacking = true;
        Combat targetCombatComponent = attackTarget.GetComponent<Combat>();
        //attack
        
        float attackTargetDistance = Mathf.Abs(Vector3.Distance(transform.position, attackTarget.transform.position));
        bool opponentIsDead = attackTarget.GetComponent<CombatantStateMachine>().isDead;
        
        while (opponentIsDead == false && unitStats.hitPoints > 0)
        {
            if (Vector3.Distance(transform.position, attackTarget.transform.position) <= unitStats.attackRange)
            {
                yield return new WaitForSeconds(unitStats.attackSpeed);
                Attack(targetCombatComponent);
            }
            else
            {
                yield return new WaitForEndOfFrame();
                unitMoveComponent.AttackMove(attackTarget);
            }
            Debug.Log("Im in the attack Loop");
        }
        unitDB.RebuildDB();
    }


    private void Attack(Combat targetCombatComponent)
    {
        targetCombatComponent.TakeDamage(unitStats.attackPower, gameObject);
    }

    public void TakeDamage(float damageAmount, GameObject attacker)
    {
        unitStats.hitPoints -= damageAmount;
        if (unitStats.hitPoints <= 0)
        {
            Die();
        }
        if (unitSM.currentlyAttacking == false) StartCoroutine(CombatSequence(attacker));
    }

    private void Die()
    {
        transform.position = new Vector3(50000f, 50000f, 0f);
        unitSM.isDead = true;
        
        gameObject.SetActive(false);
    }
}
