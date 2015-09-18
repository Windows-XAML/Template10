using System.Xml.Linq;
using Windows.Data.Xml.Dom;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Tile : IAdaptiveTile
    {
        public Visual Visual { get; set; }

        public XElement ToXElement()
        {
            return new XElement("tile",new XAttribute("version","3"), Visual.ToXElement());
        }

        public XmlDocument ToXmlDocument()
        {
            XDocument document = new XDocument(this.ToXElement())
            {
                Declaration = new XDeclaration("1.0", "utf-8", "yes")
            };
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(document.ToString());
            return xmlDoc;
        }
    }
}
