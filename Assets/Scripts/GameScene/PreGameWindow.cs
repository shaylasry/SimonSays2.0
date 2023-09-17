using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreGameWindow : MonoBehaviour
{
    public static Action PlayerDidPressStart;
    [SerializeField] private Button startButton;

    // Start is called before the first frame update
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
        Hide();
        PlayerDidPressStart?.Invoke();
    }
}