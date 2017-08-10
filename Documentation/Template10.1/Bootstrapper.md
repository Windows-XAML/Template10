# Table of Contents

- [Introduction](#introduction)
- [Navigation service](#navigation-service)
- [Activation paths](#activation-paths)
- [Splash page](#splash-page)
- [Root frame](#root-frame)
- [Suspension management](#suspension-management)
- [Window wrapper](#window-wrapper)
- [Window created](#window-created)
- [Dispatcher wrapper](#dispatcher-wrapper)
- [Modal dialog](#modal-dialog)
- [Dependency injection](#dependency-injection)
- [Properties, methods and overrides](#properties-methods-and-overrides)

# Introduction
Bootstrapper (Library/Common/Bootstrapper.cs) is responsible for the core capabilities of Template 10. It derives from Application,
and is implemented in your app in the App.xaml/App.xaml.cs files. 

Its responsibilities include:

- Creating the navigation service
- Handling an extended splash screen
- Creating the root frame
- Aggregating activation paths
- Automating suspension management

In addition, it also handles:

- Creating the initial window wrapper
- Creating the initial dispatcher wrapper
- Wrapping the root frame with a modal dialog
- Support for dependency injection for view-models

Simplest implementation looks like this:

````csharp
namespace Sample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App() { InitializeComponent(); }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
````

> This implementation comes from the blank template. You can install all templates through the Visual Studio extension gallery. Simply search for "Template 10".

# Navigation service

The Bootstrapper takes responsibility for creating an instance of the `NavigationService`. For more information about this key
service, please see the separate [NavigationService](./Navigation-Service) documentation.

# Activation paths

Bootstrapper seals away many of the overrides that ship with the out-of-the-box Application class. Where a standard UWP app
overrides `OnActivated()`, `OnLaunched()`, and several other activation variations, Template 10 simplifies the startup pipeline
to the following:

> `constructor` -> `OnInitializeAsync` -> `OnPrelaunchAsync()` -> `OnStartAsync()`

- Application `constructor` - several Application properties, including `RequestedTheme` can only be set in the constructor of
Application. Typically, applications setup global settings here.
- `OnInitializeAsync` executes first and it executes every time, even if the application is coming out of suspension. The type
of code appropriate here would be authentication and cache checks. If the developer is creating a custom frame, spoiling the
automatic implementation of the Bootstrapper, that code should also be here since the Bootstrapper automatically creates the
frame+navigation service after this override is called. 
- `OnPrelaunchAsync` optionally executes before `OnStartAsync` and only during prelaunch. Prelaunch is a feature of the platform
that launches an app for 16 seconds without UI, then suspends it. Prelaunch is not guaranteed as it is influenced by available
resources. A developer would handle Prelaunch if they need to prevent UI (for example: marking a user as 'available') that might
occur on start. When handled, the developer can prevent `OnStartAsync` from continuing. If prevented, Template 10 will
automatically call `OnStartAsync` when the app resumes from the prelaunch suspend.
- `OnStartAsync` is the one and only entry point to an application that is not resuming from suspension. `OnStartAsync` combines
every launch and activation scenario into a single, simple override. A developer can test the launch arguments of the method to
determine the startup kind. A developer can also use the custom StartKind argument to the method to determine if the original
startup method was intended to be launch or activate (if that is important to the startup logic). 

````csharp
public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
{
    var shareArgs = args as ShareTargetActivatedEventArgs;
    if (shareArgs != null)
    {
        var key = nameof(ShareOperation);
        SessionState.Add(key, shareArgs.ShareOperation);
        NavigationService.Navigate(typeof(Views.MainPage), key);
    }
    else
    {
        NavigationService.Navigate(typeof(Views.MainPage));
    }
    return Task.CompletedTask;
}
````

> The code above is from the Share Target sample project.

**Note**: The helper method `DetermineStartCause` in Bootstrapper can help a developer determine the subtle startup causes
that are otherwise unclear. For example, determining if the primary or secondary tile, or toast caused of the startup. 

````csharp
public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
{
    switch (DetermineStartCause(args))
    {
        case AdditionalKinds.SecondaryTile:
            var tileargs = args as LaunchActivatedEventArgs;
            NavigationService.Navigate(typeof(Views.DetailPage), tileargs.Arguments);
            break;
        case AdditionalKinds.Toast:
            var toastargs = args as ToastNotificationActivatedEventArgs;
            NavigationService.Navigate(typeof(Views.DetailPage), toastargs.Argument);
            break;
        case AdditionalKinds.Primary:
        case AdditionalKinds.Other:
            NavigationService.Navigate(typeof(Views.MainPage));
            break;
    }
	return Task.CompletedTask;
}
````

> The code above is from the Toast and Tiles sample project.

# Splash page
See the [SplashScreen](./SplashScreen) documentation for an explanation of what Template 10 and particularly the
BootStrapper does to help you with the execution of long-running startup tasks.

# Root frame

The Bootstrapper automatically creates your app's root frame. However, the developer can intercept this by creating their own root
frame in the `OnInitializeAsync()` override. Why? Sometimes, like when using the Hamburger Menu, you want a custom root to your
application. If you want to do this, you can do something like this:

````csharp
public override UIElement CreateRootElement(IActivatedEventArgs e)
{
    var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
    return new ModalDialog
    {
        DisableBackButtonWhenModal = true,
        Content = new Views.Shell(service),
        ModalContent = new Views.Busy(),
    };
}
````

> This implementation comes from the hamburger template. You can install all templates through the Visual Studio extension
gallery. Simply search for "Template 10".

# Suspension management

With UWP, an app may be suspended into memory at any time. Suspended apps may be terminated, due to resource constraints, at
any time. Handling these states, their transitions, and providing a seamless user experience is the responsibility of the
developer. Template 10 helps automate this.

Automatically, the Bootstrapper calls `OnNavigatedFrom` on every active view-model. In most apps, there will be only one active
view-model. Should there be more, Bootstrapper will call them all. Each call will be wrapped in a single Deferral, and enable
await in the calls. Template 10 cannot extend the 10 second limitation imposed by the platform, so the developer remains
responsible to limit the suspend-time operation.

````csharp
public override Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
{
    if (suspending)
    {
        suspensionState[nameof(Value)] = Value;
    }
    return Task.CompletedTask;
}
````

Automatically, the Bootstrapper will save and restore the navigation state of every active navigation service. This means that
when the app is restored from termination, the navigation stack (including the back and forward stacks) will be restored. The
current page will be re-created, the `OnNavigatedTo` overrides will be called on the page and the view-model, and the
suspensionState passed to those methods will be populated.

````csharp
public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
{
    if (suspensionState.Any())
    {
        Value = suspensionState[nameof(Value)]?.ToString();
    }
    return Task.CompletedTask;
}
````

In addition to those automatic operations, a developer may also use:

- `OnSuspendingAsync()` in Bootstrapper is an override that is called after the OnNavigatedFrom() methods in every view-model
are called. With whatever time remains, the developer can implement global logic to handle suspension.
- `OnResume()` in Bootstrapper is an override that is called when the application is resuming. A developer can check cache
for staleness or anything else that makes sense, and is important to the specific application. The developer does NOT need to
restore navigation state, unless they have introduced a custom navigation pattern. In addition to standard suspension, resume
from prelaunch may also be handled here.
 
# Window wrapper

The window wrapper class is documented elsewhere, however, it is important to understand what each window wrapper maps to a  window. The platform refers to this as a view, but since we call XAML pages views, this is confusing. We do not mean XAML view, in this case, we mean something that has a title bar. Something we typically call a window. 

Since an app can have more than one window, there can also be more than one window wrapper. The Bootstrapper keeps track of the creation of windows and adds them to the static `ActiveWindows` collection in the window wrapper class. This occurs in the WindowCreated overload which is not available to developers using Bootstrapper. 

# Window created

If this is an important part of the lifecycle to your app, your code can handle the Bootstrapper's `WindowCreated` event, which
will include the `WindowCreatedEventArgs` from the original override. This is an edge case most developers will not need.  

Here's the internal implementation:

````csharp
public event EventHandler<WindowCreatedEventArgs> WindowCreated;
protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
{
    DebugWrite();

    if (!WindowWrapper.ActiveWrappers.Any())
        Loaded();

    // handle window
    var window = new WindowWrapper(args.Window);
    WindowCreated?.Invoke(this, args);
    base.OnWindowCreated(args);
}
````

# Dispatcher wrapper

The dispatcher wrapper is documented elsewhere, however, here's the summary: the XAML CoreDispatcher is how an operation that is off the UI thread can execute an operation on the UI thread. The dispatcher is generally not available to non-UI threads, but can be accessed in Template 10 through the current window wrapper. The dispatcher wrapper, however, is really just a series of helper methods intended to make dispatching code simpler.

# Modal dialog

The modal dialog control is [documented elsewhere](./Controls#modaldialog), however, it's important to understand that it is used
to display one of two content. The main content or the overlaying content, meant/intended to be an overlay of some kind, or a
modal dialog. An example of this might be a login form or a busy indicator.  

The Bootstrapper automatically wraps the root frame in a modal dialog. It exposes this through the `Bootstrapper.ModalDialog` property.
Here, a developer can set their own ModalContent and the ModalDialog's IsModal value. 

> Note: developers who intercept the standard Frame creation pipeline - for example, using the HamburgerMenu Shell approach - will
find the Bootstrapper.ModalDialog property to be null, but this can be custom-implemented by the developer.

````csharp
public override Task OnInitializeAsync(IActivatedEventArgs args)
{
    // content may already be shell when resuming
    if ((Window.Current.Content as ModalDialog) == null)
    {
        // setup hamburger shell
        var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
        Window.Current.Content = new ModalDialog
        {
            DisableBackButtonWhenModal = true,
            Content = new Views.Shell(nav),
            ModalContent = new Views.Busy(),
        };
    }
    return Task.CompletedTask;
}
````

# Dependency injection

Dependency injection is a common design pattern that Template 10 supports, but does not natively implement. That being said,
Template 10 enables dependency injection specifically with Bootstrapper.`ResolveForPage()`. Overriding this method, allows a
developer to inject (or return) any `INavigable` view-model into a page immediately after initial navigation, while still
maintaining the standard navigation pipeline.

1. In App.xaml.cs ovverride ResolveForPage to resolve your ViewModel:

````csharp
 public override INavigable ResolveForPage(Page page, NavigationService navigationService)
 {
    if (page is MainPage)
    {
        // With MVVMLight.SimpleIoC it would be:
        return SimpleIoc.Default.GetInstance<MainPageViewModel>();
        // With Unity it would be something like: 
        // return (AppController.UnityContainer as UnityContainer).Resolve<INavigable>();
    }
    else
       return base.ResolveForPage(page, navigationService);
}
````

2. Clean The Page XAML, remove '`<Page.DataContext>`' section and leave xaml.cs like:

````csharp
public sealed partial class MainPage : Page
{
    MainPageViewModel _viewModel;

    public MainPageViewModel ViewModel
    {
        get { return _viewModel ?? (_viewModel = (MainPageViewModel)DataContext); }
    }
}
````

3. Now you are ready to inject your dependencies in your ViewModel's constructor:
````csharp
public MainPageViewModel(IMyService myService)
{
    _myService = myService;
    // Etcetera...
}
````

# Properties, methods and overrides

````csharp
bool AutoExtendExecutionSession { get; set; }
bool AutoRestoreAfterTerminated { get; set; }
bool AutoSuspendAllFrames { get; set; }
// CacheMaxDuration indicates the maximum TimeSpan for which cache data
// will be preserved. If Template 10 determines cache data is older than
// the specified MaxDuration it will automatically be cleared during start.
TimeSpan CacheMaxDuration { get; set; }
// Returns the current instance of BootStrapper ... though since you are already
// referencing BootStrapper in order to reach Current, it isn't clear what the
// intended purpose is ...
BootStrapper Current { get; }
// Can be None, Running, BeforeInit, AfterInit, BeforeLaunch, AfterLaunch, BeforeActivate,
// AfterActivate, BeforeStart or AfterStart. Doesn't *seem* to be referenced anywhere though ...
States CurrentState { get; set; }
// Deprecated - use AutoRestoreAfterTerminated instead.
bool EnableAutoRestoreAfterTerminated { get; set; }
// Show shell back button regardless of necessity
bool ForceShowShellBackButton { get; set; }
// The default frame is automatically wrapped in a modal dialog. This allows you to access or change the content.
UIElement ModalContent { get; set; }
// Expose automatic root wrapper 
ModalDialog ModalDialog { get; }
// Default service for first frame
INavigationService NavigationService { get; }
// This allows you to access the original arguments passed when the app is activated in case you need
// to access something that Template 10 would otherwise "hide" because of the simplification it presents
// around the different ways an app can be launched or activated.
IActivatedEventArgs OriginalActivatedArgs { get; }
// Provides an in-memory property bag
StateItems SessionState { get; set; }
// Show shell back button when necessary
bool ShowShellBackButton { get; set; }
// The SplashFactory is a Func that returns an instantiated Splash view.
// Template 10 will automatically inject this visual before loading the app.
Func<SplashScreen, UserControl> SplashFactory { get; }

// This event precedes the in-frame event by the same name. Handler can set Handled to
// prevent further activity.
event EventHandler<HandledEventArgs> BackRequested;
// This event precedes the in-frame event by the same name. Handler can set Handled to
// prevent further activity.
event EventHandler<HandledEventArgs> ForwardRequested;
// Part of a standardised property changed event handler. Not *entirely* clear if this is being used.
event PropertyChangedEventHandler PropertyChanged;
// Allows code to be executed whenever the status of the shell back button is potentially updated. Note that the handler
// is fired **regardless** of whether or not the status **actually** changes, just that UpdatedShellBackButton()
// has been called.
event EventHandler ShellBackButtonUpdated;
// Fires when OnWindowCreated() is called, thereby exposing window created operations
event EventHandler<WindowCreatedEventArgs> WindowCreated;

// Creates the NavigationService instance for a given Frame.
INavigationService CreateNavigationService(Frame frame)
// By default, Template 10 will set up the root element to be a T10 Modal Dialog
// control. If you desire something different, you can set it here.
UIElement CreateRootElement(IActivatedEventArgs e)
// This determines the simplest case for starting. This should handle 80% of common scenarios. 
// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
AdditionalKinds DetermineStartCause(IActivatedEventArgs args)
// Creates a new Frame and adds the resulting NavigationService to the WindowWrapper collection.
// In addition, it optionally will setup the shell back button to react to the nav of the Frame.
// A developer should call this when creating a new/secondary frame. The shell back button should
// only be setup one time.
INavigationService NavigationServiceFactory();
// OnInitializeAsync is where your app will do must-have up-front operations. It will be called even
// if the application is restoring from state. An app restores from state when the app was suspended
// and then terminated (PreviousExecutionState terminated).
Task OnInitializeAsync(IActivatedEventArgs args);
// Prelaunch may never occur. However, it's possible that it will. It is a Windows mechanism
// to launch apps in the background and quickly suspend them. Because of this, developers need to
// handle Prelaunch scenarios if their typical launch is expensive or requires user interaction.
//
// For Prelaunch, Template 10 does not continue the typical startup pipeline by default. 
// OnActivated will occur if the application has been prelaunched.
Task OnPrelaunchAsync(IActivatedEventArgs args, out bool runOnStartAsync);
// OnResuming is called when the application is returning from a suspend state of some kind.
//
// previousExecutionState can be Terminated, which typically does not raise OnResume.
// This is important because the resume model changes a little in Mobile.
void OnResuming(object s, object e, BootStrapper.AppExecutionState previousExecutionState);
// OnStartAsync is the one-stop-show override to handle when your app starts
// Template 10 will not call OnStartAsync if the app is restored from state.
// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
Task OnStartAsync(BootStrapper.StartKind startKind, IActivatedEventArgs args);
// OnSuspendingAsync will be called when the application is suspending, but this override
// should only be used by applications that have application-level operations that must
// be completed during suspension.
// 
// Using OnSuspendingAsync is a little better than handling the Suspending event manually
// because the asunc operations are in a single, global deferral created when the suspension
// begins and completed automatically when the last viewmodel has been called (including this method).
Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated);
// Dictionary of page keys for optional page key navigation. T must be a custom Enum.
Dictionary<T, Type> PageKeys<T>() where T : struct, IConvertible;
// If a developer overrides this method, the developer can resolve DataContext or unwrap DataContext 
// available for the Page object when using a MVVM pattern that relies on a wrapped/porxy around ViewModels
// It is, therefore, an optional dependency injection endpoint for creating view-models.
INavigable ResolveForPage(Page page, NavigationService navigationService);
// Refreshes shell button visiblity.
void UpdateShellBackButton();
````
