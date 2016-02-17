using System.Linq;
using Windows.Data.Xml.Dom;

namespace Template10.Services.ToastService
{
    using Windows.UI.Notifications;

    public class ToastHelper
    {
        private const string TextNode = "text";
        private const string ImageNode = "image";
        private const string LaunchAttr = "launch";
        private const string SrcAttr = "src";
        private const string AltAttr = "alt";

        public void ShowToast(ToastContent toastContent)
        {
            var toast = BuildToastNotification(toastContent);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        private static ToastNotification BuildToastNotification(ToastContent toastContent)
        {
            var xml = ToastNotificationManager.GetTemplateContent(toastContent.ToastTemplateType);

            AppendLaunchArgumentToNotification(toastContent, xml);
            AppendImageToNotification(toastContent, xml);
            AppendTextElementsToNotification(toastContent, xml);

            return new ToastNotification(xml);
        }

        private static void AppendLaunchArgumentToNotification(ToastContent toastContent, XmlDocument xml)
        {
            if (!string.IsNullOrWhiteSpace(toastContent.LaunchArguments))
                xml.DocumentElement.SetAttribute(LaunchAttr, toastContent.LaunchArguments);
        }

        private static void AppendTextElementsToNotification(ToastContent toastContent, XmlDocument xml)
        {
            var elements = xml.GetElementsByTagName(TextNode);

            var hasTitleOrContentElement = elements.Count == 1;
            var hasTitleAndContentElement = elements.Count == 2;
            var hasTitleContentAndSecondContentElement = elements.Count == 3;


            if (hasTitleOrContentElement || hasTitleAndContentElement)
                elements[0].InnerText = toastContent.Title ?? toastContent.Content ?? string.Empty;

            if (hasTitleAndContentElement)
                elements[0].InnerText = toastContent.Content ?? string.Empty;

            if (hasTitleContentAndSecondContentElement)
                elements[0].InnerText = toastContent.SecondContent ?? string.Empty;
        }

        private static void AppendImageToNotification(ToastContent toastContent, XmlDocument xml)
        {
            var imageElements = xml.GetElementsByTagName(ImageNode);
            if (!imageElements.Any())
                return;

            ((XmlElement)imageElements[0]).SetAttribute(SrcAttr, toastContent.Image ?? string.Empty);
            ((XmlElement)imageElements[0]).SetAttribute(AltAttr, toastContent.AltText ?? string.Empty);
        }
    }
}