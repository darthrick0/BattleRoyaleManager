using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Move : MonoBehaviour
{
    
    //private bool attackMove;
    public CombatantStats unitStats;
    public Combatant combatantComponent;
    public Vector3 centerOfCircle;
    public CombatantStateMachine unitSM;
    
    private void Awake()
    {
        unitStats = GetComponent<CombatantStats>();
        combatantComponent = GetComponent<Combatant>();
        unitSM = GetComponent<CombatantStateMachine>();
    }

    private void Start()
    {
        centerOfCircle = combatantComponent.centerOfCircle;
    }



    public void InitiateMovement(Transform target, bool fleeTrue)
    {
        StartCoroutine(MoveToTransform(target, fleeTrue));
    }
    
    public IEnumerator MoveToTransform(Transform target, bool fleeTrue)
    {
        while (target != null || Vector3.Distance(transform.position, target.transform.position) > 1000)
        {
            if (unitSM.isDead)
            {
                break;
            }
            else if (unitSM.paused || unitSM.currentlyLooting)
            {
                yield return new WaitForEndOfFrame();
            }
            else if (unitSM.attackMove)
            {
                AttackMove(target.gameObject);
                yield return new WaitForEndOfFrame();
            }
            else if (unitSM.currentlyAttacking == true)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                WanderTowardCenter();
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public void AttackMove(GameObject attackTarget)
    {
        Vector3 directionToTarget;
        directionToTarget = (attackTarget.transform.position - transform.position).normalized;
        if (unitStats.attackRange < Vector3.Distance(transform.position, attackTarget.transform.position))
        {
            transform.Translate(directionToTarget * (Time.deltaTime * unitStats.moveSpeed));
        }    
    }

    public Vector2 WanderTowardCenter()
    {
        unitSM.wandering = true;
        Vector3 centerishOfCircle = new Vector3(centerOfCircle.x + Random.Range(-0.5f, 0.5f), centerOfCircle.y + Random.Range(-0.5f, 0.5f), 0f);
        Vector3 directionOfCenter = centerishOfCircle - transform.position;
        Vector2 newDir = new Vector2(directionOfCenter.x, directionOfCenter.y).normalized;
        return newDir;
    }
}
