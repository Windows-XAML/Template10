#Template10.Samples.PageKeysSample

Template 10 allows developers to navigate by page type (which is standard on the XAML Frame) and by key. Using a key allows developers to isolate view types from view models. This is a common approach for developers implementing MVVM (but is not necessary to successfully use Template 10).

##Pages Enum

The first step is creating a custom Enum that represents the pages we want to navigate to.

````chsarp
public enum Pages { MainPage, DetailPage }
````

##BootStrapper.Template10.Samples.PageKeysSample<T>()

Then, tipically in the BootStrapper.OnInitializeAsync method, we populate the BootStrapper.Template10.Samples.PageKeysSample dictionary associating each page type with the corresponding enum value.

````chsarp
public override Task OnInitializeAsync(IActivatedEventArgs args)
{
    var keys = Template10.Samples.PageKeysSample<Pages>();
    keys.Add(Pages.MainPage, typeof(MainPage));
    keys.Add(Pages.DetailPage, typeof(DetailPage));

    return base.OnInitializeAsync(args);
}
````

##NavigationService.Navigate<T>()

In this way, we can use an overload of the NavigationService.Navigate method that takes as first argument the enum value corresponding to the page type we want to navigate to (instead of use the page type directly).

````chsarp
public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
{
    NavigationService.Navigate(Pages.MainPage);
    return Task.CompletedTask;
}
````

When we follow the MVVM pattern, this is the recommended approach for navigation because view models shouldn't know anything about page types.
