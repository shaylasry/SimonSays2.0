using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class LeaderBoard : MonoBehaviour
{
    private const string JsonLeaderBoardFileName = "JsonLeaderboardHolder.json";
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private ScoreEntry scoreEntryPrefab;
    private List<ScoreEntry> scoresEntries = new List<ScoreEntry>();
    
    private LeaderboardEntries leaderboardEntries;
    IConfigurationLoader LeaderboardLoader = new ImprovedJsonConfigurationLoader<LeaderboardEntries>();
    IConfigurationSaver leaderboarsSaver = new JsonConfigurationSaver<LeaderboardEntries>();
    
    List<int> topScores = new List<int>();

    private int winScoreEntryPosition = -1;
    private int numOfEnteries = 10;

    private void Awake()
    {
        Hide();
        LoadJsonLeaderBoard();
        InitializeTopScoresList();
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
    
    private void OnPlayerDidWin(int playerScore)
    {
        if (IsScoreValid(playerScore))
        {
            SubmitScore(playerScore);
        }
        
        Show();
    }

    public void Show()
    {
        InitiateLeaderboard();
        UpdateJsonLeaderBoard();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        ResetLeaderBoardForNextGame();
    }

    private void LoadJsonLeaderBoard()
    {
        string filePath = Path.Combine(Application.persistentDataPath, JsonLeaderBoardFileName);

        if (!File.Exists(filePath))
        {
            LeaderboardEntries leaderboardData = new LeaderboardEntries
            {
                leaderboard = new List<SingleLeaderboardEntry>()
            };

            string jsonDataToWrite = JsonUtility.ToJson(leaderboardData);
            
            File.WriteAllText(filePath, jsonDataToWrite);
        }

        string jsonDataToRead = File.ReadAllText(filePath);
        leaderboardEntries = LeaderboardLoader.LoadConfiguration<LeaderboardEntries>(jsonDataToRead);
    }


    private void InitializeTopScoresList()
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
        leaderboarsSaver.SaveConfiguration(JsonLeaderBoardFileName, leaderboardEntries);
    }
    
    private void ResetLeaderBoardForNextGame()
    {
        winScoreEntryPosition = -1;
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
                SingleLeaderboardEntry newEntry = 
                    new SingleLeaderboardEntry(playerScore, PlayerPrefs.GetString(PlayerPrefsKeys.UserName));
                                
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
