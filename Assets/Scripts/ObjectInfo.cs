using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInfo : MonoBehaviour
{
    //Any object with a collider should have one of these
    public bool combatant;
    public bool resource;
    public int team;

    public TextMeshProUGUI infoText;

    private void Start()
    {
        if (combatant == true)
        {
            infoText.text = team.ToString();
        }
        else
        {
            infoText.text = "R";
        }
    }

    public void ChangeTextToEmpty()
    {
        infoText.text = "E";
    }


}
