By default, apps have a very limited time to start presenting a user interface before Windows decides that the app is not
working properly and terminates it. If an app needs more time to get started, the solution is to use an extended splash
screen. This can present anything the developer wants to show to the user while the app finishes the startup phase, after
which the user interface is then displayed.

Template 10 makes the process simpler by providing a mechanism to display the extended splash screen at the appropriate point.

To make use of this feature, declare SplashFactory as in the `App` constructor example below:

````csharp
sealed partial class App : Template10.Common.BootStrapper
{
    public App()
    {
        InitializeComponent();
        SplashFactory = (e) => new Views.Splash(e);
    }

    // runs only when not restored from state
    public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
    {
        // TODO: do your long-running work here
        await NavigationService.NavigateAsync(typeof(Views.MainPage));
    }
}
````

What you are specifying here is a view that defines the look of your splash screen. An example of such a view can be found in
the [Minimal template](./Minimal-Template) which is where this sample code was copied from. The splash screen in this
template displays a `ProgressRing` below the SplashScreen.png asset but you can define Splash.xaml however you like.

Any long running tasks, or initialisation code, can then be executed in `OnStartAsync`. Note that this is **not** run if the
app is restoring from state.
