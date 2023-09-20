using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Leaderboard will always show the top 10 results
public class LeaderBoard : MonoBehaviour
{
    public static Action PlayerClosedLeaderBoard;
    private const string JsonLeaderBoardFileName = "JsonLeaderboardHolder.json";
    
    //Write and read to persistence leaderboard instances
    private LeaderboardEntries leaderboardEntries;
    IConfigurationLoader LeaderboardLoader = new JsonConfigurationLoader<LeaderboardEntries>();
    IConfigurationSaver leaderboarsSaver = new JsonConfigurationSaver<LeaderboardEntries>();
    
    //Game objects instances
    [SerializeField] private GameObject leaderBoardPanel;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private ScoreEntry scoreEntryPrefab;
    [SerializeField] private Button closeButton;
    
    //Leaderboard management instance
    private List<ScoreEntry> scoresEntries = new List<ScoreEntry>();
    //topScores is used to keep track during the game so we won't update leaderboard if not needed.
    //When not all leaderboard entries are full we keep -1 value so a player with score 0 will still enter the leaderboard
    List<int> topScores = new List<int>(); 
    private int winScoreEntryPosition = -1;
    private int numOfEnteries = 10;

    private void Awake()
    {
        Hide();
        LoadJsonLeaderBoard();
        InitializeCloseButton();
        InitializeTopScoresList();
    }

    private void InitializeCloseButton()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        Hide();
        PlayerClosedLeaderBoard?.Invoke();
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
        GameManager.PlayerDidWin -= OnPlayerDidWin;
    }
    
    private void OnPlayerDidWin(int playerScore)
    {
        if (IsScoreValid(playerScore))
        {
            SubmitScore(playerScore);
            UpdateJsonLeaderBoard();
        }
        Show();
    }
    
    public void Show()
    {
        InitiateLeaderboard();
        leaderBoardPanel.SetActive(true);
    }
    
    public void Hide()
    {
        /*We hide leaderBoardPanel instead of this game object to insure OnEnable will
        be called and GameManager.PlayerDidWin subscription*/
        leaderBoardPanel.SetActive(false);
        ResetLeaderBoardForNextGame();
    }
    
    private void LoadJsonLeaderBoard()
    {
        string filePath = Path.Combine(Application.persistentDataPath, JsonLeaderBoardFileName);
        
        //If we want to reset leader board we can use the code in this if
        //didn't made this functionality because we want persistence leaderboard
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
    
    //Instantiate entries only if needed - Win state only
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
