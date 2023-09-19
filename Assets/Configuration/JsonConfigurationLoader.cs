using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonConfigurationLoader : IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(TextAsset jsonTextAsset)
    {
        try
        {
            if (jsonTextAsset != null)
            {
                string jsonText = jsonTextAsset.text;
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
            else
            {
                Debug.LogError("JSON TextAsset is null.");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading configuration: {e.Message}");
            return null;
        }
    }
}