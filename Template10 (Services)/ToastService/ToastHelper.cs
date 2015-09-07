namespace Template10.Services.ToastService
{
    using System.Linq;
    using Windows.UI.Notifications;

    public class ToastHelper
    {
        const string textNode = "text";
        const string launchAttr = "launch";
        const string srcAttr = "src";

        public void ShowToastText01(string content, string arg = null)
        {
            // show toast
            var toast = BuildToastText01(content, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastText02(string title, string content, string arg = null)
        {
            // show toast
            var toast = BuildToastText02(content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastText03(string title, string content, string arg = null)
        {
            // show toast
            var toast = BuildToastText03(content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastText04(string title, string content, string content2, string arg = null)
        {
            // show toast
            var toast = BuildToastText04(content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastImageAndText01(string image, string content, string arg = null)
        {
            // show toast
            var toast = BuildToastImageAndText01(image, content, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastImageAndText02(string image, string title, string content, string arg = null)
        {
            // show toast
            var toast = BuildToastImageAndText02(image, content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastImageAndText03(string image, string title, string content, string arg = null)
        {
            // show toast
            var toast = BuildToastImageAndText03(image, content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public void ShowToastImageAndText04(string image, string title, string content, string content2, string arg = null)
        {
            // show toast
            var toast = BuildToastImageAndText04(image, content, title, arg);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public ToastNotification BuildToastText01(string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText01;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // launch arg
            if (arg != null)
                xml.DocumentElement.SetAttribute(launchAttr, arg);

            // content
            var text = xml.CreateTextNode(content);
            elements[0].AppendChild(text);

            // toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastText02(string title, string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // title
            var text = xml.CreateTextNode(title);
            elements[0].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[1].AppendChild(text);

            // sh toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastText03(string title, string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText03;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // title
            var text = xml.CreateTextNode(title);
            elements[0].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[1].AppendChild(text);

            // show toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastText04(string title, string content, string content2, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // title
            var text = xml.CreateTextNode(title);
            elements[0].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[1].AppendChild(text);

            // content2
            text = xml.CreateTextNode(content2);
            elements[2].AppendChild(text);

            // toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastImageAndText01(string image, string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // image
            elements[0].Attributes
                .First(x => x.LocalName.Equals(srcAttr)).InnerText = image;

            // content
            var text = xml.CreateTextNode(content);
            elements[1].AppendChild(text);

            // show toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastImageAndText02(string image, string title, string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // image
            elements[0].Attributes
                .First(x => x.LocalName.Equals(srcAttr)).InnerText = image;

            // title
            var text = xml.CreateTextNode(title);
            elements[1].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[2].AppendChild(text);

            // show toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastImageAndText03(string image, string title, string content, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText03;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // image
            elements[0].Attributes
                .First(x => x.LocalName.Equals(srcAttr)).InnerText = image;

            // title
            var text = xml.CreateTextNode(title);
            elements[1].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[2].AppendChild(text);

            // show toast
            return new ToastNotification(xml);
        }

        public ToastNotification BuildToastImageAndText04(string image, string title, string content, string content2, string arg = null)
        {
            // build toast
            var template = ToastTemplateType.ToastText02;
            var xml = ToastNotificationManager.GetTemplateContent(template);
            var elements = xml.GetElementsByTagName(textNode);

            // image
            elements[0].Attributes
                .First(x => x.LocalName.Equals(srcAttr)).InnerText = image;

            // title
            var text = xml.CreateTextNode(title);
            elements[1].AppendChild(text);

            // content
            text = xml.CreateTextNode(content);
            elements[2].AppendChild(text);

            // content2
            text = xml.CreateTextNode(content2);
            elements[3].AppendChild(text);

            // show toast
            return new ToastNotification(xml);
        }
    }
}
