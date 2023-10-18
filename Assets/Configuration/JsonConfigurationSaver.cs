using System;
using System.IO;
using UnityEngine;

public class JsonConfigurationSaver : IConfigurationSaver
{
    public void SaveConfiguration<T>(string fileName,  T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            File.WriteAllText(path, jsonData);
        }
        catch (Exception e)
        {
            throw new Exception("Error save to Json file: " + e.Message);
        }
    }
}