using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGameWindow : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToMenuButton;
    public TMP_Text message;
    public void Awake()
    {
        Hide();
        retryButton.onClick.AddListener(OnRetryButtonClick);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
    }

    private void OnBackToMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnRetryButtonClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
