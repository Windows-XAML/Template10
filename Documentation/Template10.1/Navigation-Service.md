# Table of Contents

- [Introduction](#introduction)
- [Navigating with the service](#navigating-with-the-service)
- [Other examples of calling Navigate](#other-examples-of-calling-navigate)
- [Properties, methods and overrides](#properties-methods-and-overrides)

# Introduction
The `NavigationService` centralizes `Frame` interaction and, as such, allows navigation to take place from view-models
rather than just code-behind.

The service also ensures that basic behaviors occur, such as calling `OnNavigatedTo` in a page's view-model before
navigating to the page.

Template 10 relies on every XAML frame control to have a companion Template 10 navigation service. The Bootstrapper creates
the root frame using several methods in concert:

- `InitializeFrameAsync` -> `CreateRootFrame`
- `NavigationServiceFactory` -> `CreateNavigationService`

Developers can intercept this process along several steps:

- Override `CreateRootFrame` - the return type of this override is a XAML Frame. If default content, properties or
registration in your app is custom, you can implement it here.
- Override `CreateNavigationService` - the return type of this override is a Template 10 INavigationService. If it
is necessary for you to create the service in a special way, including mocking, do it here.

> Note: the root frame and the subsequent navigation service are created after the OnInitializeAsync override is
called by the Bootstrapper. See [Bootstrapper activation paths](./Bootstrapper#activation-paths) for more details.

The service is testable because it is interface-based. It also corrects a few nuisance flaws in the native XAML `Frame`.

After reading this documentation, you may also want to read about [how to pass parameters to pages](./Page-Parameters) and
about [how to use the navigation cache](./Navigation-Cache).

# Navigating with the service
The main method provided by the service, `Navigate`, takes one mandatory parameter and two optional parameters:

````csharp
void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
````

There are synchronous and asynchronous calls to navigate to a page. The asynchronous calls return true or false to indicate
whether or not the navigation was successful.

The first parameter to `Navigate` is either the type of page to navigate to or a key to specify the page. Navigating to a
page type is as simple as:

````csharp
NavigationService.Navigate(typeof(Views.MainPage));
````

If you need to use a dictionary and navigate using the dictionary keys, you can use an approach similar to the following:

````csharp
// define your Enum
public Enum Pages { MainPage, DetailPage }

// setup the keys dict
var keys = BootStrapper.PageKeys<Views>();
keys.Add(Pages.MainPage, typeof(Views.MainPage));
keys.Add(Pages.DetailPage, typeof(Views.DetailPage));

...

// use Navigate<T>()
NavigationService.Navigate(Pages.MainPage);
````

The second, optional, parameter to `Navigate` is a parameter to pass to the page, e.g.:

````csharp
NavigationService.Navigate(typeof(Views.DetailPage), this.Value);
````

The `NavigationService` automatically takes care of serializing and de-serializing whatever is passed as a parameter. This
allows more complex objects to be passed to pages than could be handled with Windows 8. However, the object must be
capable of serialising and de-serialising otherwise an exception will be raised. If special handling is required to perform
those functions, you can provide your own code rather than using the T10-provided JSON-based `SerializationService`.

The final optional parameter to `Navigate` allows you to alter the appearance of the transition as the pages change.

# Other examples of calling Navigate
````csharp
// from an async method
public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
{
    await NavigationService.NavigateAsync(typeof(Views.MainPage));
}

// from inside any window
var nav = WindowWrapper.Current().NavigationServices.FirstOrDefault();
nav.Navigate(typeof(Views.DetailPage), this.Value);

// from/with a reference to a Frame
var nav = WindowWrapper.Current(MyFrame).NavigationServices.FirstOrDefault();
nav.Navigate(typeof(Views.DetailPage), this.Value);
````

# Properties, methods and overrides
````csharp
// Gets from the underlying Frame (via FrameFacade)
bool CanGoBack { get; }
// Gets from the underlying Frame (via FrameFacade)
bool CanGoForward { get; }
// Gets from the underlying Frame
object Content { get; }
// The parameter originally passed to the FrameFacade
object CurrentPageParam { get; }
// The page type for the FrameFacade
Type CurrentPageType { get; }
// Gets the Dispatcher for the Window Wrapper for this navigation service
DispatcherWrapper Dispatcher { get; }
// Gets the Frame for this navigation service
Frame Frame { get; }
// Gets the FrameFacade for this navigation service
FrameFacade FrameFacade { get; }
// True if this is the main window and not a secondary window
bool IsInMainView { get; }
// Gets the navigation state for the underlying Frame
string NavigationState { get; set; }
// Allows you to use your own serialization service instead of the one provided by T10
ISerializationService SerializationService { get; set; }

// Allows you to run code after a frame has been loaded and previous state restored
event TypedEventHandler<Type> AfterRestoreSavedNavigation;
// Called before page state is saved. Allows the event handler to cancel the process.
event EventHandler<CancelEventArgs<Type>> BeforeSavingNavigation;

// ??
void ClearCache(bool removeCachedPagesInBackStack = false);
// Clears the back stack
void ClearHistory();
// Return the navigation service associated with the specified frame
INavigationService GetForFrame(Frame frame);
// Go backwards in the page stack. Safe to call without testing CanGoBack first.
void GoBack();
// Go forwards in the page stack (what are the semantics of this?). Safe to call
// without testing CanGoForward first although implementation differs from GoBack
// so may need checking/reconciliation?
void GoForward();
// Trigger loading of the page and restoration of settings/parameters in an async manner. Returns true if successful.
Task<bool> LoadAsync();
// Navigate to the specific page type, optionally passing a parameter and specifying a page transition
void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
// Navigate to the page associated with the specified key, optionally passing a paramter and specifying a page transition
void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
// As per Navigate except asynchronous and indicates whether or not successful
Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
// As per Navigate except asynchronous and indicates whether or not successful
Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
// Creates and opens a secondary view. Returns a control to aid with the management of the view during its lifetime.
Task<ViewLifetimeControl> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);
// Cause the current page to redraw/refresh
void Refresh();
// Obsolete - use LoadAsync now. N.B. Newer T10???
Task<bool> RestoreSavedNavigationAsync();
// Doesn't do anything.
void Resuming();
// Saves the details of the current page to state. Can be aborted with BeforeSavingNavigation.
SaveAsync
// Obsolete - use SaveAsync now.
Task SaveNavigationAsync();
// Triggers a save of state ahead of suspension occurring.
Task SuspendingAsync();
````
