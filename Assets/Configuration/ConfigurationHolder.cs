using System.Collections.Generic;

public class ConfigurationHolder
{
    // Singleton instance
    private static Dictionary<string, object> configuration;
    
    public static Dictionary<string, object> Configuration
    {
        get
        {
            if (configuration == null)
            {
                configuration = new Dictionary<string, object>();
            }
            return configuration;
        }
    }

    public void UpdateConfiguration(Dictionary<string, object> newConfiguration)
    {
        configuration = newConfiguration;
    }
    
    private ConfigurationHolder() {}
}