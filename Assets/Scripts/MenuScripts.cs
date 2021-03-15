using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScripts : MonoBehaviour
{
    public GameObject priorityToolHolder;
    public GameObject[] arrayOfCombatants;
    public bool priorityToolActive;
    public bool pause;

    private void Start()
    {
        PauseGame();
        arrayOfCombatants = GameObject.FindGameObjectsWithTag("Combatant");
    }

    public void TogglePriorityHolder()
    {
        if (priorityToolActive == true)
        {
            PauseGame();
            priorityToolHolder.SetActive(false);
            priorityToolActive = false;
        }
        else
        {
            ResumeGame();
            priorityToolHolder.SetActive(true);
            priorityToolActive = true;
        }
    }

    public void TogglePause()
    {
        if (pause == true)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        foreach(var combatant in arrayOfCombatants)
        {
            combatant.GetComponent<Combatant>().PauseObject();
        }
    }

    private void ResumeGame()
    {
        foreach (var combatant in arrayOfCombatants)
        {
            combatant.GetComponent<Combatant>().ResumeObject();
        }
    }

}
