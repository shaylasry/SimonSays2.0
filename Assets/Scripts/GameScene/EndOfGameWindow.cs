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
    [SerializeField] private SceneChanger sceneChanger;
    public TMP_Text message;
    public void Awake()
    {
        Hide();
        retryButton.onClick.AddListener(OnRetryButtonClick);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
    }

    private void OnBackToMenuButtonClick()
    {
        sceneChanger.FadeOutTrigger("Menu");
    }

    private void OnRetryButtonClick()
    {
        sceneChanger.FadeOutTrigger("Game");
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
