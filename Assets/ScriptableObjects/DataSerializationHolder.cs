using UnityEngine;

[CreateAssetMenu(fileName = "DataSerializationHolder", menuName = "Scriptable Objects/Data Serialization Holder")]
public class DataSerializationHolder : ScriptableObject
{
    public TextAsset DataSerializationFileAsset;
    public string fileExtensionType;
}