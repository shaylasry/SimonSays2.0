using System.Collections.Generic;

interface IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(string filePath);
}