using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PriorityUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject selectedUnit;
    public BuildDB selectedUnitCombatant;
    public Slider teamMateSlider;
    public Slider resourceSlider;
    public Slider enemySlider;
    public TMP_InputField teamMateInputField;
    public TMP_InputField resourceInputField;
    public TMP_InputField enemyInputField;

    public GameObject priorityCanvas;



    public void StartRound()
    {
        DisablePrioritiesUI();
        GameObject[] arrayOfCombatants = GameObject.FindGameObjectsWithTag("Combatant");
        
        foreach (GameObject combatant in arrayOfCombatants)
        {
            combatant.GetComponent<Combatant>().StartRound();
            combatant.GetComponent<CombatantStateMachine>().StartRound();
            combatant.GetComponent<BuildDB>().StartRound();
        }
        GameObject[] arrayOfResources = GameObject.FindGameObjectsWithTag("Resource");
        foreach (GameObject combatant in arrayOfResources)
        {
            combatant.GetComponent<Resource>().StartBattleRoyale();
        }
    }
    public void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.GetComponent<Combatant>().ResetColor();
        }
        
    } 
    public void ResetUnitSelection(BuildDB newUnitBuildDB)
    {
        selectedUnit = newUnitBuildDB.gameObject;
        selectedUnitCombatant = newUnitBuildDB;
        EnablePrioritiesUI();
        ResetUI();

    }
    public void DisablePrioritiesUI()
    {
        priorityCanvas.SetActive(false);
    }
    public void EnablePrioritiesUI()
    {
        priorityCanvas.SetActive(true);
    }
    public void ResetUI()
    {
        if (selectedUnitCombatant.teamAction == BuildDB.MoveAction.Flee) teamMateSlider.value = 0;
        else if (selectedUnitCombatant.teamAction == BuildDB.MoveAction.Ignore) teamMateSlider.value = 1;
        else teamMateSlider.value = 2;

        if (selectedUnitCombatant.enemyAction == BuildDB.MoveAction.Flee) enemySlider.value = 0;
        else if (selectedUnitCombatant.enemyAction == BuildDB.MoveAction.Ignore) enemySlider.value = 1;
        else enemySlider.value = 2;

        if (selectedUnitCombatant.resourceAction == BuildDB.MoveAction.Flee) resourceSlider.value = 0;
        else if (selectedUnitCombatant.resourceAction == BuildDB.MoveAction.Ignore) resourceSlider.value = 1;
        else resourceSlider.value = 2;

        resourceInputField.text = selectedUnitCombatant.resourcePriority.ToString();
        teamMateInputField.text = selectedUnitCombatant.teamMatePriority.ToString();
        enemyInputField.text = selectedUnitCombatant.enemyPriority.ToString();
    }
    public void PauseGame()
    {
        GameObject[] arrayOfCombatants = GameObject.FindGameObjectsWithTag("Combatant");
        foreach (GameObject combatant in arrayOfCombatants)
        {
            combatant.GetComponent<Combatant>().StartRound();
        }
    }
    public void SetTeamMatePriorityRanking(string strRank)
    {
        Debug.Log("team mate Ranked");
        int intRank = int.Parse(strRank);
        selectedUnitCombatant.teamMatePriority = intRank;
    }
    public void SetResourcePriorityRanking(string strRank)
    {
        Debug.Log("resource Ranked");
        int intRank = int.Parse(strRank);
        selectedUnitCombatant.resourcePriority = intRank;
    }
    public void SetEnemyPriorityRanking(string strRank)
    {
        Debug.Log("enemy Ranked");
        int intRank = int.Parse(strRank);
        selectedUnitCombatant.enemyPriority = intRank;
    }
    public void SetTeamMateAction(float selection)
    {
        if (selection == 0) selectedUnitCombatant.teamAction = BuildDB.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.teamAction = BuildDB.MoveAction.Ignore;
        else selectedUnitCombatant.teamAction = BuildDB.MoveAction.Pursue;
    }
    public void SetResourceAction(float selection)
    {
        if (selection == 0) selectedUnitCombatant.resourceAction = BuildDB.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.resourceAction = BuildDB.MoveAction.Ignore;
        else selectedUnitCombatant.resourceAction = BuildDB.MoveAction.Pursue;
    }
    public void SetEnemyAction(float selection)
    {
        if (selection == 0) selectedUnitCombatant.enemyAction = BuildDB.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.enemyAction = BuildDB.MoveAction.Ignore;
        else selectedUnitCombatant.enemyAction = BuildDB.MoveAction.Pursue;
    }
}
