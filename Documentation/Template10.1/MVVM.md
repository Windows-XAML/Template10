# Table of Contents

- [Introduction](#introduction)
- [BindableBase](#bindablebase)
- [DelegateCommand](#delegatecommand)
- [ViewModelBase](#viewmodelbase)
- [MVVM Frameworks](#mvvm-frameworks)

# Introduction
MVVM - Model, View, View-Model - is a classic design pattern favoured by most XAML apps. It separates the user interface
into the View, the data-portion into the Model and the connecting logic into the View-Model.

In Template 10, it is not a requirement to use MVVM and Template 10 is not intending to displace any other MVVM framework.
That said, if you are designing your app to use this design pattern, Template 10 provides several features to make this easier.

All of the classes described below can be overridden if you want to use an alternative implementation, e.g. a MVVM framework
that you are already using or are more familiar with.

# BindableBase
This class provides two features: it implements `INotifyPropertyChanged`, which is key if you want the UI to be updated
when a databound value changes, and it ensures that the `PropertyChanged` event is marshalled onto the current UI thread. The
latter is important if your app is running multiple threads.

Here is an example of implementing a property in a class that inherits `BindableBase`:

````csharp
private string _Value = "Default";
public string Value
{
    get
    {
        return _Value;
    }
    set
    {
        Set(ref _Value, value);
    }
}
````

````csharp
// This is what interested parties (e.g. XAML) hook onto in order to be notified when property values change in the
// class that is inheriting BindableBase.
public event PropertyChangedEventHandler PropertyChanged;

// Fire the PropertyChanged event for the specified property name. By default, the property name will be taken
// from the name of the caller which would normally be the property itself. This can be used, though, to fire
// the event for any property, which is useful if several properties need to be seen to be updated at once.
public virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)

// As with the previous method, this also fires the PropertyChanged event but the expression is evaluated in order
// to determine the property name.
public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)

// If the current value in storage isn't the same as the passed value, sets it to that value and then fires the
// PropertyChanged event for the specified property name, or the caller's name if no name is given.
public virtual bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)

// The same as the previous method except that the property name is derived from the evaluated propertyExpression.
public virtual bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
````

# DelegateCommand
This is Template 10's implementation of `ICommand`, with support for `CommandParameter`. The intention of `ICommand` is to
allow `Button` controls to execute code but with the added value of being able to check whether or not that code is
allowed to execute. The introduction of x:Bind in XAML means that `DelegateCommand` might not be as needed as it once used to be.

Example of implementing code-behind for a Save button:

````csharp
DelegateCommand _SaveCommand;

public DelegateCommand SaveCommand => _SaveCommand ?? (_SaveCommand = new DelegateCommand(ExecuteSave, CanSave));

private void ExecuteSave()
{
    _dataService.Save();
}

private bool CanSave()
{
    return _dataService.IsValid();
}
````

If you need to specify a parameter to the command, there is the `DelegateCommand<T>` derivative. An example taken from the code
for the HamburgerMenu:

````csharp
DelegateCommand<HamburgerButtonInfo> _navCommand;

public DelegateCommand<HamburgerButtonInfo> NavCommand => _navCommand ?? (_navCommand = new DelegateCommand<HamburgerButtonInfo>(ExecuteNav));

void ExecuteNav(HamburgerButtonInfo commandInfo)
{
    DebugWrite($"HamburgerButtonInfo: {commandInfo}");

    if (commandInfo == null)
    {
        throw new NullReferenceException("CommandParameter is not set");
    }

    if (commandInfo.PageType != null)
    {
        Selected = commandInfo;
    }
    else
    {
        ExecuteNavButtonICommand(commandInfo);
        commandInfo.RaiseTapped(new RoutedEventArgs());
        CommandButttonTapped?.Invoke(commandInfo, null);
    }
}
````

and the corresponding snippet from the XAML:

````XAML
<ToggleButton
    ...
    Command="{Binding NavCommand, ElementName=ThisPage}"
    CommandParameter="{Binding}"
    ...
    >
````

If you need the command to run as an async process, there is `AwaitableDelegateCommand`.

````csharp
public AwaitableDelegateCommand(Func<AwaitableDelegateCommandParameter, Task> execute, Func<AwaitableDelegateCommandParameter, bool>
canexecute = null)
````

> This section needs more information, particularly an example of how to use it! :)

# ViewModelBase
This class provides an implementation of INavigable, which enables the navigation service, and also inherits BindableBase. All
view-models in Template 10 should inherit this class to gain maximum benefit from the library.

The class provides the following:

````csharp
public virtual IDispatcherWrapper Dispatcher { get; set; }
public virtual INavigationService NavigationService { get; set; }
public virtual IStateItems SessionState { get; set; }

public virtual Task OnNavigatingFromAsync(NavigatingEventArgs args)
public virtual Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
````

Each of the methods can be overridden in your own code so that you can perform whatever operations you desire at the
appropriate time. `OnNavigatingFromAsync` occurs before the navigation takes place and can be used to stop the navigation
from happening while `OnNavigatedFromAsync` occurs after the navigation has happened and is also used when suspension is
taking place in order to give the app a chance to save data away.

> Note that the three properties are marked with `[JsonIgnore]` so that they will not be saved if the view-model is serialised.

# MVVM Frameworks
To leverage other MVVM frameworks, simply inherit from Template10.Mvvm.ViewModelBase or implement
Template10.Services.NavigationService.INavigable. This enables OnNavigatedTo/From in your view-models.

For an example of how to do this with MVVMLight, see the MvvmLight sample in the T10 GitHub repository.
