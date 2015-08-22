using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Template10.Services.PrimaryTileService
{
    public class PrimaryTileHelper
    {
        const string badgeXpath = "/badge";
        const string valueAttr = "value";

        internal void UpdateBadge(int value)
        {
            var type = Windows.UI.Notifications.BadgeTemplateType.BadgeNumber;
            var badge = BadgeUpdateManager.GetTemplateContent(type);

            var xml = badge.SelectSingleNode(badgeXpath) as XmlElement;
            xml.SetAttribute(valueAttr, value.ToString());

            var updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            updater.Update(new BadgeNotification(badge));
        }
    }
}