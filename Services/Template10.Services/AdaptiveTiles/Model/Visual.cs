using System.Collections.Generic;
using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Visual : VisualBindingBase, IAdaptiveTile
    {
        public string Version { get; set; }

        public List<Binding> Bindings
        {
            get; set;
        }
        public XElement GetXElement()
        {
            var element = new XElement("visual", GetXAttributes());
            if (!string.IsNullOrWhiteSpace(Version))
            {
                element.Add(new XAttribute("version", Version));
            }

            foreach (var binding in Bindings)
            {
                element.Add(binding.GetXElement());
            }

            return element;
        }
    }
}