using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public InterstitialAdManager interstitialAd;

    public static GameManager Instance;
    public GameObject winPopup;
    public GameObject losePopup;
    public GameObject pausePopup;

    private int totalSkeletons = 0;
    private int deadSkeletons = 0;

    public int timeScale = 1;

    public TextMeshProUGUI leaderboardText;
    public TextMeshProUGUI playerScoreText;

    void Awake()
    {
        Time.timeScale = timeScale;
        Instance = this;
    }

    public void RegisterSkeleton()
    {
        totalSkeletons++;
    }

    public void SkeletonDied()
    {
        deadSkeletons++;
        if (deadSkeletons >= totalSkeletons)
        {
            _ = WinGame();
        }
    }

    public void SkeletonsWin()
    {
        SoldierGridManager.Instance.SolidiersLost();
        _ = LoseGame();
    }

    async Task WinGame()
    {
        try
        {
            await Task.Delay(1000);

            Debug.Log("Checking SoldierGridManager...");
            if (SoldierGridManager.Instance == null) { Debug.Log("SoldierGridManager is NULL"); return; }

            Debug.Log("Checking LeaderboardManager...");
            if (LeaderboardManager.Instance == null) { Debug.Log("LeaderboardManager is NULL"); return; }

            Debug.Log("Getting most soldiers...");
            int mostSoldiers = SoldierGridManager.Instance.GetMostSoldiers();
            Debug.Log($"Most soldiers: {mostSoldiers}");

            Debug.Log("Submitting score...");
            await LeaderboardManager.Instance.SubmitScore(mostSoldiers);

            Time.timeScale = 0f;
            winPopup.SetActive(true);

            Debug.Log("Checking leaderboard text fields...");
            if (leaderboardText == null) { Debug.Log("leaderboardText is NULL"); return; }
            if (playerScoreText == null) { Debug.Log("playerScoreText is NULL"); return; }

            await ShowLeaderboard(mostSoldiers);
        }
        catch (System.Exception e)
        {
            Debug.Log($"WinGame error: {e.Message}");
        }
    }

    async Task LoseGame()
    {
        try
        {
            await Task.Delay(1000);

            int mostSoldiers = SoldierGridManager.Instance.GetMostSoldiers();
            await LeaderboardManager.Instance.SubmitScore(mostSoldiers);

            losePopup.SetActive(true);
            Time.timeScale = 0f;
        }
        catch (System.Exception e)
        {
            Debug.Log($"LoseGame error: {e.Message}");
        }
    }

    public void PlayAgain()
    {
        interstitialAd.ShowAd();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePopup.SetActive(true);
    }

    public void ResumeGame()
    {
        pausePopup.SetActive(false);
        Time.timeScale = timeScale;
    }

    public async Task ShowLeaderboard(int playerScore)
    {
        playerScoreText.text = $"Your Score: {playerScore} soldiers";
        leaderboardText.text = "Loading...";

        List<(string playerName, int score)> topScores = await LeaderboardManager.Instance.GetTopScores();

        if (topScores.Count == 0)
        {
            leaderboardText.text = "No scores yet!";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("TOP SOLDIERS\n");

        for (int i = 0; i < topScores.Count; i++)
        {
            string playerName = string.IsNullOrEmpty(topScores[i].playerName) ? "Anonymous" : topScores[i].playerName;

            sb.AppendLine($"{i + 1}. {playerName} - {topScores[i].score}");
        }

        leaderboardText.text = sb.ToString();
    }
}