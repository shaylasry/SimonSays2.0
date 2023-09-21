using System;
using System.IO;
using System.Xml.Serialization;

using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLConfigurationLoader<T> : IConfigurationLoader
{
    public T LoadConfiguration<T>(string asset)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(asset))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading XML configuration: " + e.Message);
            return default(T);
        }
    }
}
