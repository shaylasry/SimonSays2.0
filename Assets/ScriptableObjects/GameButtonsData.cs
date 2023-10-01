using UnityEngine;

//We use scriptable object to keep both TextAsset and type of the TextAsset file so we can distinguish the type more easily
[CreateAssetMenu(fileName = "GameButtonsData", menuName = "Scriptable Objects/Game Buttons Data")]
public class GameButtonsData : ScriptableObject
{ 
    public Color[] gameButtonsColors;
    public AudioClip[] gameButtonsSounds;
    public AudioClip wrongGameButtonSound;
}
