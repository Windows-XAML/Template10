using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public abstract class VisualBindingBase
    {
        public string Lang { get; set; }
        public Uri BaseUri { get; set; }
        public VisualBranding? Branding { get; set; }
        public bool? AddImageQuery { get; set; }
        public string ContentId { get; set; }
        public string DisplayName { get; set; }

        protected List<XAttribute> GetXAttributes()
        {
            var attributes = new List<XAttribute>();
            if (!string.IsNullOrWhiteSpace(Lang))
            {
                attributes.Add(new XAttribute("lang", Lang));
            }

            if (BaseUri != null)
            {
                attributes.Add(new XAttribute("baseUri", BaseUri.ToString()));
            }

            if (Branding.HasValue)
            {
                attributes.Add(new XAttribute("branding", Branding.Value.ToString().FirstLetterToLower()));
            }

            if (AddImageQuery.HasValue)
            {
                attributes.Add(new XAttribute("addImageQuery", AddImageQuery.Value));
            }

            if (!string.IsNullOrWhiteSpace(ContentId))
            {
                attributes.Add(new XAttribute("contentId", ContentId));
            }

            if (!string.IsNullOrWhiteSpace(DisplayName))
            {
                attributes.Add(new XAttribute("displayName", DisplayName));
            }

            return attributes;
        }
    }
}