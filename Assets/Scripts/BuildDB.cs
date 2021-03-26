using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDB : MonoBehaviour
{
    
    public List<Transform> enemyTransforms;
    public List<Transform> teamTransforms;
    public List<Transform> resourceTransforms;

    
    public enum MoveAction {Flee, Ignore, Pursue};
    public MoveAction teamAction = MoveAction.Ignore;
    public MoveAction resourceAction = MoveAction.Ignore;
    public MoveAction enemyAction = MoveAction.Ignore;
    
    
    private Dictionary<int, MoveAction> priorityRanking = new Dictionary<int, MoveAction>();
    private Dictionary<int, List<Transform>> rankedListOfTransforms = new Dictionary<int, List<Transform>>();
    public int teamMatePriority = 2;
    public int resourcePriority = 1;
    public int enemyPriority = 0;
    
    public Transform currentBest;
    public Collider2D[] potentialTargets;

    private CombatantStats unitStats;
    private ObjectInfo combatantObjectInfo;
    private CombatantStateMachine unitSM;
    

    private void Awake()
    {
        unitStats = GetComponent<CombatantStats>();
        combatantObjectInfo = GetComponent<ObjectInfo>();
        unitSM = GetComponent<CombatantStateMachine>();
    }
    
    public void StartRound()
    {
        priorityRanking.Add(enemyPriority, enemyAction);
        rankedListOfTransforms.Add(enemyPriority, enemyTransforms);
        priorityRanking.Add(teamMatePriority, teamAction);
        rankedListOfTransforms.Add(teamMatePriority, teamTransforms);
        priorityRanking.Add(resourcePriority, resourceAction);
        rankedListOfTransforms.Add(resourcePriority, resourceTransforms);
    }
    
    
    
    public Dictionary<int, MoveAction> GetPriorityRanking()
    {
        return priorityRanking;
    }

    public Dictionary<int, List<Transform>> GetRankedListOfTransforms()
    {
        return rankedListOfTransforms;
    }
    
    
    public void RebuildDB()
    {
        enemyTransforms.Clear();
        teamTransforms.Clear();
        resourceTransforms.Clear();
        potentialTargets = Physics2D.OverlapCircleAll(transform.position, unitStats.visionRange);

        foreach (var target in potentialTargets)
        {
            var targetObjectInfo = target.gameObject.GetComponent<ObjectInfo>();
            if (target.transform != transform && targetObjectInfo.combatant == true)
            {
                if (targetObjectInfo.team != combatantObjectInfo.team)
                {
                    enemyTransforms.Add(target.transform);
                }
                else
                {
                    teamTransforms.Add(target.transform);
                }
            }

            if (targetObjectInfo.resource == true &&
                target.gameObject.GetComponent<Resource>().DoesCombatantKnowOfLoot(gameObject) == true)
            {
                resourceTransforms.Add(target.transform);
            }
        }
    }

    private bool CheckForAnything()
    {
        var checkForAnyTargets = Physics2D.OverlapCircleAll(transform.position, unitStats.visionRange);
        return checkForAnyTargets.Length > 1;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (unitSM.roundStart && CheckForAnything())
        {
            RebuildDB();
        }
    }
    

}
