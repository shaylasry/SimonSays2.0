using UnityEngine;

[CreateAssetMenu(fileName = "ConfigurationHolder", menuName = "Scriptable Objects/Configuration Holder")]
public class ConfigurationHolder : ScriptableObject
{
    public TextAsset ConfigurationFileAsset;
    public string fileExtensionType;
}