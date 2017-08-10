# Table of Contents

- [Introduction](#Introduction)
- [FileService](#fileservice)
- [KeyboardService](#keyboardservice)
- [LoggingService](#loggingservice)
- [NavigationService](./Navigation-Service)
- [PopupService](#popupservice)
- [ProfileService](#profileservice)
- [SecretService](#secretservice) - new
- [SerializationService](#serializationservice)
- [SettingsService](#settingsservice)

The following are currently only available in the source of T10 so can only be used if you
download the source and compile T10 yourself:

- [NetworkAvailableService](#networkavailableservice)
- [QueueServices](#queueservices)
- [ViewService](#viewservice)

# Introduction
A service is a functionality wrapper that provides a defined interface with an implementation
of the interface. Using interfaces makes it easier to perform unit testing, Template 10 comes
with a number of services, all intended to make it easier to get started with development of
your app.

# FileService
`FileService` simplifies the common ways of interacting with files by abstracting the storage
types to `Local`, `Roaming` and `Temporary`. In doing so, the service takes care of correctly
creating the appropriate paths. All storage strategies are within the app's own data directory.
All calls that allow a storage strategy to be specified default to local storage.

````csharp
// Deletes a file in the specified storage strategy. key is the path of the file in storage. This
// is safe to call even if the file doesn't exist already. Returns true if the deletion operation
// was successful (i.e. the file doesn't exist).
async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)

// This checks for the existence of a file, either in the specified storage strategy or in the
// specified folder.
async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
async Task<bool> FileExistsAsync(string key, StorageFolder folder)

// ReadFileAsync reads and deserializes a file into specified type T. T specifies the type into which
// to deserialize the file content while key is the path to the file within the specified storage strategy.
async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)

// WriteFileAsync serializes an object (of specified type T) and writes it to file in the specified storage
// strategy. Key specifies the path to the file within the storage strategy. You can also alter the behaviour
// if the code if the destination file already exists. The method returns a boolean to indicate whether or not
// the file exists.
async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local,
            CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
````

# KeyboardService
The intent of the `KeyboardService` is to provide an abstracted way of reliably handling keyboard input.
It was initially created to handle the back and forward gestures in the `NavigationService` but has been
abstracted in this service for general use.

> Documentation needed

# LoggingService
This service is useful if you want to see the internal T10 operations and/or if you want a centralised logging
service. The framework of this service has been designed to give you a lot of control but the default implementation
is very basic, doesn't take advantage of that control and can actually only be used if you've compiled T10
yourself with debugging enabled (since it relies on `System.Diagnostics.Debug.WriteLine`).

````csharp
// Allows you to control what level of messages you want to appear in the log. Note that the default logging
// implementation ignores this.
public enum Severities { Template10, Info, Warning, Error, Critical }
// Allows you to control where the messages are displayed/sent. The default implementation only implements
// a solution for the Debug target, sending it to the normal debug output window. Specifying Log causes an
// exception.
public enum Targets { Debug, Log }
````

To use the logging service, either compile T10 with debugging enabled or add the following to App.xaml.cs:

````csharp
public App()
{
    ...
    LoggingService.WriteLine = new DebugWriteDelegate(LogHandler);
    ...
}

public void LogHandler(string text = null, Severities severity = Severities.Info, Targets target = Targets.Debug, [CallerMemberName]string caller = null)
{
    System.Diagnostics.Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} {severity} {caller} {text}");
}
````

You can, of course, make LogHandler much more sophisticated but the above code at least matches the level provided
by T10. This ability to replace the default implementation means that you can then merge Template 10 logging (i.e.
logging what T10 is doing) with your own log output and send it wherever you like, e.g. HockeyApp.

# NavigationService
Please see the separate [NavigationService](./Navigation-Service) documentation.

# NetworkAvailableService
> Currently only available in T10 source code.

The `NetworkAvailableService` provides a very simple interface to allow apps to check the current status of
the network available to the device and to be advised when the network availability changes.

````csharp
// Hook into notifications of when the availability changes. Note that this is not an event handler - only
// one function can be called.
public Action<ConnectionTypes> AvailabilityChanged { get; set; }

// Check on the current status to see if access to the Internet is possible.
public async Task<bool> IsInternetAvailable();

// Check on the current status to see if there is any network availability.
public async Task<bool> IsNetworkAvailable();
````

# PopupService
The `PopupService` standardises the mechanisms to display a pop-up window that either fills the screen or takes its
size from the size of the current window.

This service is used by [`BootStrapper`](./BootStrapper) to handle the splash screen at start-up of the app.

````csharp
public enum PopupSize { FullScreen, ContentBased }

// Create and return a Popup set to the desired size (FullScreen or ContentBased) and the passed content.
public Popup Create(PopupSize size, UIElement content = null)

// Retrieve the content currently being displayed in the specified Popup.
public UIElement GetContent(Popup popup)

// Change the content for the specified Popup to the passed content. Note that this updates the size of
// the Popup to match that of the current window.
public void SetContent(Popup popup, UIElement content)

// Creates, displays and returns a Popup. Basically calls Create then sets IsOpen to true.
public Popup Show(PopupSize size, UIElement content = null)
````

There are some extensions provided that can be performed on a Popup:

````csharp
// Return the content for the popup.
UIElement popup.GetContent();

// Change the content for the popup and reset the size of the popup to match the current window's size.
void popup.SetContent(UIElement newContent);

// Change the content for the popup and show the popup.
void popup.Show(UIElement newContent);

// Show the popup.
void popup.Show();

// Change the content for the popup and hide the popup.
void popup.Hide(UIElement newContent);

// Hide the popup.
void popup.Hide();
````

# ProfileService
The `ProfileService` is a very simple class that retrieves the first name, last name and the display name of the
person currently using the device.

````csharp
public string FirstName;
public string LastName;
public string DisplayName;
````

# QueueServices
> Currently only available in T10 source code.

This provides a simple enqueue and dequeue interface while also providing events when these interfaces
are called.

````csharp
public event TypedEventHandler<T> Enqueued;
public event TypedEventHandler<T> Dequeued;

// Adds the item to the end of the queue being maintained by this service and triggers the Enqueued event.
public void Enqueue(T item)
// Removes and returns the item at the start of the queue being maintained by this service, and triggers the
// Dequeued event.
public T Dequeue()
````

> The name of this service doesn't follow the convention (it is a plural). Does it need renaming? Does more code need writing? It doesn't seem to be very defensive.

# SecretService
The `SecretService` securely stores secret strings. Secret strings are any string value you don't want to store as plain text (like in a file or database or setting). The `SecretService` is a simple wrapper for Windows' [Credential Manager](https://msdn.microsoft.com/en-us/library/windows/desktop/aa374792(v=vs.85).aspx). The Credential Manager handles the encryption part and secures the result internally. This is the Windows standard for securing strings.

````
public interface ISecretService
{
    string ReadSecret(string key);
    string ReadSecret(string container, string key);
    void WriteSecret(string key, string secret);
    void WriteSecret(string container, string key, string secret);
}
````

Example usage:

````
SecretService service = new SecretService();

// save a secret in the default container
public string Token
{
    get { return service.ReadSecret(nameof(Token)); }
    set { service.WriteSecret(nameof(Token), value); }
}

The purpose of a custom container is if you want to reuse a key without overwriting another value. For example if you store two tokens, they can both be stored as "Token" in separate containers. 

// save a secret in a custom container
public string Token
{
    get { return service.ReadSecret("OAuth", nameof(Token)); }
    set { service.WriteSecret("OAuth", nameof(Token), value); }
}
````

Saving OAuth tokens is a good use of the `SecretService`. If you maintain any credentials, keys, hash values, or tokens for the user or your services, I recommend you consider storing the values using this service. 

# SerializationService
The `SerializationService` provides a simple abstracted interface to a service that implements consistent
serialization and deserialization of objects:

````csharp
// Serialize the passed object to a string. Returns null if the parameter is null. Returns an empty string
// if the parameter is an empty string.
string Serialize(object parameter);

// Converts the passed string back to an object. If the passed parameter is null, null is returned. If the
// string is empty, an empty string is returned.
object Deserialize(string parameter);

// Converts the passed string back to a object of type T. If deserialization fails, the default value
// for type T is returned. Note that if T is the wrong type for the passed serialized value, an
// InvalidCastException can occur. This can be avoided by using the next method.
T Deserialize<T>(string parameter);

// Attempts to deserialize the value while catching any InvalidCastException that may occur. Returns
// true if a value value was obtained, otherwise false is returned, along with the defaul value for
// type T.
bool TryDeserialize<T>(string parameter, out T result);
````

The `NavigationService` sets up an instance to the Template 10-included JSON `SerializationService`:

````csharp
SerializationService = Services.SerializationService.SerializationService.Json;
````

# SettingsService
The intent of the `SettingsService` is to provide an abstracted way of interacting with a service that properly
supports serialization and an interface implementation for unit testing purposes. There are two parts to using
`SettingsService`:

1. Implementing the different settings that are read and written.
1. Changing those values, typically from a view-model associated with the UI that alllows the user to make those changes.

If you look at the Minimal template, for example, the file Services\SettingsServices\SettingsService.cs provides
a template example of how to implement the first part, while the file ViewModels\SettingsPageViewModel.cs provides
an example of how to implement the second part.

In more detail, here is a snippet of how to implement the first part:

````csharp
public class SettingsService
{
    Template10.Services.SettingsService.ISettingsHelper _helper;

    // Set up the hook to the settings helper interface.
    private SettingsService()
    {
        _helper = new Template10.Services.SettingsService.SettingsHelper();
    }

    // Example of a setting that can be changed from a view model
    public bool UseShellBackButton
    {
        get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
        set
        {
            _helper.Write(nameof(UseShellBackButton), value);
            // Perform some additional logic here to ensure that when the value
            // changes, the app behaves in a consistent manner.
            BootStrapper.Current.NavigationService.GetDispatcherWrapper().Dispatch(() =>
            {
                BootStrapper.Current.ShowShellBackButton = value;
                BootStrapper.Current.UpdateShellBackButton();
            });
        }
    }
}
````

and here is a snippet of how to implement the second part:

````csharp
public class SettingsPartViewModel : ViewModelBase
{
    Services.SettingsServices.SettingsService _settings;

    public SettingsPartViewModel()
    {
        _settings = Services.SettingsServices.SettingsService.Instance;
    }

    public bool UseShellBackButton
    {
        get { return _settings.UseShellBackButton; }
        set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
    }
}
````

The above are very simple examples of how to use this service. The SettingsService is flexible enough to
support settings that are local to the device or can roam across instances of the app running on multiple
devices.

````csharp
// Returns the underlying service for the specified strategy (local or roaming). Can be useful if you want
// to interact with the settings at a deeper level than that provided by the high level methods described
// below.
ISettingsService Container(SettingsStrategies strategy)

// Returns true or false to indicate if a setting with the specified key already exists.
bool Exists(string key, SettingsStrategies strategy = SettingsStrategies.Local)

// Removes the specified setting from the settings strategy container. This is safe to call if the
// setting doesn't exist.
void Remove(string key, SettingsStrategies strategy = SettingsStrategies.Local)

// Writes the value of type T to the setting with the specified key name. Note that the object
// T must be serializable otherwise an exception will occur.
void Write<T>(string key, T value, SettingsStrategies strategy = SettingsStrategies.Local)

// Returns an object of type T for the setting with the specified key name. If the setting
// doesn't exist in the specified storage strategy, the value "otherwise" is returned.
T Read<T>(string key, T otherwise, SettingsStrategies strategy = SettingsStrategies.Local)
````

As can be seen, the methods all default to local settings but these can be changed on a per-setting
basis to use roaming. For example, if you wanted to have the theme choice to be consistent across
all devices, you might have a method like the following in your SettingsServices code:

````csharp
public ApplicationTheme AppTheme
{
    get
    {
        var theme = ApplicationTheme.Light;
        var value = _helper.Read<string>(nameof(AppTheme), theme.ToString(), SettingsStrategies.Roam);
        return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
    }
    set
    {
        _helper.Write(nameof(AppTheme), value.ToString(), SettingsStrategies.Roam);
        (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
    }
}
````

Note that it is important that both the Read and Write calls specify the same SettingsStrategies value!

# ViewService
> Currently only available in T10 source code.
