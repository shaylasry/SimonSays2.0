using UnityEngine;

public interface IConfigurationSaver
{
    //Using Generic type so we can you this interface anytime we want to save data to TextAsset
    void SaveConfiguration<T>(string fileName, T data);
}