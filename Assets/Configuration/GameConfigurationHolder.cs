using System.Collections.Generic;

public class GameConfigurationHolder
{
    // Singleton instance
    private static Dictionary<string, Dictionary<string,object>> configuration;
    
    public static Dictionary<string, Dictionary<string,object>> Configuration
    {
        get
        {
            if (configuration == null)
            {
                configuration = new Dictionary<string, Dictionary<string,object>>();
            }
            return configuration;
        }
    }

    public static void UpdateConfiguration(Dictionary<string, Dictionary<string,object>> newConfiguration)
    {
        configuration = newConfiguration;
    }
    
    private GameConfigurationHolder() {}
}