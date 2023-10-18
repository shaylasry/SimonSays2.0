using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button buttonComponent;
    [SerializeField] private Sprite buttonUp;
    [FormerlySerializedAs("buttonPressed")] [SerializeField] private Sprite buttonDown;
    public AudioSource buttonSound;
    
    public void SetButtonSprite(bool active)
    {
        buttonComponent.image.sprite = active ? buttonDown : buttonUp;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        SetButtonSprite(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetButtonSprite(false);
    }
}
