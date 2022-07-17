using UnityEngine.UI;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerNameInput;
    [SerializeField] private int leaderboardID;

    [SerializeField] private int leaderboardSize;
    [SerializeField] private TextMeshProUGUI[] leaderboardNames;
    [SerializeField] private TextMeshProUGUI[] leaderboardScores;

    [SerializeField] private GameObject submitScorePanel;
    [SerializeField] private Button submitScoreBtn;
    [SerializeField] private TextMeshProUGUI submitScoreBtnText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject playAgainBtn;

    // Start is called before the first frame update
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
    }

    public void SubmitScoreBtn()
    {
        // Set actual score here NOT 10!
        SubmitScore(PlayerNameInput.text, leaderboardSize);
        submitScoreBtn.interactable = false;
        submitScoreBtnText.text = "Submitted!";
    }
    
    void SubmitScore(string playerName, int playerScore)
    {
        LootLockerSDKManager.SubmitScore(playerName, playerScore, leaderboardID, (res) =>
        {
            if (res.success)
            {
                Debug.Log("Leaderboard Submit Score Success");
                FetchScores();
            }
            else
                Debug.LogError("Leaderboard Submit Score Failed");
        });
    }

    public void FetchScores ()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, leaderboardSize, (res) =>
        {
            if (res.success)
                for (int i = 0; i < leaderboardSize; i++)
                {
                    if (res.items.Length > i)
                    {
                        leaderboardNames[i].text = res.items[i].member_id;
                        leaderboardScores[i].text = res.items[i].score + "";
                    }
                    else
                    {
                        leaderboardNames[i].text = "None";
                        leaderboardScores[i].text = "0";
                    }
                }
            else
                Debug.LogError("Leaderboard Submit Score Failed");
        });
    }

    internal void activatePostGame(int score)
    {
        refreshPostGame();
        scoreText.text = score + "";
        submitScorePanel.SetActive(true);
        playAgainBtn.SetActive(true);
    }
    internal void refreshPostGame()
    {
        scoreText.text = "";
        submitScoreBtn.interactable = true;
        submitScoreBtnText.text = "Submit Score";
    }

    internal void disablePostGame()
    {
        submitScorePanel.SetActive(false);
        playAgainBtn.SetActive(false);
    }
}
