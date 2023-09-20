using UnityEngine;

public interface IConfigurationSaver
{
    void SaveConfiguration<T>(string fileName, T data);
}