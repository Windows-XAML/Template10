using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{
    public interface IMessageBoxEx
    {
        IResourceResolver Resolver { get; set; }
        MessageBoxType Type { get; set; }
        string Text { get; set; }
    }

    /// <summary>
    /// This class encapsulates the behavior for a simple message box.
    /// </summary>
    public partial class MessageBoxEx : IMessageBoxEx
    {
        public MessageBoxEx(string text, MessageBoxType messageBoxType = MessageBoxType.Ok, IResourceResolver resolver = null)
        {
            Text = text;
            Type = messageBoxType;
            Resolver = resolver ?? Settings.DefaultResolver;
        }

        public string Text { get; set; }
        public MessageBoxType Type { get; set; }
        public IResourceResolver Resolver { get; set; }

        public async Task<MessageBoxResult> ShowAsync(TimeSpan? timeout = null, CancellationToken? token = null)
        {
            var dialog = new ContentDialog { Content = Text };
            SetupButtons(dialog);
            var result = await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);
            return DetermineResult(result);
        }

        private MessageBoxResult DetermineResult(ContentDialogResult result)
        {
            switch (result)
            {
                case ContentDialogResult.None:
                    return MessageBoxResult.Cancel;
                case ContentDialogResult.Primary:
                    switch (Type)
                    {
                        case MessageBoxType.Ok: return MessageBoxResult.Ok;
                        case MessageBoxType.OkCancel: return MessageBoxResult.Ok;
                        case MessageBoxType.YesNo: return MessageBoxResult.Yes;
                        case MessageBoxType.YesNoCancel: return MessageBoxResult.Yes;
                        default: return MessageBoxResult.Cancel;
                    }
                case ContentDialogResult.Secondary:
                    switch (Type)
                    {
                        case MessageBoxType.Ok: return MessageBoxResult.Cancel;
                        case MessageBoxType.OkCancel: return MessageBoxResult.Cancel;
                        case MessageBoxType.YesNo: return MessageBoxResult.No;
                        case MessageBoxType.YesNoCancel: return MessageBoxResult.No;
                        default: return MessageBoxResult.Cancel;
                    }
                default:
                    return MessageBoxResult.Cancel;
            }
        }

        private void SetupButtons(ContentDialog dialog)
        {
            switch (Type)
            {
                case MessageBoxType.Ok:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Ok));
                    break;
                case MessageBoxType.OkCancel:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Ok));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.Cancel));
                    break;
                case MessageBoxType.YesNo:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Yes));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.No));
                    break;
                case MessageBoxType.YesNoCancel:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Yes));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.No));
                    dialog.SetCloseButton(Resolver.Resolve(ResourceTypes.Cancel));
                    break;
                default:
                    break;
            }
        }
    }
}
