using System.Xml.Linq;
using Windows.Data.Xml.Dom;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Tile : IAdaptiveTile
    {
        public Visual Visual { get; set; }

        public XElement GetXElement()
        {
            return new XElement("tile",new XAttribute("version","3"), Visual.GetXElement());
        }

        public XmlDocument GetXmlDocument()
        {
            XDocument document = new XDocument(this.GetXElement());
            document.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            var xmlDoc = new XmlDocument();
            var xmlAsString = document.ToString();
            xmlDoc.LoadXml(document.ToString());
            return xmlDoc;
        }
    }
}
