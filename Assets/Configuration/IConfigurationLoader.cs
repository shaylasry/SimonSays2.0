using System.Collections.Generic;
using UnityEngine;

public interface IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(TextAsset xmlTextAsset);
    
    public T LoadConfiguration<T>(string asset);
}