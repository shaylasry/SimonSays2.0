using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    private IConfigurationLoader configurationLoader;

    [SerializeField] private ConfigurationHolder configurationHolder;
    
    [SerializeField] private GameObject levelMenu;
    private Dictionary<string, Dictionary<string, object>> gameConfiguration;

    public static Action PlayerDidPickLevel;

    [SerializeField] private Button levelButtonPrefab;
    private void Awake()
    {
        Hide();
        LoadConfiguration();

        InitiateLevelMenu();
    }

    private void LoadConfiguration()
    {
        InitiateConfigurationLoader(configurationHolder.fileExtensionType);
        GameConfigurationHolder.UpdateConfiguration(configurationLoader.LoadConfiguration(configurationHolder.ConfigurationFileAsset));
        gameConfiguration = GameConfigurationHolder.Configuration;
    }

    private void InitiateConfigurationLoader(string fileExtensionType)
    {
        if (string.IsNullOrEmpty(fileExtensionType))
        {
            throw new ArgumentException("File extension type cannot be empty.");
        }

        switch (fileExtensionType.ToLower())
        {
            case "json":
                configurationLoader = new JsonConfigurationLoader();
                break;
            case "xml":
                configurationLoader = new XMLConfigurationLoader();
                break;
            default:
                throw new ArgumentException("Unsupported file extension type: " + fileExtensionType);
        }
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
