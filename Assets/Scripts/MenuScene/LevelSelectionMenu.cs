using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    public static Action PlayerDidPickLevel;
    
    private IConfigurationLoader configurationLoader;

    [SerializeField] private DataSerializationHolder gameConfigurationFileHolder;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private Button levelButtonPrefab;


    private void Awake()
    {
        Hide();
        LoadConfiguration();

        InitiateLevelMenu();
    }

    private void LoadConfiguration()
    {
        InitiateConfigurationLoader(gameConfigurationFileHolder.fileExtensionType);
        var textAsset = gameConfigurationFileHolder.DataSerializationFileAsset;
        
        GameConfigurationHolder.Configuration =  configurationLoader.LoadConfiguration<GameConfigurations>(textAsset.ToString());
    }

    //use fileExtensionType to chose configuration according to the TextAsset extension type
    private void InitiateConfigurationLoader(string fileExtensionType)
    {
        if (string.IsNullOrEmpty(fileExtensionType))
        {
            throw new ArgumentException("File extension type cannot be empty.");
        }

        //lower case to avoid typing errors
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
        foreach (SingleGameConfiguration targetConfig in GameConfigurationHolder.Configuration.configurations)
        {
            Button newButton = Instantiate(levelButtonPrefab, levelMenu.transform);
            TMP_Text levelButtonText = newButton.GetComponentInChildren<TMP_Text>();

            if (levelButtonText != null)
            {
                levelButtonText.text = targetConfig.title;
            }
            
            newButton.onClick.AddListener(() => OnLevelButtonClick(targetConfig.id));
        }
    }

    public void OnLevelButtonClick(string clickedButtonId)
    {
//add sound
        List<SingleGameConfiguration> configuration = GameConfigurationHolder.Configuration.configurations;
        var selectedConfig = configuration.First(config => config.id == clickedButtonId);

        if (selectedConfig.id == "") return;
        
        LevelConfigurationHolder.Configuration = selectedConfig;
        PlayerDidPickLevel?.Invoke();
    }
}
