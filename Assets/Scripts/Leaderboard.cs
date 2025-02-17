using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{

    string leaderboardFile = "leaderboard.json";
    // [SerializeField] private GameObject names;
    // [SerializeField] private GameObject scores;
    // [SerializeField] private GameObject times;

    [SerializeField] private GameObject original;
    [SerializeField] private Transform content;


    [Serializable]
    public class LeaderboardEntry
    {
        public string name;
        public int score;
        public float time;
    }

    [Serializable]
    public class LeaderboardWrapper
    {
        public List<LeaderboardEntry> leaderboard;
    }

    private LeaderboardWrapper leaderboardData;

    void Start()
    {
        loadData();
    }

    public void loadData() {
        string filePath = Application.persistentDataPath + "/" + leaderboardFile;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderboardData = JsonUtility.FromJson<LeaderboardWrapper>(json);

            foreach (LeaderboardEntry entry in leaderboardData.leaderboard)
            {
                GameObject item = Instantiate(original);

                TMPro.TextMeshProUGUI name = item.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                name.text = entry.name;
                name.enableWordWrapping = false;
                name.overflowMode = TMPro.TextOverflowModes.Ellipsis;
                
                TMPro.TextMeshProUGUI  rolls = item.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                rolls.text = entry.score.ToString();

                TMPro.TextMeshProUGUI time = item.transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                time.text = entry.time.ToString();

                item.SetActive(true);
                item.transform.SetParent(content, false);

                Debug.Log("Name: " + entry.name + ", Score: " + entry.score + ", Time: " + entry.time);
            }
        }
        else
        {
            Debug.LogError("File not found");
        }
    }
}
