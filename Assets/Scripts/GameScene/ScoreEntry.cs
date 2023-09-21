using TMPro;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text playerName;
    
    public void InitEntry(string initRank, string intitScore, string initPlayerName)
    {
        rank.text = initRank;
        score.text = intitScore;
        playerName.text = initPlayerName;
    }

    public void InitEntryWithHighLight(string initRank, string intitScore, string initPlayerName)
    {
        rank.text = $"<color=yellow>{initRank}</color>";
        score.text = $"<color=yellow>{intitScore}</color>";
        playerName.text = $"<color=yellow>{initPlayerName}</color>";
    }
}
