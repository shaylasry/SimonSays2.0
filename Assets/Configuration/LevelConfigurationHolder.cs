using System;
using System.Collections.Generic;

public class LevelConfigurationHolder
{
    // Singleton instance
    private static Dictionary<string,object>configuration;

    public static Dictionary<string,object> Configuration
    {
        get
        {
            if (configuration == null)
            {
                configuration = new Dictionary<string,object>();
            }
            return configuration;
        }
    }

    public static void UpdateConfiguration(Dictionary<string, object> newConfiguration)
    {
        Dictionary<string, object> updatedConfiguration = new Dictionary<string, object>();
        // To avoid cases where the seriliaztion function saved int or float as 64 data types
        // we will change it back to int and float. If we can't - throw exception
        foreach (var kvp in newConfiguration)
        {
            if (kvp.Value is long)
            {
                int intValue;
                if (int.TryParse(kvp.Value.ToString(), out intValue))
                {
                    updatedConfiguration[kvp.Key] = intValue;
                }
                else
                {
                    throw new FormatException($"Value for '{kvp.Key}' is not convertible to int: {kvp.Value}");
                }
            }
            else if (kvp.Value is double)
            {
                float floatValue;
                if (float.TryParse(kvp.Value.ToString(), out floatValue))
                {
                    updatedConfiguration[kvp.Key] = floatValue;
                }
                else
                {
                    throw new FormatException($"Value for '{kvp.Key}' is not convertible to float: {kvp.Value}");
                }
            }
            else
            {
                updatedConfiguration[kvp.Key] = kvp.Value;
            }
        }

        configuration = updatedConfiguration; 
    }



    
    private LevelConfigurationHolder() {}
}