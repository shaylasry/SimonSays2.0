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
    IConfigurationLoader LeaderboardLoader = new JsonConfigurationLoader();
    IConfigurationSaver leaderboarsSaver = new JsonConfigurationSaver();
    
    //Game objects instances
    [SerializeField] private GameObject leaderBoardPanel;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private ScoreEntry scoreEntryPrefab;
    [SerializeField] private Button closeButton;
    
    //Leaderboard management instance
    private List<ScoreEntry> scoresEntries = new List<ScoreEntry>();

    //keep the winner poistion in leader board for the score highlight
    private int winScoreEntryPosition = -1;
    private int numOfEnteries = 10;

    private void Awake()
    {
        Hide();
        LoadJsonLeaderBoard();
        InitializeCloseButton();
    }

    private void InitializeCloseButton()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        // AudioManager.Instance.PlayMenuButtonSound();
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
        for (int i = 0; i < leaderboardEntries.leaderboard.Count && i < numOfEnteries; i++)
        {
            int curScore = leaderboardEntries.leaderboard[i].score;

            if (playerScore <= curScore) continue;
            
            winScoreEntryPosition = i;
                
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
    
    private bool IsScoreValid(int playerScore)
    {
        int lowestScore = leaderboardEntries.leaderboard[^1].score;
        return playerScore > lowestScore;
    }
}
