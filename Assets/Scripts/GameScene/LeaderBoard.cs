using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private ScoreEntry scoreEntryPrefab;
    private List<ScoreEntry> scoresEntries = new List<ScoreEntry>();
    
    [SerializeField] private DataSerializationHolder leaderboardFileHolder;
    private LeaderboardEntries leaderboardEntries;
    IConfigurationLoader LeaderboardLoader = new ImprovedJsonConfigurationLoader<LeaderboardEntries>();
    
    List<int> topScores = new List<int>();

    private int winScoreEntryPosition = -1;
    private int numOfEnteries = 10;

    private void InitiateLeaderBoardLoader(string fileExtensionType)
    {
        if (string.IsNullOrEmpty(fileExtensionType))
        {
            throw new ArgumentException("File extension type cannot be empty.");
        }

        switch (fileExtensionType.ToLower())
        {
            case "json":
                LeaderboardLoader = new ImprovedJsonConfigurationLoader<LeaderboardEntries>();
                break;
            default:
                throw new ArgumentException("Unsupported file extension type: " + fileExtensionType);
        }
    }
    
    private void Awake()
    {
        Hide();
        LoadJsonLeaderBoard();
        InititalTopScoresList();
    }

    private void InititalTopScoresList()
    {
        for (int i = 0; i < numOfEnteries; i++)
        {
            if (i < leaderboardEntries.leaderboard.Count)
            {
                topScores.Add(leaderboardEntries.leaderboard[i].score);
            }
            else
            {
                topScores.Add(-1);
            }
        }
    }

    private void LoadJsonLeaderBoard()
    {
        InitiateLeaderBoardLoader(leaderboardFileHolder.fileExtensionType);
        var textAsset = leaderboardFileHolder.DataSerializationFileAsset;
        leaderboardEntries = LeaderboardLoader.LoadConfiguration<LeaderboardEntries>(textAsset.ToString());
    }

    private void InitiateLeaderboard()
    {
        for (int i = 0; i < numOfEnteries; i++)
        {
            string rank = (i + 1).ToString();;
            string score = "";
            string playerName = "";
            ScoreEntry newEntry = Instantiate(scoreEntryPrefab, scorePanel.transform);
            
            if (i < leaderboardEntries.leaderboard.Count)
            {
                SingleLeaderboardEntry curEntry = leaderboardEntries.leaderboard[i];
                score = curEntry.score.ToString();
                playerName = curEntry.playerName;
                
                topScores.Add(curEntry.score);
            }
            else
            {
                topScores.Add(-1);
            }

            if (winScoreEntryPosition == i)
            {
                newEntry.InitEntryWithHighLight(rank, score, playerName);
            }
            else
            {
                newEntry.InitEntry(rank, score, playerName);
            }
            
            scoresEntries.Add(newEntry);
        }
    }
    
    
    private void UpdateJsonLeaderBoard()
    {
        
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();   
    }
    
    private void Subscribe()
    {
        GameManager.PlayerDidWin += OnPlayerDidWin;
    }

    private void Unsubscribe()
    {
        GameManager.PlayerDidWin += OnPlayerDidWin;
    }

    public void Show()
    {
        InitiateLeaderboard();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        ResetLeaderBoardForNextGame();
    }

    private void ResetLeaderBoardForNextGame()
    {
        winScoreEntryPosition = -1;
    }

    private void OnPlayerDidWin(int playerScore)
    {
        if (IsScoreValid(playerScore))
        {
            SubmitScore(playerScore);
        }
        
        Show();
    }
    
    private void SubmitScore(int playerScore)
    {
        for (int i = 0; i < numOfEnteries; i++)
        {
            if (playerScore > topScores[i])
            {
                winScoreEntryPosition = i;
                
                topScores.Insert(i,playerScore);
                topScores.RemoveAt(topScores.Count - 1);
                SingleLeaderboardEntry newEntry = new SingleLeaderboardEntry(i + 1, playerScore, PlayerPrefsKeys.UserName);
                                
                leaderboardEntries.leaderboard.Insert(i, newEntry);
                if (leaderboardEntries.leaderboard.Count > numOfEnteries)
                {
                    leaderboardEntries.leaderboard.RemoveAt(leaderboardEntries.leaderboard.Count - 1);
                }
                break;
            }
        }
    }
    
    private bool IsScoreValid(int playerScore)
    {
        return playerScore > topScores[topScores.Count - 1];
    }
}
