using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//using Random = Unity.Mathematics.Random;

public class Combatant : MonoBehaviour
{


    //Priority related variable declarations
    public GameObject spriteHolder;
    public Color storedColor; // shows when unit is selected
    public Vector3 centerOfCircle;
    public PriorityUI priorityManager;
    public bool isDead = false;
    public bool paused;
    public Combatant combatantComponent;
    public BuildDB unitDB;
    
    private void Awake()
    {
        storedColor = spriteHolder.GetComponent<SpriteRenderer>().color;
        combatantComponent = GetComponent<Combatant>();
        unitDB = GetComponent<BuildDB>();
    }
    
    private void Start()
    {
        priorityManager = GameObject.FindGameObjectWithTag("PriorityManager").GetComponent<PriorityUI>();
        isDead = false;
    }

    public void StartRound()
    {
        paused = false;
    }
    
    private void OnMouseDown()
    {
        ChangeColorWhenSelected();
        priorityManager.DeselectUnit();
        priorityManager.ResetUnitSelection(unitDB);
    }
    public void ResetColor()
    {
        spriteHolder.GetComponent<SpriteRenderer>().color = storedColor;
    }
    public void ChangeColorWhenSelected()
    {
        spriteHolder.GetComponent<SpriteRenderer>().color = Color.black; 
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


