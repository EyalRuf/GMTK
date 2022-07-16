using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI_Panels")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject LeaderBoardPanel;
    [SerializeField] private GameObject InGamePanel;

    [Header("Other")]
    [SerializeField] private LeaderboardController lc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToIngamePanel()
    {
        LeaderBoardPanel.SetActive(false);
        MainMenuPanel.SetActive(false);

        InGamePanel.SetActive(true);
    }

    public void OpenLeaderBoardFromMainMenu ()
    {
        MainMenuPanel.SetActive(false);
        lc.FetchScores();
        lc.disablePostGame();
        LeaderBoardPanel.SetActive(true);
    }

    public void OpenLeaderBoardAfterGame()
    {
        InGamePanel.SetActive(false);
        lc.FetchScores();
        lc.activatePostGame();
        LeaderBoardPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        LeaderBoardPanel.SetActive(false); 
        LeaderBoardPanel.SetActive(false);
        InGamePanel.SetActive(false);

        lc.refreshPostGame();

        MainMenuPanel.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
