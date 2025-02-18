using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // leaderboardData.leaderboard.Sort((x, y) => y.score.CompareTo(x.score));
            var sortedLeaderboard = leaderboardData.leaderboard.OrderBy(x => x.score).ToArray();

            var podium = 0;

            foreach (LeaderboardEntry entry in sortedLeaderboard)
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

                switch (podium)
                {
                    case 0:
                        name.color = new Color32(255, 215, 0, 255);
                        rolls.color = new Color32(255, 215, 0, 255);
                        time.color = new Color32(255, 215, 0, 255);
                        podium++;
                        break;
                    case 1:
                        name.color = new Color32(192, 192, 192, 255);
                        rolls.color =new Color32(192, 192, 192, 255);
                        time.color = new Color32(192, 192, 192, 255);
                        podium++;
                        break;
                    case 2:
                        name.color = new Color32(205, 127, 50, 255);
                        rolls.color = new Color32(205, 127, 50, 255);
                        time.color = new Color32(205, 127, 50, 255);
                        podium++;
                        break;
                }

                item.SetActive(true);
                item.transform.SetParent(content, false);

            }
        }
        else
        {
            Debug.LogError("File not found");
        }
    }
}
