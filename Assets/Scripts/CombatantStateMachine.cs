using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CombatantStateMachine : MonoBehaviour
{
    public bool isDead = false;
    public bool currentlyLooting = false;
    public bool currentlyAttacking = false;
    public bool roundStart;
    public bool paused;
    public bool attackMove;
    public bool wandering;

    private DecideActivity unitDecider;

    private void Awake()
    {
        unitDecider = GetComponent<DecideActivity>();
    }

    private void Start()
    {
        isDead = false;
        currentlyLooting = false;
        currentlyAttacking = false;
        roundStart = false;
        paused = true;
        attackMove = false;
        wandering = false;
    }

    private void Update()
    {
        unitDecider.GetActivity();
    }
    
    public void StartRound()
    {
        roundStart = true;
        paused = false;
    }

    public void EndRound()
    {
        roundStart = false;
        paused = true;
        wandering = false;
    }
}
