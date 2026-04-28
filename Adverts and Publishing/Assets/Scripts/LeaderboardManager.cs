using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{

    public static LeaderboardManager Instance;

    private const string LEADERBOARD_ID = "Most_Soldiers";

    private void Awake()
    {
        Instance = this;
    }

    public async Task SubmitScore(int score)
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Not signed in, cannot submit score");
                return;
            }

            await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
            Debug.Log($"Score submitted: {score}");
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to submit score: {e.Message}");
        }
    }

    public async Task<int> GetPlayerHighScore()
    {
        try
        {
            var score = await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
            return (int)score.Score;
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to get score: {e.Message}");
            return 0;
        }
    }

    public async Task<List<(string playerName, int score)>> GetTopScores(int count = 10)
    {
        List<(string, int)> results = new List<(string, int)>();

        try
        {
            var scores = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID, new GetScoresOptions { Limit = count });

            foreach (var entry in scores.Results)
            {
                results.Add((entry.PlayerName, (int)entry.Score));
            }
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to get top scores: {e.Message}");
        }

        return results;
    }
}
