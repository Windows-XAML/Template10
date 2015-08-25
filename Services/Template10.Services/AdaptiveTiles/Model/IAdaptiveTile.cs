using System.Xml.Linq;

namespace Template10.Services.AdaptiveTiles.Model
{
    public interface IAdaptiveTile
    {
        XElement ToXElement();
    }
}