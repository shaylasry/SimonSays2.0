using System.Collections.Generic;

public class GameConfigurationHolder
{
    private static GameConfigurations configuration = new GameConfigurations();
    
    public static GameConfigurations Configuration
    {
        get
        {
            return configuration;
        }
    }

    public static void UpdateConfiguration(GameConfigurations configurations)
    {
        configuration = configurations;
    }
}