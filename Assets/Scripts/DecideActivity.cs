using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DecideActivity : MonoBehaviour
{
    public float currentBestDistance;
    public Transform currentBestTransform;

    private BuildDB unitDB;
    private LootActions unitLootActions;
    private Move unitMoveComponent;
    private CombatantStats unitStats;
    private CombatantStateMachine unitStateMachine;
    private Combat unitCombat;
    private ObjectInfo unitObjectInfo;
    
    private Dictionary<int, BuildDB.MoveAction> priorityRanking = new Dictionary<int, BuildDB.MoveAction>();
    private Dictionary<int, List<Transform>> rankedListOfTransforms = new Dictionary<int, List<Transform>>();
    
    private void Awake()
    {
        unitDB = GetComponent<BuildDB>();
        unitLootActions = GetComponent<LootActions>();
        unitMoveComponent = GetComponent<Move>();
        unitStats = GetComponent<CombatantStats>();
        unitStateMachine = GetComponent<CombatantStateMachine>();
        unitCombat = GetComponent<Combat>();
        unitObjectInfo = GetComponent<ObjectInfo>();
    }

    private void StartRound()
    {
        RefreshDBVals();
    }

    private void RefreshDBVals()
    {
        priorityRanking = unitDB.GetPriorityRanking();
        rankedListOfTransforms = unitDB.GetRankedListOfTransforms();
    }

    public Vector2 GetActivity()
    {
        //getActivityRunningCount = 0f;
        currentBestDistance = 100000f;
        currentBestTransform = null;
        RefreshDBVals();
        Vector2 returnDirection;
        int storePriority = -1;
        for (int i = 0; i < rankedListOfTransforms.Count; i++)
        {
            if(priorityRanking[i] != BuildDB.MoveAction.Ignore)
            {
                foreach(var potentialTransform in rankedListOfTransforms[i])
                {
                    var transDistance = Vector3.Distance(transform.position, potentialTransform.position);
                    if (transDistance < currentBestDistance)
                    {
                        currentBestTransform = potentialTransform;
                        currentBestDistance = transDistance;
                        storePriority = i;
                    }
                }
            }
            if (currentBestTransform != null) i = rankedListOfTransforms.Count;
        }
        //Debug.Log("currentBestTransform = " + currentBestTransform.position);
        if (currentBestTransform != null)
        {
            returnDirection = ChooseActivity(currentBestTransform, storePriority);
        }
        else
        {
            returnDirection = unitMoveComponent.WanderTowardCenter();
        }
        return returnDirection;  
    }

    private Vector2 ChooseActivity(Transform targetTransform, int priority)
    {
        Vector2 returnDirection = (targetTransform.position - transform.position).normalized;
        ObjectInfo targetObjectInfo = targetTransform.gameObject.GetComponent<ObjectInfo>();
        if (priorityRanking[priority] == BuildDB.MoveAction.Flee)
        {
            returnDirection *= -1;
        }
        else if (targetObjectInfo.resource == true &&
                 Vector3.Distance(targetTransform.position, transform.position) <= unitStats.lootingRange &&
                 unitStateMachine.currentlyLooting == false)
        {
            unitLootActions.InitiateLooting(targetTransform);

        }
        else if (targetObjectInfo.team != unitObjectInfo.team &&
                 Vector3.Distance(targetTransform.position, transform.position) <= unitStats.attackRange &&
                 unitStateMachine.currentlyAttacking == false)
        {
            unitCombat.InitiateCombat(targetTransform.gameObject);
        }
        return returnDirection;
    }
    
    
    
    
}
