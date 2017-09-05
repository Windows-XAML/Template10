using System;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{
    public interface IMessageBoxEx
    {
        // empty
    }

    /// <summary>
    /// This class encapsulates the behavior for a simple message box.
    /// </summary>
    /// <remarks>
    /// This class uses a <see cref="ContentDialogEx"/> behind the scenes.
    /// </remarks>
    public partial class MessageBoxEx : IMessageBoxEx
    {
        private readonly ContentDialogEx _contentDialogEx;
        private MessageBoxType _messageBoxType;
        IResourceResolver _resolver;

        public MessageBoxEx(IResourceResolver resolver = null)
            : base()
        {
            _resolver = resolver ?? Settings.DefaultResolver;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageBoxEx" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public MessageBoxEx(string text, IResourceResolver resolver = null)
            : this(resolver)
        {
            _contentDialogEx = new ContentDialogEx
            {
                Content = text,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Two.Text = text;

            // default as an OK 
            WithOk();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageBoxEx" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="messageBoxType">Type of the message box.</param>
        /// <exception cref="ArgumentOutOfRangeException">messageBoxType - null</exception>
        public MessageBoxEx(string text, MessageBoxType messageBoxType, IResourceResolver resolver = null)
            : this(text, resolver)
        {
            switch (messageBoxType)
            {
                case MessageBoxType.Ok:
                    WithOk();
                    break;
                case MessageBoxType.OkCancel:
                    WithOkCancel();
                    break;
                case MessageBoxType.YesNo:
                    WithYesNo();
                    break;
                case MessageBoxType.YesNoCancel:
                    WithYesNoCancel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageBoxType), messageBoxType, null);
            }
        }

        /// <summary>
        ///     Configures the <see cref="MessageBoxEx" /> to display an Ok button.
        /// </summary>
        /// <returns>
        ///     <see cref="MessageBoxEx" />
        /// </returns>
        MessageBoxEx WithOk()
        {
            _messageBoxType = MessageBoxType.Ok;
            Two.SetPrimaryButton(_resolver.Resolve(ResourceTypes.Ok));
            return this;
        }

        /// <summary>
        ///     Configures the <see cref="MessageBoxEx" /> to display Ok and Cancel buttons.
        /// </summary>
        /// <returns>
        ///     <see cref="MessageBoxEx" />
        /// </returns>
        MessageBoxEx WithOkCancel()
        {
            Two.SetPrimaryButton(_resolver.Resolve(ResourceTypes.Ok));
            Two.SetSecondaryButton(_resolver.Resolve(ResourceTypes.Cancel));
            _messageBoxType = MessageBoxType.OkCancel;
            return this;
        }

        /// <summary>
        ///     Configures the <see cref="MessageBoxEx" /> to display Yes and No buttons.
        /// </summary>
        /// <returns>
        ///     <see cref="MessageBoxEx" />
        /// </returns>
        MessageBoxEx WithYesNo()
        {
            Two.SetPrimaryButton(_resolver.Resolve(ResourceTypes.Yes));
            Two.SetSecondaryButton(_resolver.Resolve(ResourceTypes.No));
            _messageBoxType = MessageBoxType.YesNo;
            return this;
        }

        /// <summary>
        ///     Configures the <see cref="MessageBoxEx" /> to display Yes, No and Cancel buttons.
        /// </summary>
        /// <returns>
        ///     <see cref="MessageBoxEx" />
        /// </returns>
        MessageBoxEx WithYesNoCancel()
        {
            Two.SetPrimaryButton(_resolver.Resolve(ResourceTypes.Yes));
            Two.SetSecondaryButton(_resolver.Resolve(ResourceTypes.No));
            Two.SetCloseButton(_resolver.Resolve(ResourceTypes.Cancel));
            _messageBoxType = MessageBoxType.YesNoCancel;
            return this;
        }

        /// <summary>
        ///     Shows the <see cref="MessageBoxEx" /> using an Async operation. The method is "safe" as it ensures
        ///     that any currently displayed MessageBox is closed before this MessageBox opens.
        /// </summary>
        /// <returns>Task&lt;MessageBoxResult&gt;.</returns>
        /// <exception cref="NotSupportedException">
        ///     Thrown if the dialog return type is unknown.
        /// </exception>
        internal async Task<MessageBoxResult> ShowAsync()
        {
            var result = await _contentDialogEx.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None:
                    // assuming this is Cancel
                    return MessageBoxResult.Cancel;

                case ContentDialogResult.Primary:
                    switch (_messageBoxType)
                    {
                        case MessageBoxType.Ok:
                        case MessageBoxType.OkCancel:
                            return MessageBoxResult.Ok;
                        case MessageBoxType.YesNo:
                        case MessageBoxType.YesNoCancel:
                            return MessageBoxResult.Yes;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case ContentDialogResult.Secondary:
                    switch (_messageBoxType)
                    {
                        case MessageBoxType.Ok:
                        case MessageBoxType.OkCancel:
                            return MessageBoxResult.Cancel;
                        case MessageBoxType.YesNo:
                        case MessageBoxType.YesNoCancel:
                            return MessageBoxResult.No;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                default:
                    {
                        throw new NotSupportedException($"Result:{result}");
                    }
            }
        }
    }
}
