using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("configurations")]
public struct GameConfigurations
{
    [XmlElement("configuration")]
    public List<SingleGameConfiguration> configurations;
}