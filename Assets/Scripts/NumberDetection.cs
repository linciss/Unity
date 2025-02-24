using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDetection : MonoBehaviour
{
    // Start is called before the first frame update
    DiceRoll diceRoll;
    void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider sideCollider)
    {
        if (diceRoll == null) Debug.LogError("Not found");
        if (diceRoll.GetComponent<Rigidbody>().velocity == Vector3.zero){
            diceRoll.isLanded = true;
            diceRoll.rolledNumber = sideCollider.name;
            diceRoll.rolledNumberUI = sideCollider.name;
        }
        else
        {
            diceRoll.isLanded = false;
        }
    }
}
