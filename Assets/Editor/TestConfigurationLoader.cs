using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestConfigurationLoader
{
    private GameConfigurations gameConfigurationFromJson;
    private GameConfigurations gameConfigurationFromXml;

    [UnityTest]
    public IEnumerator LoadGameConfigurationFromJson()
    {
        LoadJsonConfiguration();
        Assert.NotNull(gameConfigurationFromJson);
        //check all the id's are in
        Assert.AreEqual(3, gameConfigurationFromJson.configurations.Count);
        
        Assert.AreEqual("easy", gameConfigurationFromJson.configurations[0].id);
        Assert.AreEqual("Easy", gameConfigurationFromJson.configurations[0].title);
        Assert.AreEqual(4, gameConfigurationFromJson.configurations[0].numOfGameButtons);
        Assert.AreEqual(1, gameConfigurationFromJson.configurations[0].pointsPerStep);
        Assert.AreEqual(50, gameConfigurationFromJson.configurations[0].gameTime);
        Assert.AreEqual(true, gameConfigurationFromJson.configurations[0].repeatMode);
        Assert.AreEqual(1.0f, gameConfigurationFromJson.configurations[0].gameSpeed);

        Assert.AreEqual("medium", gameConfigurationFromJson.configurations[1].id);
        Assert.AreEqual("Medium", gameConfigurationFromJson.configurations[1].title);
        Assert.AreEqual(5, gameConfigurationFromJson.configurations[1].numOfGameButtons);
        Assert.AreEqual(2, gameConfigurationFromJson.configurations[1].pointsPerStep);
        Assert.AreEqual(45, gameConfigurationFromJson.configurations[1].gameTime);
        Assert.AreEqual(true, gameConfigurationFromJson.configurations[1].repeatMode);
        Assert.AreEqual(1.25f, gameConfigurationFromJson.configurations[1].gameSpeed);

        Assert.AreEqual("hard", gameConfigurationFromJson.configurations[2].id);
        Assert.AreEqual("Hard", gameConfigurationFromJson.configurations[2].title);
        Assert.AreEqual(6, gameConfigurationFromJson.configurations[2].numOfGameButtons);
        Assert.AreEqual(3, gameConfigurationFromJson.configurations[2].pointsPerStep);
        Assert.AreEqual(30, gameConfigurationFromJson.configurations[2].gameTime);
        Assert.AreEqual(false, gameConfigurationFromJson.configurations[2].repeatMode);
        Assert.AreEqual(1.5f, gameConfigurationFromJson.configurations[2].gameSpeed);
        

        yield return null;
    }
    
    [UnityTest]
    public IEnumerator UpdateGameConfigurationHolderSingleton()
    {
        LoadJsonConfiguration();
        GameConfigurationHolder.UpdateConfiguration(gameConfigurationFromJson);
        Assert.NotNull(gameConfigurationFromJson);
        
        Assert.AreEqual("easy", GameConfigurationHolder.Configuration.configurations[0].id);
        Assert.AreEqual("Easy", GameConfigurationHolder.Configuration.configurations[0].title);
        Assert.AreEqual(4, GameConfigurationHolder.Configuration.configurations[0].numOfGameButtons);
        Assert.AreEqual(1, GameConfigurationHolder.Configuration.configurations[0].pointsPerStep);
        Assert.AreEqual(50, GameConfigurationHolder.Configuration.configurations[0].gameTime);
        Assert.AreEqual(true, GameConfigurationHolder.Configuration.configurations[0].repeatMode);
        Assert.AreEqual(1.0f, GameConfigurationHolder.Configuration.configurations[0].gameSpeed);

        Assert.AreEqual("medium", GameConfigurationHolder.Configuration.configurations[1].id);
        Assert.AreEqual("Medium", GameConfigurationHolder.Configuration.configurations[1].title);
        Assert.AreEqual(5, GameConfigurationHolder.Configuration.configurations[1].numOfGameButtons);
        Assert.AreEqual(2, GameConfigurationHolder.Configuration.configurations[1].pointsPerStep);
        Assert.AreEqual(45, GameConfigurationHolder.Configuration.configurations[1].gameTime);
        Assert.AreEqual(true, GameConfigurationHolder.Configuration.configurations[1].repeatMode);
        Assert.AreEqual(1.25f, GameConfigurationHolder.Configuration.configurations[1].gameSpeed);

        Assert.AreEqual("hard", GameConfigurationHolder.Configuration.configurations[2].id);
        Assert.AreEqual("Hard", GameConfigurationHolder.Configuration.configurations[2].title);
        Assert.AreEqual(6, GameConfigurationHolder.Configuration.configurations[2].numOfGameButtons);
        Assert.AreEqual(3, GameConfigurationHolder.Configuration.configurations[2].pointsPerStep);
        Assert.AreEqual(30, GameConfigurationHolder.Configuration.configurations[2].gameTime);
        Assert.AreEqual(false, GameConfigurationHolder.Configuration.configurations[2].repeatMode);
        Assert.AreEqual(1.5f, GameConfigurationHolder.Configuration.configurations[2].gameSpeed);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator XMLLoadConfigurationTest()
    {
        LoadXmlConfiguration();
        Assert.NotNull(gameConfigurationFromXml);
        //check all the id's are in
        Assert.AreEqual(3, gameConfigurationFromJson.configurations.Count);
        
        Assert.AreEqual("easy", gameConfigurationFromJson.configurations[0].id);
        Assert.AreEqual("Easy", gameConfigurationFromJson.configurations[0].title);
        Assert.AreEqual(4, gameConfigurationFromJson.configurations[0].numOfGameButtons);
        Assert.AreEqual(1, gameConfigurationFromJson.configurations[0].pointsPerStep);
        Assert.AreEqual(50, gameConfigurationFromJson.configurations[0].gameTime);
        Assert.AreEqual(true, gameConfigurationFromJson.configurations[0].repeatMode);
        Assert.AreEqual(1.0f, gameConfigurationFromJson.configurations[0].gameSpeed);

        Assert.AreEqual("medium", gameConfigurationFromJson.configurations[1].id);
        Assert.AreEqual("Medium", gameConfigurationFromJson.configurations[1].title);
        Assert.AreEqual(5, gameConfigurationFromJson.configurations[1].numOfGameButtons);
        Assert.AreEqual(2, gameConfigurationFromJson.configurations[1].pointsPerStep);
        Assert.AreEqual(45, gameConfigurationFromJson.configurations[1].gameTime);
        Assert.AreEqual(true, gameConfigurationFromJson.configurations[1].repeatMode);
        Assert.AreEqual(1.25f, gameConfigurationFromJson.configurations[1].gameSpeed);

        Assert.AreEqual("hard", gameConfigurationFromJson.configurations[2].id);
        Assert.AreEqual("Hard", gameConfigurationFromJson.configurations[2].title);
        Assert.AreEqual(6, gameConfigurationFromJson.configurations[2].numOfGameButtons);
        Assert.AreEqual(3, gameConfigurationFromJson.configurations[2].pointsPerStep);
        Assert.AreEqual(30, gameConfigurationFromJson.configurations[2].gameTime);
        Assert.AreEqual(false, gameConfigurationFromJson.configurations[2].repeatMode);
        Assert.AreEqual(1.5f, gameConfigurationFromJson.configurations[2].gameSpeed);
        
        yield return null;
    }
    
    public void LoadJsonConfiguration()
    {
        TextAsset jsonGameConfiguration = Resources.Load<TextAsset>("Json/JsonGameConfiguration");
        IConfigurationLoader configurationLoader = new ImprovedJsonConfigurationLoader<GameConfigurations>();
        gameConfigurationFromJson = configurationLoader.LoadConfiguration<GameConfigurations>(jsonGameConfiguration.ToString());
    }

    private void LoadXmlConfiguration()
    {
        TextAsset jsonGameConfiguration = Resources.Load<TextAsset>("XML/XMLGameConfiguration");
        IConfigurationLoader configurationLoader = new XMLConfigurationLoader<GameConfigurations>();
        gameConfigurationFromJson = configurationLoader.LoadConfiguration<GameConfigurations>(jsonGameConfiguration.ToString());
    }
}