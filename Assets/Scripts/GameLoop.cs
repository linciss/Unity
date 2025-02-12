using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    private int playerIndex = 0;
    private DiceRoll dice;
    private int rolledNumber;
    private GameObject playerObject;
    private GameObject[] players;
    public List<Transform> waypoints = new List<Transform>();
    private bool isMoving = false;
    private bool hasRolled = false;
    private bool isPlayerTurn = true;

    Dictionary<int, int> playerPositions = new Dictionary<int, int>();

    void Start()
    {
        playerObject = GameObject.Find("Players");
        var childrenCount = playerObject.transform.childCount;
        players = new GameObject[childrenCount];

        for (int i = 0; i < childrenCount; i++)
        {
            players[i] = playerObject.transform.GetChild(i).gameObject;
            playerPositions.Add(i, 0);
        }

        dice = FindObjectOfType<DiceRoll>();
    
    }

    void Update()
    {
        if (dice == null)
        {
            Debug.LogError("DiceRoll not found!");
            return;
        }

        if(isMoving) return;


        //  for some reaosn this works DO NOT TOUCH IT!~!!!!
        if (isPlayerTurn)
        {
            if (dice.isLanded && !hasRolled)    
            {
                rolledNumber = Int32.Parse(dice.rolledNumber);
                Math.Max(1, rolledNumber);

                int prevTile = playerPositions[playerIndex];
                int total = playerPositions[playerIndex] += rolledNumber;
                int playerTotal = total;
               
                if(total > waypoints.Count){
                    var diff = total - waypoints.Count;
                    playerTotal = waypoints.Count - diff;
                }else{
                    playerTotal = playerPositions[playerIndex];
                }

                playerPositions[playerIndex] = playerTotal;

                StartCoroutine(movePlayer(prevTile, total -1));

                isMoving = true;
                hasRolled = true;

                dice.resetDice();
            }
        }
        else
        {
            if (!hasRolled)
            {
                dice.rollDice();
                isPlayerTurn = true;
            }
        }
    }

    private IEnumerator movePlayer(int startIndex, int endIndex){
        isMoving = true;
        GameObject currentPlayer = players[playerIndex];
        // 8
        int currentWaypointIndex = (startIndex == 0) ? 0 : startIndex - 1;
        
        if (currentWaypointIndex + 1 > endIndex){
            yield return StartCoroutine(moveWaypoint(currentPlayer, currentWaypointIndex));
            playerPositions[playerIndex] = currentWaypointIndex;
              // 14 > 11 - 1
        }else if (endIndex > waypoints.Count - 1){
            // 14 - 11 = 3
            int bounce = endIndex - (waypoints.Count - 1);

            // 8 + 1
            for (int i = currentWaypointIndex + 1; i < waypoints.Count; i++)
            {
                yield return StartCoroutine(moveWaypoint(currentPlayer, i));
                currentWaypointIndex = i;
            }
            // (11- 1) - 3 = 7
            int bounceTarget = (waypoints.Count - 1) - bounce;
            // 10 - 1  9>=7 
            for (int i = (waypoints.Count - 1) - 1; i >= bounceTarget; i--)
            {
                yield return StartCoroutine(moveWaypoint(currentPlayer, i));
                currentWaypointIndex = i;
            }
            playerPositions[playerIndex] = currentWaypointIndex + 1;
        }else{
            //  0 + 1 
            for (int i = currentWaypointIndex + 1; i <= endIndex; i++)
            {
                yield return StartCoroutine(moveWaypoint(currentPlayer, i));
                currentWaypointIndex = i;
            }
            playerPositions[playerIndex] = currentWaypointIndex + 1;
        }


        currentPlayer.transform.position = new Vector3(
            waypoints[currentWaypointIndex].position.x,
            0.489f,
            waypoints[currentWaypointIndex].position.z
        );

        isMoving = false;
        hasRolled = false;

        if (playerIndex == 0)
        {
            isPlayerTurn = false;
            playerIndex = 1;
        }
        else
        {
            isPlayerTurn = true;
            playerIndex = 0;
        }
    }

    private IEnumerator moveWaypoint(GameObject player, int idx){
        Vector3 target = new Vector3(
            waypoints[idx].position.x,
            0.489f,
            waypoints[idx].position.z
        );
        
        while (Vector3.Distance(player.transform.position, target) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                target,
                Time.deltaTime * 3
            );
            yield return null;
        }
        
        player.transform.position = target;
        yield return null;
    }

}