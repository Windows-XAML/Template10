using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Binding : VisualBindingBase, IAdaptiveTile
    {
        public string Template { get; set; }
        public string Fallback { get; set; }

        public VisualHintTextStacking? HintTextStacking { get; set; }

        public VisualHintPresentation? HintPresentation { get; set; }

        private int? _hintOverlay;
        /// <summary>
        /// none|logo|name|nameAndLogo
        /// </summary>
        public int? HintOverlay
        {
            get
            {
                return _hintOverlay;
            }

            set
            {
                if (value >= 0 && value <= 100)
                {
                    _hintOverlay = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(HintOverlay), "Must in range of [0-100]");
                }
            }
        }

        public string HintLockDetailedStatus1 { get; set; }
        public string HintLockDetailedStatus2 { get; set; }
        public string HintLockDetailedStatus3 { get; set; }

        public List<IVisualChild> Children { get; set; } = new List<IVisualChild>();

        public XElement ToXElement()
        {
            var element = new XElement("binding", GetXAttributes());
            element.Add(new XAttribute("template", Template));
            if (HintTextStacking.HasValue)
            {
                element.Add(new XAttribute("hint-textStacking", HintTextStacking.Value.ToString().FirstLetterToLower()));
            }

            if (HintOverlay.HasValue)
            {
                element.Add(new XAttribute("hint-overlay", HintOverlay.Value));
            }

            if (HintPresentation.HasValue)
            {
                element.Add(new XAttribute("hint-presentation", HintPresentation.Value.ToString().FirstLetterToLower()));
            }

            if (!string.IsNullOrWhiteSpace(HintLockDetailedStatus1))
            {
                element.Add(new XAttribute("hint-lockDetailedStatus1", HintLockDetailedStatus1));
            }

            if (!string.IsNullOrWhiteSpace(HintLockDetailedStatus2))
            {
                element.Add(new XAttribute("hint-lockDetailedStatus2", HintLockDetailedStatus2));
            }

            if (!string.IsNullOrWhiteSpace(HintLockDetailedStatus3))
            {
                element.Add(new XAttribute("hint-lockDetailedStatus3", HintLockDetailedStatus3));
            }

            if (!string.IsNullOrWhiteSpace(Fallback))
            {
                element.Add(new XAttribute("fallback", Fallback));
            }

            foreach (var child in Children)
            {
                element.Add(child.ToXElement());
            }

            return element;
        }
    }
}