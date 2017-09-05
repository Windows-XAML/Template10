namespace Template10.Services.Dialog
{
    public interface IMessageBoxEx2 : IMessageBoxEx
    {
        string Text { get; set; }
        MessageBoxEx SetPrimaryButton(string displayText);
        MessageBoxEx SetSecondaryButton(string displayText);
        MessageBoxEx SetCloseButton(string displayText);
    }

    public partial class MessageBoxEx : IMessageBoxEx2
    {
        IMessageBoxEx2 Two => this as IMessageBoxEx2;

        string IMessageBoxEx2.Text { get; set; }

        /// <summary>
        ///     Sets the primary button.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        MessageBoxEx IMessageBoxEx2.SetPrimaryButton(string displayText)
        {
            _contentDialogEx.PrimaryButtonText = displayText;
            _contentDialogEx.IsPrimaryButtonEnabled = true;
            return this;
        }

        /// <summary>
        ///     Sets the secondary button.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        MessageBoxEx IMessageBoxEx2.SetSecondaryButton(string displayText)
        {
            _contentDialogEx.SecondaryButtonText = displayText;
            _contentDialogEx.IsSecondaryButtonEnabled = true;
            return this;
        }

        /// <summary>
        ///     Sets the close button.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        MessageBoxEx IMessageBoxEx2.SetCloseButton(string displayText)
        {
            // Appears setting the text automatically enables the Close button
            _contentDialogEx.CloseButtonText = displayText;
            return this;
        }
    }
}
