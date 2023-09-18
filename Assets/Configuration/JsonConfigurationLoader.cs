using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonConfigurationLoader : IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(string filePath)
    {
        try
        {
            string jsonText = System.IO.File.ReadAllText(filePath);
            JObject jsonData = JObject.Parse(jsonText);

            Dictionary<string, Dictionary<string, object>> configuration = new Dictionary<string, Dictionary<string, object>>();

            foreach (var kvp in jsonData)
            {
                if (kvp.Value is JObject subObj)
                {
                    Dictionary<string, object> subDict = subObj.ToObject<Dictionary<string, object>>();
                    configuration.Add(kvp.Key, subDict);
                }
            }

            return configuration;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading configuration: {e.Message}");
            return null;
        }
    }
}