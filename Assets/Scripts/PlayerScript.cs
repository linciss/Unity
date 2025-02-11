using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using System.IO;


public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;
    int[] otherPlayers;
    int index;

    private const string textFileName = "playerNames";

    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject mainCharacter = Instantiate(playerPrefabs[characterIndex], 
            spawnPoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(
            PlayerPrefs.GetString("PlayerName"));

        // Vēlāk vēl turpināsim...
        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] nameArr = readLinesFromFile(textFileName);

        for(int i =0; i<otherPlayers.Length-1; i++)
        {
            spawnPoint.transform.position += new Vector3(0.2f, 0, 0.2f);
            index = Random.Range(0, playerPrefabs.Length);
            GameObject character = Instantiate(playerPrefabs[index],
                spawnPoint.transform.position, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(
                nameArr[Random.Range(0, nameArr.Length - 1)]);
                    
        }
    }

    public string[] readLinesFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (!textAsset){
            Debug.LogError("file not found!" + fileName);
            return new string[0];
        }
        return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

    }
}
