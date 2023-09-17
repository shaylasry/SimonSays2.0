public class LevelData
{
    // Singleton instance
    private static LevelData instance;

    public static LevelData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelData();
            }
            return instance;
        }
    }

    public string difficulty;
    public int gameButtons;
    public int pointsPerStep;
    public int gameTime;
    public bool repeatMode;
    public float gameSpeed;

    //default as easy
    private LevelData()
    {
        difficulty = "Easy";
        gameButtons = 4;
        pointsPerStep = 1;
        gameTime = 50;
        repeatMode = true;
        gameSpeed = 1.0f;
    }

    public void UpdateLevelData(string newDifficulty, int newGameButtons, int newPointsPerStep, int newGameTime, bool newRepeatMode, float newGameSpeed)
    {
        difficulty = newDifficulty;
        gameButtons = newGameButtons;
        pointsPerStep = newPointsPerStep;
        gameTime = newGameTime;
        repeatMode = newRepeatMode;
        gameSpeed = newGameSpeed;
    }
}