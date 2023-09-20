[System.Serializable]
public struct SingleLeaderboardEntry
{
    public int rank;
    public int score;
    public string playerName;
    
    public SingleLeaderboardEntry(int initRank, int initScore, string initPlayerName)
    {
        rank = initRank;
        score = initScore;
        playerName = initPlayerName;
    }
}