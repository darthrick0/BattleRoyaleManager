using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PriorityUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject selectedUnit;
    public Combatant selectedUnitCombatant;
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
    public void ResetUnitSelection(Combatant newUnitCombatant)
    {
        selectedUnit = newUnitCombatant.gameObject;
        selectedUnitCombatant = newUnitCombatant;
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
        if (selectedUnitCombatant.teamAction == Combatant.MoveAction.Flee) teamMateSlider.value = 0;
        else if (selectedUnitCombatant.teamAction == Combatant.MoveAction.Ignore) teamMateSlider.value = 1;
        else teamMateSlider.value = 2;

        if (selectedUnitCombatant.enemyAction == Combatant.MoveAction.Flee) enemySlider.value = 0;
        else if (selectedUnitCombatant.enemyAction == Combatant.MoveAction.Ignore) enemySlider.value = 1;
        else enemySlider.value = 2;

        if (selectedUnitCombatant.resourceAction == Combatant.MoveAction.Flee) resourceSlider.value = 0;
        else if (selectedUnitCombatant.resourceAction == Combatant.MoveAction.Ignore) resourceSlider.value = 1;
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
        if (selection == 0) selectedUnitCombatant.teamAction = Combatant.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.teamAction = Combatant.MoveAction.Ignore;
        else selectedUnitCombatant.teamAction = Combatant.MoveAction.Pursue;
    }
    public void SetResourceAction(float selection)
    {
        if (selection == 0) selectedUnitCombatant.resourceAction = Combatant.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.resourceAction = Combatant.MoveAction.Ignore;
        else selectedUnitCombatant.resourceAction = Combatant.MoveAction.Pursue;
    }
    public void SetEnemyAction(float selection)
    {
        if (selection == 0) selectedUnitCombatant.enemyAction = Combatant.MoveAction.Flee;
        else if (selection == 1) selectedUnitCombatant.enemyAction = Combatant.MoveAction.Ignore;
        else selectedUnitCombatant.enemyAction = Combatant.MoveAction.Pursue;
    }
}
