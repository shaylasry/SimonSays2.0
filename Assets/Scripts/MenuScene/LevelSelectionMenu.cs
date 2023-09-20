using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    private IConfigurationLoader configurationLoader;

    [SerializeField] private DataSerializationHolder gameConfigurationFileHolder;
    
    [SerializeField] private GameObject levelMenu;
    private GameConfigurations gameConfiguration;

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
        InitiateConfigurationLoader(gameConfigurationFileHolder.fileExtensionType);
        var textAsset = gameConfigurationFileHolder.DataSerializationFileAsset;
        GameConfigurations loadedConfiguration =
            configurationLoader.LoadConfiguration<GameConfigurations>(textAsset.ToString());

        GameConfigurationHolder.UpdateConfiguration(loadedConfiguration);
        gameConfiguration = GameConfigurationHolder.Configuration;
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
                configurationLoader = new JsonConfigurationLoader<GameConfigurations>();
                break;
            case "xml":
                configurationLoader = new XMLConfigurationLoader<GameConfigurations>();
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
        foreach (SingleGameConfiguration targetConfig in gameConfiguration.configurations)
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
        var configuration = gameConfiguration.configurations;
        var selectedConfig = configuration.First(config => config.id == clickedButtonId);

        if (selectedConfig.id == "") return;
        
        LevelConfigurationHolder.UpdateConfiguration(selectedConfig);
        PlayerDidPickLevel?.Invoke();
    }
}
