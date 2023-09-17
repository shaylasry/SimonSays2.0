using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LevelSelectionMenu : MonoBehaviour
{
    public static Action<string> PlayerDidPickLevel;
    [SerializeField] private Button [] levelButtons;

    private void Awake()
    {
        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button button in levelButtons)
        {
            string buttonText = button.GetComponentInChildren<TMP_Text>().text;
            button.onClick.AddListener(() => OnButtonClick(buttonText));
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnButtonClick(string difficulty)
    {
        PlayerDidPickLevel?.Invoke(difficulty);
    }
}
