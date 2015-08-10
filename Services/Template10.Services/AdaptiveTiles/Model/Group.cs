using System.Collections.Generic;
using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Group : IVisualChild
    {
        public List<SubGroup> SubGroups
        {
            get; set;
        }
        public XElement GetXElement()
        {
            var element = new XElement("group");
            foreach (var subgroup in SubGroups)
            {
                element.Add(subgroup.GetXElement());
            }

            return element;
        }
    }
}