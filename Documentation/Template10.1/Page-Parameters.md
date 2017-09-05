# Table of Contents

- [Introduction](#introduction)
- [Navigation refresher](#navigation-refresher)
- [Passing complex parameters](#passing-complex-parameters)

# Introduction
Page navigation in UWP apps allows for a single parameter to be passed to the destination page. Historically, this has been a
*simple* parameter such as a string or integer because of restrictions imposed by the navigation code included in the Windows 8
templates.

Template 10 overcomes a lot of those restrictions by using serialization to ensure that complex objects (e.g. custom classes) are
serialized to a string before navigating to the page then deserialized back to the original object type so that the associated
view-model can work with the passed data without any knowledge of the fact that this conversion has taken place.

If the object being passed can't be serialized, though, or would impose a high processing cost to serialized and deserialize, the
session state can be used instead to store the object and a key to the object passed instead.

# Navigation refresher
When using the [`NavigationService`](./Navigation-Service), the `Navigate` method takes a value that is passed to the underlying
page as parameter data:

````csharp
void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
````

In all variants of the method, `parameter` is of type `object`, meaning you can pass anything to the page being navigated to.

Here is a simple example:

````csharp
var value = "Hello world";
await NavigationService.NavigateAsync(typeof(Views.MainPage), value);
````

then, in the view-model for MainPage:

````csharp
public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
{
    var value = parameter as string;
    // TODO: use the parameter somehow
    await Task.CompletedTask;
}
````

# Passing complex parameters
Passing something more complicated, such as a class instance, is just as easy - so long as your class can be serialised. If,
however, your class cannot be serialised or the class contents are too complex to spend the cost of serialising and deserialising,
the alternative is to use session state to store the object and then pass the key instead.

`SessionState` is a standard property in Template 10 view-models that inherit ViewModelBase. This is the same, identical
SessionState available in every view-model and App.xaml.cs. 

````csharp
var value = new MyType();
SessionState.Add("MyKey", value);
await NavigationService.NavigateAsync(typeof(Views.MainPage), "MyKey");
````

then, in the view-model for MainPage:

````csharp
public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
{
    var key = parameter as string;
    if (SessionState.ContainsKey(key))
    {
        var value = SessionState[key] as MyType;
        // TODO: use the parameter
    }
    else
    {
        // TODO: handle missing parameter
    }
    await Task.CompletedTask;
}
````

> Note that SessionState is only held in memory. Therefore, if the app gets suspended, SessionState may not contain the values When
the app is resumed. The code must cope with this in the *handle missing parameter* section of the example code, perhaps by using the
key to retrieve the data through an alternative manner.
