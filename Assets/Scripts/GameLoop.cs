using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private int[] badFields = {19, 32, 11}; //19, 32, 11
    private int[] goodFields = {7, 14, 15};// 7, 14, 15


    // good fields 19   32   11
                // to   
    // bad fields 8,    14  15

    Dictionary<int, int> playerPositions = new Dictionary<int, int>();
    public bool gameWon = false;
    private List<int> aiWon = new List<int>();
    public bool isAITurn = false;
    float time = 0.0f;

    public bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject wonMenu;
    [SerializeField] private TMPro.TextMeshProUGUI rolls;
    [SerializeField] private TMPro.TextMeshProUGUI timeToWin;



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

        if(gameWon == true){
            return;
        }

        if (dice == null)
        {
            Debug.LogError("DiceRoll not found!");
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            gamePaused = !gamePaused;
            pauseMenu.SetActive(gamePaused);
        }

        if(gamePaused) return;

        time += Time.deltaTime;

        if(isMoving) return;



        if (isPlayerTurn){
            if (dice.isLanded && !hasRolled){
                if(((playerIndex + 1) % players.Length+1) != 0) {
                    isAITurn = true;
                }else{
                    isAITurn = false;
                }; 
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
        }else{
            if (!hasRolled)
            {
                if (!aiWon.Contains(playerIndex)){
                    dice.rollDice();
                }else{
                    playerIndex = (playerIndex + 1) % players.Length;
                    if (playerIndex == 0) { 
                        isAITurn = false;
                    }
                    else if(!aiWon.Contains(playerIndex)){
                        dice.rollDice();
                    }
                }
                isPlayerTurn = true;
            }
        }
    }

    private IEnumerator movePlayer(int startIndex, int endIndex){
        isMoving = true;

        // extra checks just in case

        if (aiWon.Contains(playerIndex)){
            isMoving = false;
            hasRolled = false;

            isAITurn = true;
            playerIndex = (playerIndex + 1) % players.Length;
            isPlayerTurn = false;
            if (playerIndex == 0){
                isAITurn = false;
                isPlayerTurn = true;
            }

            yield break;
        }

        GameObject currentPlayer = players[playerIndex];

        int currentWaypointIndex = (startIndex == 0) ? 0 : startIndex - 1;



        // 8
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
            endIndex = currentWaypointIndex;
        }else{
            //  0 + 1 
            for (int i = currentWaypointIndex + 1; i <= endIndex; i++)
            {
                yield return StartCoroutine(moveWaypoint(currentPlayer, i));
                currentWaypointIndex = i;
            }
        }

        if(goodFields.Contains<int>(endIndex+1)){
            int goodFieldIndex = Array.IndexOf(goodFields, endIndex + 1);
            int badFieldIndex  = badFields[goodFieldIndex];

            yield return StartCoroutine(moveWaypoint(currentPlayer, badFieldIndex - 1));
            playerPositions[playerIndex] = badFieldIndex;
            currentWaypointIndex = badFieldIndex - 1;
    
        }else if(badFields.Contains<int>(endIndex+1)){
            int badFieldIndex = Array.IndexOf(badFields, endIndex + 1);
            int goodFieldIndex  = goodFields[badFieldIndex];
            yield return StartCoroutine(moveWaypoint(currentPlayer, goodFieldIndex - 1));
            playerPositions[playerIndex] = goodFieldIndex;
            currentWaypointIndex = goodFieldIndex - 1;
        }

        currentPlayer.transform.position = new Vector3(
            waypoints[currentWaypointIndex].position.x,
            0.489f,
            waypoints[currentWaypointIndex].position.z
        );

        isMoving = false;
        hasRolled = false;
        // 
        if (playerPositions[playerIndex] == waypoints.Count){
            if (playerIndex == 0){
                gameWon = true;
                saveScore();
                timeToWin.text = "Time: " + (int)time + " seconds";
                rolls.text = "Times rolled: " + dice.timesThrown;
                wonMenu.SetActive(true);
            }
            else{
                if (!aiWon.Contains(playerIndex) && playerIndex != 0)
                {
                    aiWon.Add(playerIndex);
                }
            }
        }

        if (playerIndex == 0 && aiWon.Count != (players.Count() - 1)){
            isPlayerTurn = false;
            playerIndex = (playerIndex + 1) % players.Length;
            isAITurn = true;
        }else{
            isAITurn = true;
            isPlayerTurn = false;
            if(aiWon.Count != (players.Count() - 1))
                playerIndex = (playerIndex + 1) % players.Length;
            if (playerIndex == 0){
                isAITurn = false;
                isPlayerTurn = true;
            }
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



    [Serializable]
    public class LeaderboardEntry{
        public string name;
        public int score;
        public float time;
    }

    [Serializable]
    public class LeaderboardWrapper{
        public List<LeaderboardEntry> leaderboard;
    }

    private void saveScore(){
        string leaderboardFile = "leaderboard.json";
        string filePath = Application.persistentDataPath + "/" + leaderboardFile;

        if (File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            LeaderboardWrapper leaderboardData = JsonUtility.FromJson<LeaderboardWrapper>(json);

            LeaderboardEntry newEntry = new LeaderboardEntry();
            newEntry.name = PlayerPrefs.GetString("PlayerName");
            newEntry.score = dice.timesThrown;
            newEntry.time = (int)time;

            leaderboardData.leaderboard.Add(newEntry);
            string newJson = JsonUtility.ToJson(leaderboardData);
            File.WriteAllText(filePath, newJson);

        }else{
            LeaderboardWrapper leaderboardData = new LeaderboardWrapper();
            LeaderboardEntry newEntry = new LeaderboardEntry();
            newEntry.name = PlayerPrefs.GetString("PlayerName");
            newEntry.score = dice.timesThrown;
            newEntry.time = (int)time;

            leaderboardData.leaderboard = new List<LeaderboardEntry>();
            leaderboardData.leaderboard.Add(newEntry);

            string newJson = JsonUtility.ToJson(leaderboardData);
            File.WriteAllText(filePath, newJson);
        }

    }

    public void continueGame(){
        gamePaused = false;
        pauseMenu.SetActive(false);
    }
}