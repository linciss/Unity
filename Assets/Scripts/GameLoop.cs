using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] tiles;
    private int playerIndex = 0;
    private DiceRoll dice;
    private int rolledNumber;
    private GameObject playerObject;
    private GameObject[] players;
    public List<Transform> waypoints = new List<Transform>();
    private bool isMoving = false;
    private Vector3 targetPosition;
    private bool playerRolled = true;

    void Start()
    {
        playerObject = GameObject.Find("Players");
        var childrenCount = playerObject.transform.childCount;
        players = new GameObject[childrenCount];

        for(int i=0; i<childrenCount; i++)
        {
            players[i] = playerObject.transform.GetChild(i).gameObject;
        }

        dice = FindObjectOfType<DiceRoll>();

    }

    // Update is called once per frame
    void Update()
    {

        if (dice == null)
        {
            Debug.LogError("DiceRoll not found!");
            return;
        }

        if (isMoving)
        {
            movePlayer();
            return; 
        }

        if (dice.isLanded)
        {
            rolledNumber = Int32.Parse(dice.rolledNumber)-1;
            Debug.Log("Player rolled: " + rolledNumber);

            targetPosition = new Vector3(
                waypoints[rolledNumber].position.x,
                0.489f,  
                waypoints[rolledNumber].position.z
            );

            isMoving = true;
        }

        if (!isMoving && dice.isLanded)
        {
            nextPlayerTurn();
        }
    }

    private void movePlayer()
    {
        GameObject currentPlayer = players[playerIndex];


        playerIndex++;
        if (playerIndex >= players.Length)
        {
            playerIndex = 0;
        }

        if (playerIndex == 0) playerRolled = true;

        if (currentPlayer.transform.position == targetPosition && playerRolled)
        {
            isMoving = false;
            playerRolled = false;
            nextPlayerTurn();
            return;
        }
        currentPlayer.transform.position = Vector3.MoveTowards(currentPlayer.transform.position, targetPosition, Time.deltaTime * 3);
    }
    private void nextPlayerTurn()
    {
       // playerIndex = (playerIndex + 1) % players.Length;

        if (playerIndex != 0)
        {
            rolledNumber = UnityEngine.Random.Range(1, 7); 
            Debug.Log("Computer player rolled: " + rolledNumber);

            targetPosition = new Vector3(
                waypoints[rolledNumber].position.x,
                0.489f, 
                waypoints[rolledNumber].position.z
            );

            isMoving = true;
        }

    }
}
