using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public Button buttonComponent;
    [SerializeField] private Sprite buttonUp;
    [SerializeField] private Sprite buttonPressed;
    public AudioSource buttonSound;

    public void ButtonIsPressed()
    {
        buttonComponent.image.sprite = buttonPressed;
    }
    
    public void ButtonIsReleased()
    {
        buttonComponent.image.sprite = buttonUp;
    }
    
    
}
