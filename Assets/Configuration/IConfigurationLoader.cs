using System.Collections.Generic;
using UnityEngine;

public interface IConfigurationLoader
{
    //Using Generic type so we can you this interface anytime we want to load from a TextAsset.text
    public T LoadConfiguration<T>(string asset);
}