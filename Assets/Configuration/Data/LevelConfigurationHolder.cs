using System;
using System.Collections.Generic;
//Singleton GameConfigurationHolder so we can use the selected level configuration from all scenes 
public class LevelConfigurationHolder
{
    // Singleton instance
    private static SingleGameConfiguration configuration = new SingleGameConfiguration();

    public static SingleGameConfiguration Configuration
    {
        get
        {
            return configuration;
        }
    }

    public static void UpdateConfiguration(SingleGameConfiguration config)
    {
        configuration = config;
    }
}