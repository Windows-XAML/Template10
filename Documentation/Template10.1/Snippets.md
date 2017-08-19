# Table of Contents

- [How to install and use](#how-to-install-and-use)
- [T10_Command](#t10_command)
- [T10_CommandTyped](#t10_commandtyped)
- [T10_Dispatch](#t10_dispatch)
- [T10_DispatchAwait](#t10_dispatchawait)
- [T10_DispatchAwaitResult](#t10_dispatchawaitresult)
- [T10_Event](#t10_Event)
- [T10_EventTyped](#t10_eventtyped)
- [T10_INotifyPropertyChanged](#t10_inotifypropertychanged)
- [T10_Property](#t10_property)
- [T10_PropertySetting](#t10_propertysetting)
- [T10_PropertyValidated](#t10_propertyvalidated)
- [T10_SampleData](#t10_sampledata)
- [T10_SingletonClass](#t10_singletonclass)
- [T10_ViewModel](#t10_viewmodel)
- [T10_ViewModelFull](#t10_viewmodelfull)

# How to install and use
The Template 10 project provides a number of useful code snippets to help make the most of the libary.

At the moment, the snippets are only available if you download the source code for Template 10 and then manually configure
Visual Studio to find the snippets. There is work being done to make the snippets available as a Visual Studio Extension (like
the Templates pack) but this hasn't yet been finished.

To add the snippets to your copy of Visual Studio, start by clicking Tools > Snippet Manager. Make sure that the selected
language is **CSharp** then click on the **Add** button. Find the downloaded sources for Template 10 then find this directory:

*VisualStudio\Vsix\Snippets\CSharp*

When you've navigated to this folder, click on **Select folder**.

> Please note that this directory also contains a XAML snippet. This currently cannot be used in this configuration as
it needs to be in its own directory. The source code for T10 will be updated soon to reflect this.

To use a snippet, simply type the snippet name and press TAB to have it expanded in Visual Studio.

> As noted above, the XAML snippet (T10_PageContent) currently cannot be used.

# T10_Command
Expands to create a command definition:
```csharp
DelegateCommand _executeCommand;
public DelegateCommand ExecuteCommand
    => _executeCommand ?? (_executeCommand = new DelegateCommand(() =>
    {
        throw new NotImplementedException();
    }, () => true));
```
# T10_CommandTyped
Expands to create a strongly type command:
```csharp
DelegateCommand<object> _MyCommand;
public DelegateCommand<object> MyCommand
    => _MyCommand ?? (_MyCommand = new DelegateCommand<object>(MyCommandExecute, MyCommandCanExecute));
bool MyCommandCanExecute(object param) => true;
void MyCommandExecute(object param)
{
    throw new NotImplementedException();
}
```
# T10_Dispatch
Expands to code snippet that allows an action to be executed on the current UI thread:
```csharp
WindowWrapper.Current().Dispatcher.Dispatch(() => { /* TODO */ });
```
# T10_DispatchAwait
Expands to code snippet that allows an async action to be executed on the current UI thread and the task to be awaited:
```csharp
await WindowWrapper.Current().Dispatcher.DispatchAsync(() => { /* TODO */ });
```
# T10_DispatchAwaitResult
Expands to code snippet that allows an async action to be executed on the current UI thread, the task to be awaited and the result to be assigned:
```csharp
var MyVariable = await WindowWrapper.Current().Dispatcher
                 .DispatchAsync<string>(() => { return default(string); });
```
# T10_Event
Expands to create an event and method to raise the event:
```csharp
public event EventHandler MyEvent;
void RaiseMyEvent() => MyEvent?.Invoke(this, new EventArgs { });
```
# T10_EventTyped
Expands to create an event that takes a strongly typed value:
```csharp
public event EventHandler<string> MyEvent;
void RaiseMyEvent(string value) => MyEvent?.Invoke(this, value);
```
>**Note:** by convention the value would derive from EventArg.

# T10_INotifyPropertyChanged
Expands to implement the INotifiyPropertyChanged interface:
```csharp
#region INotifyPropertyChanged

public event PropertyChangedEventHandler PropertyChanged;

public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
{
    if (!Equals(storage, value))
    {
        storage = value;
        RaisePropertyChanged(propertyName);
    }
}

public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

#endregion
```

>**Note:** You still need to add the interface to your class definition.
```csharp
public class MyViewModel : INotifiyPropertyChanged
{
    // expand T10_INotifyPropertyChanged here
}
```
# T10_Property
Expands to create a property definition that raises the PropertyChanged event:
```csharp
string _MyProperty = default(string);
public string MyProperty { get { return _MyProperty; } set { Set(ref _MyProperty, value); } }
```

>**Note:** the `Set` method checks if the incoming value matches the existing value and only assigns the value\raises the `PropertyChanged` event if the values don't match (useful as in some circumstances, dur to knock on effects/relationships the assignment may be called recusively).

# T10_PropertySetting
Expands to create a property that leverages the `SettingsHelper` to read\write the value to the local app settings:

```csharp
public string MyProperty
{
    get { return (new SettingsHelper()).Read<string>(nameof(MyProperty), default(string)); }
    set { (new SettingsHelper()).Write(nameof(MyProperty), value); }
}
```
# T10_PropertyValidated
Expands to create a property that calls functions for read and writing values that provides the means to validate data before writing, etc.:
```csharp
public string MyProperty { get { return Read<string>(); } set { Write(value); } }  
```
# T10_SampleData
Expands to create some color sample data:
```csharp
#region SampleData
          
public class ColorInfo
{
    public Color Color { get; internal set; }
    public string Name { get; internal set; }
    public string Hex => Color.ToString();
    public Brush Brush => new SolidColorBrush(Color);
}

public ObservableCollection<ColorInfo> Colors { get; }
    = new ObservableCollection<ColorInfo>(typeof(Colors).GetRuntimeProperties()
        .Select(x => new ColorInfo { Name = x.Name, Color = (Color)x.GetValue(null) }));

ColorInfo _Color = default(ColorInfo);
public ColorInfo Color { get { return _Color; } set { Set(ref _Color, value); } }

#endregion
```
# T10_SingletonClass
A snippet that expands to create the basis for a singleton class:
```csharp
public class YourClassName
{
    private YourClassName() 
    {
        // constructor
    }
    private static YourClassName sInstance;
    public static YourClassName Instance => sInstance ?? (sInstance = new YourClassName());
}
```
# T10_ViewModel
A snippet that expands to create the basis for a view model class:
```csharp
public class MyPageViewModel : ViewModelBase
{
}
```
# T10_ViewModelFull
A snippet that expands to create a full example of a view model class:
```csharp
public class MyPageViewModel : ViewModelBase
{
    public MyPageViewModel() 
    {
        if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
        {
            // design-time experience
        }
        else
        {
            // runtime experience
        }
    }
    
    // sample data
    public IEnumerable<object> Items =>
        typeof(Windows.UI.Colors).GetRuntimeProperties()
            .Select(x => new { Name = x.Name, Color = (Color)x.GetValue(null) });
    
    public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
    {
        if (state.Any())
        {
            // restore state
            state.Clear();
        }
        else
        {
            // use parameter
        }
        return Task.CompletedTask;
    }

    public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
    {
        if (suspending)
        {
            // save state
        }
        return Task.CompletedTask;
    }

    public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
    {
        args.Cancel = false;
        return Task.CompletedTask;
    }
}
```
