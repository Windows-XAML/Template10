#PageKeys

Template 10 allows developers to navigate by page type (which is standard on the XAML Frame) and by key. Using a key allows developers to isolate view types from view models. This is a common approach for developer implementing MVVM. But, this approach is not necessary to successfully use Template 10.

##Pages Enum

Lorem ipsum...

````chsarp
public enum Pages { MainPage }
````

##BootStrapper.PageKeys<T>()

Lorem ipsum...

````chsarp
public override Task OnInitializeAsync(IActivatedEventArgs args)
{
    var keys = PageKeys<Pages>();
    keys.Add(Pages.MainPage, typeof(Views.MainPage));
    return base.OnInitializeAsync(args);
}
````

##NavigationSerice.Navigate<T>()

Lorem ipsum...

````chsarp
public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
{
    NavigationService.Navigate(Pages.MainPage);
    return Task.FromResult<object>(null);
}
````