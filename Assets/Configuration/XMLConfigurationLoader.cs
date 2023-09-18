using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;

public class XMLConfigurationLoader : IConfigurationLoader
{
    public Dictionary<string, Dictionary<string, object>> LoadConfiguration(string filePath)
    {
        try
        {
            Dictionary<string, Dictionary<string, object>> configuration = new Dictionary<string, Dictionary<string, object>>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList levelNodes = xmlDoc.SelectNodes("/Configurations/*");

            foreach (XmlNode levelNode in levelNodes)
            {
                Dictionary<string, object> levelConfig = new Dictionary<string, object>();

                foreach (XmlNode propertyNode in levelNode.ChildNodes)
                {
                    if (propertyNode.NodeType == XmlNodeType.Element)
                    {
                        string propertyName = propertyNode.Name;

                        // Attempt to convert propertyValue to int
                        if (int.TryParse(propertyNode.InnerText, out int intValue))
                        {
                            levelConfig[propertyName] = intValue;
                        }
                        // Attempt to convert propertyValue to float
                        else if (float.TryParse(propertyNode.InnerText, out float floatValue))
                        {
                            levelConfig[propertyName] = floatValue;
                        }
                        // Attempt to convert propertyValue to bool
                        else if (bool.TryParse(propertyNode.InnerText, out bool boolValue))
                        {
                            levelConfig[propertyName] = boolValue;
                        }
                        // Otherwise, treat it as a string
                        else
                        {
                            levelConfig[propertyName] = propertyNode.InnerText;
                        }
                    }
                }

                configuration[levelNode.Name] = levelConfig;
            }

            return configuration;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading configuration: {e.Message}");
            return null;
        }
    }
}
