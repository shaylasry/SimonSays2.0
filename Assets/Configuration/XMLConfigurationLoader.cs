using System;
using System.IO;
using System.Xml.Serialization;

public class XMLConfigurationLoader : IConfigurationLoader
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
            throw new Exception("Error loading XML configuration: " + e.Message);
        }
    }
}
