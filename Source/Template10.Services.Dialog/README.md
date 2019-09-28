# DialogService
`DialogService` simplifies the common ways of interacting with the UWP `ContentDialog` and `MessageBox` types. The `MessageBoxEx` class provides a simple way of displaying `ContentDialog` instances. The `DialogService` provides methods to simplify the display of the `MessageBoxEx` and the `DialogManager` is used to ensure only a single `MessageBoxEx` instance is displayed at any one time.

```csharp
public interface IDialogService
{
    Task<IUICommand> ShowAsync(MessageDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null);
        
    Task<ContentDialogResult> ShowAsync(ContentDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null);
        
    Task<MessageBoxResult> AlertAsync(string content, IDialogResourceResolver resolver = null);

    Task<MessageBoxResult> AlertAsync(string title, string content, IDialogResourceResolver resolver = null);

    Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null);

    Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null);

    Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null);

    Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null);
}

public interface IMessageBoxEx
{
    IDialogResourceResolver Resolver { get; set; }
    MessageBoxType Type { get; set; }
    string Text { get; set; }
    ElementTheme RequestedTheme { get; set; }
}
```

