using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChanges : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject leaderboardMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void goBack()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        leaderboardMenu.SetActive(false);
    }
    public void goLeaderboard()
    {
        mainMenu.SetActive(false);
        leaderboardMenu.SetActive(true);
    }
}
