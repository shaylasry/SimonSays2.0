using UnityEngine;

//We use scriptable object to keep both TextAsset and type of the TextAsset file so we can distinguish the type more easily
[CreateAssetMenu(fileName = "DataSerializationHolder", menuName = "Scriptable Objects/Data Serialization Holder")]
public class DataSerializationHolder : ScriptableObject
{
    public TextAsset DataSerializationFileAsset;
    public string fileExtensionType;
}