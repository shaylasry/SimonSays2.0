[System.Serializable]
public struct SingleLeaderboardEntry
{
    public int score;
    public string playerName;
    
    public SingleLeaderboardEntry(int initScore, string initPlayerName)
    {
        score = initScore;
        playerName = initPlayerName;
    }
}