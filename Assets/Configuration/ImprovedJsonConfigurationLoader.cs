using System;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedJsonConfigurationLoader<T>: IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(TextAsset xmlTextAsset)
    {
        throw new System.NotImplementedException();
    }

    public T LoadConfiguration<T>(string asset)
    {
        try
        {
            T data = JsonUtility.FromJson<T>(asset);
            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}

