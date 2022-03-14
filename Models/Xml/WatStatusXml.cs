using System.Xml.Serialization;

namespace WATPlanCore.Models;

public class WatStatus
{

    [XmlRoot(ElementName="title")]
    public class Title {
        [XmlAttribute(AttributeName="name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName="folder")]
    public class Folder {
        [XmlAttribute(AttributeName="name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName="ProcessedCnt")]
        public string ProcessedCnt { get; set; }
        [XmlAttribute(AttributeName="NotProcessedCnt")]
        public string NotProcessedCnt { get; set; }
        [XmlAttribute(AttributeName="icon")]
        public string Icon { get; set; }
    }

    [XmlRoot(ElementName="data")]
    public class Data {
        [XmlElement(ElementName="folder")]
        public List<Folder> Folder { get; set; }
    }

    [XmlRoot(ElementName="who")]
    public class Who {
        [XmlAttribute(AttributeName="lastupdatetext")]
        public string Lastupdatetext { get; set; }
    }

    [XmlRoot(ElementName="xml")]
    public class Xml {
        [XmlElement(ElementName="title")]
        public Title Title { get; set; }
        [XmlElement(ElementName="data")]
        public Data Data { get; set; }
        [XmlElement(ElementName="who")]
        public Who Who { get; set; }
    }
}