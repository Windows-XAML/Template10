using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public class Text : IVisualChild, ISubGroupChild
    {
        public string Content { get; set; }
        public bool? HintWrap { get; set; }
        public int? HintMaxLines { get; set; }
        public int? HintMinLines { get; set; }
        public TextStyle? HintStyle { get; set; }
        public TextHintAlign? HintAlign { get; set; }

        public XElement ToXElement()
        {
            var element = new XElement("text", Content);

            if (HintWrap.HasValue)
            {
                element.Add(new XAttribute("hint-wrap", HintWrap.Value));
            }
            if (HintMaxLines.HasValue)
            {
                element.Add(new XAttribute("hint-maxLines", HintMaxLines.Value));
            }
            if (HintMinLines.HasValue)
            {
                element.Add(new XAttribute("hint-wrap", HintMinLines.Value));
            }
            if (HintAlign.HasValue)
            {
                element.Add(new XAttribute("hint-align", HintAlign.Value.ToString().FirstLetterToLower()));
            }
            if (HintStyle.HasValue)
            {
                element.Add(new XAttribute("hint-style", HintStyle.Value.ToString().FirstLetterToLower()));
            }

            return element;
        }
    }
}