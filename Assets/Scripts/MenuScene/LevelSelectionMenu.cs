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
    private JsonConfigurationLoader jsonConfigurationLoader = new JsonConfigurationLoader();
    [SerializeField] private string gameConfigFilePath;
    [SerializeField] private GameObject levelMenu;
    private Dictionary<string, Dictionary<string, object>> gameConfiguration;

    public static Action PlayerDidPickLevel;

    [SerializeField] private Button levelButtonPrefab;
    private void Awake()
    {
        Hide();
        GameConfigurationHolder.UpdateConfiguration(jsonConfigurationLoader.LoadConfiguration(gameConfigFilePath));
        gameConfiguration = GameConfigurationHolder.Configuration;
        InitiateLevelMenu();
    }

    // Start is called before the first frame update
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void InitiateLevelMenu()
    {
        List<string> keysList = new List<string>(gameConfiguration.Keys);
        int numOfButtons = keysList.Count;
        
        for (int i = 0; i < numOfButtons; i++)
        {
            Button newButton = Instantiate(levelButtonPrefab, levelMenu.transform);
            TMP_Text levelButtonText = newButton.GetComponentInChildren<TMP_Text>();

            if (levelButtonText != null)
            {
                levelButtonText.text = keysList[i];
            }
            
            newButton.onClick.AddListener(() => OnLevelButtonClick(levelButtonText));
        }
    }

    public void OnLevelButtonClick(TMP_Text levelButtonText)
    {
        LevelConfigurationHolder.UpdateConfiguration(gameConfiguration[levelButtonText.text]);
        PlayerDidPickLevel?.Invoke();
    }
}
