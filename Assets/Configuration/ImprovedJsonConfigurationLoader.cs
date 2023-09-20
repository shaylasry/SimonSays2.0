using System;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedJsonConfigurationLoader<T>: IConfigurationLoader
{
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

