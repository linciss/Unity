using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RolledNumber : MonoBehaviour
{
    DiceRoll diceRoll;
    [SerializeField] TMP_Text rolledNumberText;

    void Awake()
    {
        diceRoll = FindObjectOfType<DiceRoll>();    
    }

    void Update()
    {
        if (diceRoll == null) Debug.LogError("Not found dice roll");

        if (diceRoll.isLanded)
        {
            rolledNumberText.text = diceRoll.rolledNumberUI;
        }
        else
        {
            rolledNumberText.text = "?";
        }
        
    }
}
