using System;
using UnityEngine;
using UnityEngine.UI;

public class PreGameWindow : MonoBehaviour
{
    public static Action PlayerDidPressStart;
    [SerializeField] private Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        //add sound
        Hide();
        PlayerDidPressStart?.Invoke();
    }
}