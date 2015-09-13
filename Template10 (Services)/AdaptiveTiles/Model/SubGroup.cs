using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class SubGroup : IVisualChild
    {
        private int? _hintWeight;
        /// <summary>
        /// none|logo|name|nameAndLogo
        /// </summary>
        public int? HintWeight
        {
            get
            {
                return _hintWeight;
            }

            set
            {
                if (value >= 0 && value <= 100)
                {
                    _hintWeight = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(HintWeight), "Must in range of [0-100]");
                }
            }
        }
        public SubGroupTextStacking? HintTextStacking { get; set; }

        public List<ISubGroupChild> Children { get; set; }
        public XElement ToXElement()
        {
            var element = new XElement("subgroup");
            if (HintWeight.HasValue)
            {
                element.Add(new XAttribute("hint-weight", HintWeight.Value));
            }
            if (HintTextStacking.HasValue)
            {
                element.Add(new XAttribute("hint-textStacking", HintTextStacking.Value.ToString().FirstLetterToLower()));
            }

            foreach (var child in Children)
            {
                element.Add(child.ToXElement());
            }

            return element;
        }
    }
}