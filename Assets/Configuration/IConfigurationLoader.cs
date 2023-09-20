using System.Collections.Generic;
using UnityEngine;

public interface IConfigurationLoader
{
    public T LoadConfiguration<T>(string asset);
}