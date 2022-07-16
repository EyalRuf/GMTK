using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("UI_Panels")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject LeaderBoardPanel;
    [SerializeField] private GameObject InGamePanel;

    [Header("Other")]
    [SerializeField] private LeaderboardController leaderboard;

    [Header("LevelLoader")]
    public Animator transition;
    public float transitionTime = 1f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (FindObjectsOfType<MenuController>().Length > 1)
        {
            Destroy(gameObject);
        }
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
        leaderboard.FetchScores();
        leaderboard.disablePostGame();
        LeaderBoardPanel.SetActive(true);
    }

    public void OpenLeaderBoardAfterGame()
    {
        InGamePanel.SetActive(false);
        leaderboard.FetchScores();

        // SET ACTUAL SCORE
        leaderboard.activatePostGame(10);

        LeaderBoardPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        LeaderBoardPanel.SetActive(false); 
        LeaderBoardPanel.SetActive(false);
        InGamePanel.SetActive(false);

        leaderboard.refreshPostGame();

        MainMenuPanel.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    // LEVEL LOADER
    //function used to load the next level/scene 
    public void LoadGameLevel()
    {
        StartCoroutine(LoadGameLevelCoRoutine());
    }

    public void LoadMenuLevel()
    {
        StartCoroutine(LoadMenuLevelCoRoutine());
    }

    IEnumerator LoadGameLevelCoRoutine()
    {
        transition.SetTrigger("StartCrossfade");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(1);
        
        transition.ResetTrigger("StartCrossfade");

        ShowGamePanels();

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("StartCrossfade");
    }
    IEnumerator LoadMenuLevelCoRoutine()
    {
        transition.SetTrigger("StartCrossfade");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(0);

        transition.ResetTrigger("StartCrossfade");

        ShowMenuPanels();

        yield return new WaitForSeconds(transitionTime);
        
        transition.SetTrigger("StartCrossfade");
    }

    private void ShowMenuPanels()
    {
        OpenLeaderBoardAfterGame();
    }

    private void ShowGamePanels()
    {
        GoToIngamePanel();
    }
}
