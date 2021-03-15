using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    public float currentBestDistance;
    public float targetDistance;

    //Priority related variable declarations
    public GameObject spriteHolder;
    public Color storedColor; // shows when unit is selected
    
    public enum MoveAction {Flee, Ignore, Pursue};
    public MoveAction teamAction = MoveAction.Ignore;
    public MoveAction resourceAction = MoveAction.Ignore;
    public MoveAction enemyAction = MoveAction.Ignore;

    public Transform currentBest;
    public Collider2D[] potentialTargets;
    public List<Transform> enemyTransforms;
    public List<Transform> teamTransforms;
    public List<Transform> resourceTransforms;

    private Dictionary<int, MoveAction> priorityRanking = new Dictionary<int, MoveAction>();
    private Dictionary<int, List<Transform>> rankedListOfTransforms = new Dictionary<int, List<Transform>>();

    public int teamMatePriority = 2;
    public int resourcePriority = 1;
    public int enemyPriority = 0;

    
    public bool roundStart;
    public bool paused;

    public float dbRunningTime;
    public float dbRebuildInterval;
    

    public bool wandering;
    public Vector2 moveDir;
    Vector2 returnDirection = new Vector2(0, 0);
    public float wanderTimer;
    public float wanderRunningCount;
    public Vector3 centerOfCircle;
    

    public GameObject resource;
    public float lootTime;
    public bool currentlyLooting = false;
    public bool currentlyAttacking = false;
    
    public float attackPower;
    public float attackRange;
    public float attackSpeed;
    public float visionRange;
    public float lootingRange;
    public float hitPoints;
    public float moveSpeed;
    public Buff currentWeapon;
    public GameObject weapon;
    public int combatantTeam;
    public Combatant combatantComponent;
    public PriorityUI priorityManager;
    

    private void Awake()
    {
        storedColor = spriteHolder.GetComponent<SpriteRenderer>().color;
        combatantTeam = GetComponent<ObjectInfo>().team;
        combatantComponent = gameObject.GetComponent<Combatant>();
    }


    private void Start()
    {
        priorityManager = GameObject.FindGameObjectWithTag("PriorityManager").GetComponent<PriorityUI>();
    }

    public void StartRound()
    {
        RebuildDB();
        priorityRanking.Add(enemyPriority, enemyAction);
        rankedListOfTransforms.Add(enemyPriority, enemyTransforms);
        priorityRanking.Add(teamMatePriority, teamAction);
        rankedListOfTransforms.Add(teamMatePriority, teamTransforms);
        priorityRanking.Add(resourcePriority, resourceAction);
        rankedListOfTransforms.Add(resourcePriority, resourceTransforms);
        wandering = false;
        roundStart = true;
        paused = false;
    }
    
    private void RebuildDB()
    {
        dbRunningTime = 0f;
        enemyTransforms.Clear();
        teamTransforms.Clear();
        resourceTransforms.Clear();

        potentialTargets = Physics2D.OverlapCircleAll(transform.position, visionRange);

        foreach (var target in potentialTargets)
        {
            var targetObjectInfo = target.gameObject.GetComponent<ObjectInfo>();
            if (target.transform != transform && targetObjectInfo.combatant == true)
            {
                if (targetObjectInfo.team != combatantTeam)
                {
                    enemyTransforms.Add(target.transform);
                }
                else
                {
                    teamTransforms.Add(target.transform);
                }
            }

            if (targetObjectInfo.resource == true &&
                target.gameObject.GetComponent<Resource>().combatantKnowledgeOfLoot.Contains(gameObject) == false)
            {
                resourceTransforms.Add(target.transform);
            }
        }
    }

    private void Update()
    {
        
        if (roundStart == true && paused == false && currentlyLooting == false && currentlyAttacking == false)
        {
            if (dbRunningTime >= dbRebuildInterval) RebuildDB();
            dbRunningTime += Time.deltaTime;
            
            moveDir = GetActivity();
            transform.Translate(moveDir * (Time.deltaTime * moveSpeed));
        }
    }

    private void OnMouseDown()
    {
        ChangeColorWhenSelected();
        priorityManager.DeselectUnit();
        priorityManager.ResetUnitSelection(combatantComponent);
    }
    public void ResetColor()
    {
        spriteHolder.GetComponent<SpriteRenderer>().color = storedColor;
        //Trash bad
        
    }
    public void ChangeColorWhenSelected()
    {
        spriteHolder.GetComponent<SpriteRenderer>().color = Color.black; 
    }
    
    private Vector2 GetActivity()
    {
        currentBestDistance = 100000f;
        Transform currentBestTransform = null;
        int storePriority = -1;
        for (int i = 0; i < rankedListOfTransforms.Count; i++)
        {
            if(priorityRanking[i] != MoveAction.Ignore)
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
            wandering = false;
        }
        //Debug.Log("currentBestTransform = " + currentBestTransform.position);
        if (currentBestTransform != null)
        {
            DetermineActivity(currentBestTransform, storePriority);
        }
        else
        {
            returnDirection = WanderTowardCenter();
        }
        return returnDirection;  
    }

    private void DetermineActivity(Transform targetTransform, int priority)
    {
        returnDirection = (targetTransform.position - transform.position).normalized;
        ObjectInfo targetObjectInfo = targetTransform.gameObject.GetComponent<ObjectInfo>();
        if (priorityRanking[priority] == MoveAction.Flee)
        {
            returnDirection *= -1;
        }
        else if (targetObjectInfo.resource == true &&
                 Vector3.Distance(targetTransform.position, transform.position) <= lootingRange)
        {
            StartCoroutine(CheckForLoot(targetTransform));
        }
        else if (targetObjectInfo.team != combatantTeam &&
                 Vector3.Distance(targetTransform.position, transform.position) <= attackRange)
        {
            StartCoroutine(Attack(targetTransform.gameObject));
        }
    }

    private Vector2 WanderTowardCenter()
    {
        wandering = true;
        var position = transform.position;
        var randomDir = new Vector2(position.x, position.y).normalized;
        return randomDir;
    }

    private IEnumerator Attack(GameObject attackTarget)
    {
        currentlyAttacking = true;
        Combatant targetCombatantComponent = attackTarget.GetComponent<Combatant>();
        //attack
        bool enemyAlive = true;
        float attackTargetDistance = Mathf.Abs(Vector3.Distance(transform.position, attackTarget.transform.position));
        while (enemyAlive == true && hitPoints > 0 && attackTargetDistance <= attackRange)
        {
            targetCombatantComponent.TakeDamage(attackPower,gameObject);
            yield return new WaitForSeconds(attackSpeed);
            if (targetCombatantComponent.hitPoints <= 0)
            {
                enemyAlive = false;
                currentlyAttacking = false;
                RebuildDB();
            }
        }
    }

    public void TakeDamage(float damageAmount, GameObject attacker)
    {
        hitPoints -= damageAmount;
        if (hitPoints <= 0)
        {
            Die();
        }
        if (currentlyAttacking == false) StartCoroutine(Attack(attacker));
    }

    private void Die()
    {
        transform.position = new Vector3(0f, 0f, -99999f);
        gameObject.SetActive(false);
    }
    
    private IEnumerator CheckForLoot(Transform lootTarget)
    {
        currentlyLooting = true;
        yield return new WaitForSeconds(lootTime);
        GameObject potentialBuffGameObject = lootTarget.gameObject.GetComponent<Resource>().LootResource();
        Buff potentialBuff = potentialBuffGameObject.GetComponent<Buff>();
        Resource lootTargetResource = lootTarget.gameObject.GetComponent<Resource>();
        
        if (potentialBuffGameObject != null)
        {
            Loot(potentialBuff, potentialBuffGameObject, lootTargetResource);
        }
        lootTargetResource.AddGameObjectToKnowledgeOfLoot(gameObject);
        RebuildDB();
        currentlyLooting = false;
    }

    private void Loot(Buff potentialBuff, GameObject potentialBuffGameObject, Resource lootTargetResource)
    {
        if (potentialBuff.isWeapon == true && currentWeapon == null)
        {
            ApplyStatChanges(potentialBuff, false);
            currentWeapon = potentialBuff;
            DisableWeaponComponents(potentialBuffGameObject);
            
            lootTargetResource.ConfirmLootTaken();
        }
        else if (potentialBuff.isWeapon == true && potentialBuff.attackRangeBonus > currentWeapon.attackRangeBonus)
        {
            ApplyStatChanges(currentWeapon, true);
            EnableWeaponComponents();
            ApplyStatChanges(potentialBuff, false);
            currentWeapon = potentialBuff;
            DisableWeaponComponents(potentialBuffGameObject);
                
            lootTargetResource.ConfirmLootTaken();
        }
        else if (potentialBuff.isWeapon == false)
        {
            UnpackBuff(potentialBuffGameObject);
            ApplyStatChanges(potentialBuff, false);
            Destroy(potentialBuffGameObject);
            lootTargetResource.ConfirmLootTaken();
        }
    }

    private void ApplyStatChanges(Buff buff, bool subtract)
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


    private void EnableWeaponComponents()
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

    private void UnpackBuff(GameObject newItem)
    {
        Buff newItemBuff = newItem.GetComponent<Buff>();
        
        ApplyStatChanges(newItemBuff, false);
        
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

    private void DisableWeaponComponents(GameObject newItem)
    {
        newItem.GetComponent<Resource>().enabled = false;
        newItem.GetComponent<ObjectInfo>().enabled = false;
        if (newItem.GetComponent<Collider2D>() != null) newItem.GetComponent<Collider2D>().enabled = false;
        newItem.transform.SetParent(transform);
        newItem.transform.position = transform.position;
    }

    public void SetCenter(Vector3 center)
    {
        centerOfCircle = center;
    }
    
    public void PauseObject()
    {
        paused = true;
    }

    public void ResumeObject()
    {
        paused = false;
    }
}


