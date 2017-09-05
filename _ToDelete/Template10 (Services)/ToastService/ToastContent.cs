using Windows.UI.Notifications;

namespace Template10.Services.ToastService
{
    public class ToastContent
    {
        private ToastContent(ToastTemplateType toastTemplateType, string title, string content, string secondContent, string image, string altText, string launchArguments)
        {
            ToastTemplateType = toastTemplateType;
            Title = title;
            Content = content;
            SecondContent = secondContent;
            LaunchArguments = launchArguments;
            Image = image;
            AltText = altText;
        }

        public static ToastContent CreateToastText01(string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastText01, null, content, null, null, null, launchArguments);
        }

        public static ToastContent CreateToastText02(string title, string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastText02, title, content, null,  null, null, launchArguments);
        }

        public static ToastContent CreateToastText03( string title, string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastText03, title, content, null,  null, null, launchArguments);
        }

        public static ToastContent CreateToastText04(string title, string content, string secondContent, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastText04, title, content, secondContent, null, null, launchArguments);
        }

        public static ToastContent CreateToastImageAndText01(string image, string altText, string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastImageAndText01, null, content, null, image, altText, launchArguments);
        }

        public static ToastContent CreateToastImageAndText02(string image, string altText, string title, string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastImageAndText02, title, content, null, image, altText, launchArguments);
        }

        public static ToastContent CreateToastImageAndText03(string image, string altText, string title, string content, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastImageAndText03, title, content, null, image, altText, launchArguments);
        }

        public static ToastContent CreateToastImageAndText04(string image, string altText, string title, string content, string secondContent, string launchArguments)
        {
            return new ToastContent(ToastTemplateType.ToastImageAndText04, title, content, secondContent, image, altText, launchArguments);
        }

        public ToastTemplateType ToastTemplateType { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string SecondContent { get; private set; }
        public string LaunchArguments { get; private set; }
        public string Image { get; set; }
        public string AltText { get; set; }
    }
}

