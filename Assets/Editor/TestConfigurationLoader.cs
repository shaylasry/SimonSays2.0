using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestConfigurationLoader
{
    private GameConfigurations gameConfigurationFromJson;
    private GameConfigurations gameConfigurationFromXml;

    private SingleGameConfiguration badSingleGameConfiguration = new SingleGameConfiguration()
    {
        id = "easy",
        title = "Easy",
        numOfGameButtons = 4,
        pointsPerStep = 1,
        gameTime = 5,
        repeatMode = true,
        gameSpeed = 1.0f,
    };
    
    private GameConfigurations expectedConfigurations = new GameConfigurations()
    {
        configurations = new List<SingleGameConfiguration>()
        {
            new SingleGameConfiguration()
            {
                id = "easy",
                title = "Easy",
                numOfGameButtons = 4,
                pointsPerStep = 1,
                gameTime = 50,
                repeatMode = true,
                gameSpeed = 1.0f,
            },
            new SingleGameConfiguration()
            {
                id = "medium",
                title = "Medium",
                numOfGameButtons = 5,
                pointsPerStep = 2,
                gameTime = 45,
                repeatMode = true,
                gameSpeed = 1.25f,
            },
            new SingleGameConfiguration()
            {
                id = "hard",
                title = "Hard",
                numOfGameButtons = 6,
                pointsPerStep = 3,
                gameTime = 30,
                repeatMode = false,
                gameSpeed = 1.5f,
            }
        }
    };

    [UnityTest]
    public IEnumerator Should_SuccessfulLoadJsonConfiguration_When_GameConfigurationsAreValid()
    {
        LoadJsonConfiguration();
        
        Assert.NotNull(gameConfigurationFromJson);
        Assert.AreEqual(expectedConfigurations.configurations.Count, gameConfigurationFromJson.configurations.Count);
       
        Assert.AreEqual(expectedConfigurations.configurations, gameConfigurationFromJson.configurations);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Should_LoadWrongJsonConfiguration_When_GameConfigurationAreUnmatched()
    {
        LoadJsonConfiguration();
        
        Assert.NotNull(gameConfigurationFromJson);
       
        Assert.AreNotEqual(badSingleGameConfiguration, gameConfigurationFromJson.configurations[0]);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Should_SuccessfulUpdateGameConfigurationHolder_When_GameConfigurationsAreValid()
    {
        LoadJsonConfiguration();
        GameConfigurationHolder.Configuration = gameConfigurationFromJson;
        Assert.NotNull(gameConfigurationFromJson);
        Assert.AreEqual(expectedConfigurations.configurations, GameConfigurationHolder.Configuration);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator Should_SuccessfulLoadXMLConfiguration_When_GameConfigurationsAreValid()
    {
        LoadXmlConfiguration();
        Assert.NotNull(gameConfigurationFromXml);
        Assert.AreEqual(expectedConfigurations.configurations.Count, gameConfigurationFromXml.configurations.Count);
       
        Assert.AreEqual(expectedConfigurations.configurations, gameConfigurationFromXml.configurations);
        
        yield return null;
    }
    [UnityTest]
    public IEnumerator Should_LoadWrongXMLConfiguration_When_GameConfigurationAreUnmatched()
    {
        LoadXmlConfiguration();
        
        Assert.NotNull(gameConfigurationFromJson);
       
        Assert.AreNotEqual(badSingleGameConfiguration, gameConfigurationFromXml.configurations[0]);
        yield return null;
    }
    
    public void LoadJsonConfiguration()
    {
        TextAsset jsonGameConfiguration = Resources.Load<TextAsset>("Json/JsonGameConfiguration");
        IConfigurationLoader configurationLoader = new JsonConfigurationLoader();
        gameConfigurationFromJson = configurationLoader.LoadConfiguration<GameConfigurations>(jsonGameConfiguration.ToString());
    }

    private void LoadXmlConfiguration()
    {
        TextAsset jsonGameConfiguration = Resources.Load<TextAsset>("XML/XMLGameConfiguration");
        IConfigurationLoader configurationLoader = new XMLConfigurationLoader();
        gameConfigurationFromJson = configurationLoader.LoadConfiguration<GameConfigurations>(jsonGameConfiguration.ToString());
    }
}