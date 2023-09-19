using TMPro;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text playerName;
    
    private string rankText = "";
    private string scoreText = "";
    private string playerNameText = "";
    public void SetEntry(string initRank)
    {
        rankText = initRank;
        rank.text = rankText;
        score.text = "";
        playerName.text = "";
    }
    
    public void UpdateEntryData(string newScore, string newPlayerName)
    {
        scoreText = newScore;
        playerNameText = newPlayerName;

        rank.text = $"<color=yellow>{rank.text}</color>";
        score.text = $"<color=yellow>{scoreText}</color>";
        playerName.text = $"<color=yellow>{playerNameText}</color>";
    }
    
    public void RemoveHighlightScore()
    {
        rank.text = rankText;
        score.text = scoreText;
        playerName.text = playerNameText;
    }
}
