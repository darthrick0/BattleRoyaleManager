using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class GameManager : MonoBehaviour
{
    private Vector3 mapCenter;
    private Vector3 centerOfCircle;
    public float centerVariance;
    public GameObject[] arrayOfCombatants;
    public GameObject map;

    private void Start()
    {
        arrayOfCombatants = GameObject.FindGameObjectsWithTag("Combatant");
        mapCenter = map.transform.position;
    }
    public void StartRoyale()
    {
        DetermineCenterOfCircle();
        SendCenterOfCircle();
    }
    private void DetermineCenterOfCircle()
    {
        float mapWidth = map.GetComponent<Rect>().width;
        float mapHeight = map.GetComponent<Rect>().height;
        centerOfCircle = new Vector3(
            Random.Range(-0.5f * centerVariance, 0.5f * centerVariance) * mapWidth,
            Random.Range(-0.5f * centerVariance, 0.5f * centerVariance) * mapHeight,
            0);

    }
    private void SendCenterOfCircle()
    {
        foreach (var combatant in arrayOfCombatants)
        {
            combatant.GetComponent<Combatant>().SetCenter(centerOfCircle);
        }
    }


}
