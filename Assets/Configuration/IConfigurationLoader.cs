using System.Collections.Generic;

interface IConfigurationLoader
{
    public Dictionary<string, object> LoadConfiguration(string filePath);
}