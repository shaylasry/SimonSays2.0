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
    private ScoreEntryData[] leaderboardData;
    
    List<int> topScores = new List<int>();

    private int entryPosition = -1;
    private int numOfEnteries = 10;

    private void Awake()
    {
        Hide();
        InitiateScoresEntries();
    }

    private void InitiateScoresEntries()
    {
        for (int i = 0; i < numOfEnteries; i++)
        {
            ScoreEntry newEntry = Instantiate(scoreEntryPrefab, scorePanel.transform);
            string rank = (i + 1).ToString();
            newEntry.SetEntry(rank);

            scoresEntries.Add(newEntry);
            topScores.Add(-1);
        }
    }
    
    
    private void GetJsonLeaderBoard(TextAsset jsonTextAsset)
    {
        leaderboardData = new ScoreEntryData[scoresEntries.Count];
        
        ScoreEntryData[] tmpLeaderboardData = JsonUtility.FromJson<ScoreEntryData[]>(jsonTextAsset.text);

        for (int i = 0; i < leaderboardData.Length; i++)
        {
            if (i >= tmpLeaderboardData.Length)
            {
                break;
            }

            leaderboardData[i] = tmpLeaderboardData[i];
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
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        RemoveHighlightScore();
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
        for (int i = 0; i < topScores.Count; i++)
        {
            if (playerScore > topScores[i])
            {
                topScores.Insert(i,playerScore);
                topScores.RemoveAt(topScores.Count - 1);
                
                entryPosition = i;
                
                scoresEntries[entryPosition].UpdateEntryData(playerScore.ToString(), PlayerPrefs.GetString(PlayerPrefsKeys.UserName));
                break;
            }
        }
        // UpdateJsonLeaderBoard();
    }

    private bool IsScoreValid(int playerScore)
    {
        return playerScore > topScores[topScores.Count - 1];
    }
    
    private void RemoveHighlightScore()
    {
        if (entryPosition != -1)
        {
            scoresEntries[entryPosition].RemoveHighlightScore();
        }

        entryPosition = -1;
    }
}
