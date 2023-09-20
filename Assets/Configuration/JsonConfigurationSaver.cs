using System.IO;
using UnityEngine;

public class JsonConfigurationSaver<T> : IConfigurationSaver
{
    public void SaveConfiguration<T>(string fileName,  T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, jsonData);
    }
}