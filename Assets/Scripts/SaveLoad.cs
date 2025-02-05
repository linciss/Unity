using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    string saveFileName = "saveFile.json";
    [Serializable] public class GameData
    {
        public int characterIndex;
        public string playerName;
    }

    private GameData gameData = new GameData();

    public void saveGame(int character, string name)
    {
        gameData.characterIndex = character;
        gameData.playerName = name;

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/" + saveFileName, json);

        Debug.Log("Game saved to " + Application.persistentDataPath + "/" + saveFileName);
    }

    public void loadGame()
    {
        string filePath = Application.persistentDataPath + "/" + saveFileName;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            gameData = JsonUtility.FromJson<GameData>(json);

            Debug.Log("game loaded from " + filePath + " char index " + gameData.characterIndex + " player name " + gameData.playerName);

        }
        else
        {
            Debug.LogError("FIle not found");
        }
    }
}
