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

    public int numOfGameButtons;
    public int pointsPerStep;
    public int gameTime;
    public bool repeatMode;
    public float gameSpeed;

    //default as easy
    private LevelData()
    {
        numOfGameButtons = 4;
        pointsPerStep = 1;
        gameTime = 50;
        repeatMode = true;
        gameSpeed = 1.0f;
    }

    public void UpdateLevelData(int newNumOfGameButtons, int newPointsPerStep, int newGameTime, bool newRepeatMode, float newGameSpeed)
    {
        numOfGameButtons = newNumOfGameButtons;
        pointsPerStep = newPointsPerStep;
        gameTime = newGameTime;
        repeatMode = newRepeatMode;
        gameSpeed = newGameSpeed;
    }
}