using System.Collections.Generic;
using UnityEngine;

interface IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(TextAsset xmlTextAsset);
}