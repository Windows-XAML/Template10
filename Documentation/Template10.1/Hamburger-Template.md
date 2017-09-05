#Welcome to the Hamburger template

The Hamburger template builds on the foundation of the Minimal template by showing one way that the hamburger control can be used. You are welcome to customize your implementation.

Hit `F5` right now and run your new app! To refresh Visual Studio's intellisense, follow this: 

1. Press Ctrl+Q, type `pac man gen` 
1. Ensure `Allow NuGet to download missing packages` is checked.
1. Ensure `Automatically check for missing packages during build in Visual Studio` is checked.  
1. Right-click your Solution, and select `Clean`.
1. Right-click your Solution, and select `Rebuild`.
1. Select your project, and click the `Refresh` button at the top of Solution Explorer.

Congratulations, you can now use your project without "missing assemblies" errors. If you want, return the Package Manager Settings back to their original values. This will make your builds significantly faster.

![](https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/GetStarted.gif)

The approach taken is to create a frame consisting of a ModalDialog that then shows the [HamburgerMenu](./Controls#hamburgermenu) control alongside the current page that the user has navigated to. The frame is created with the following code in App.xaml.cs:

````csharp
if (Window.Current.Content as ModalDialog == null)
{
    // create a new frame 
    var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

    // create modal root
    Window.Current.Content = new ModalDialog
    {
        DisableBackButtonWhenModal = true,
        Content = new Views.Shell(nav),
        ModalContent = new Views.Busy(),
    };
}
````

This is executed in `OnInitializeAsync` and a more detailed explanation can be found in [Implementing a shell](./Controls#implementing-a-shell). This approach ensures that the HamburgerMenu is displayed on every page without the developer needing to add it explicitly to every page.

If you are seeing squiggly lines after creating a project with this template, please follow the [instructions to fix missing assemblies](./Fixing-Missing-Assemblies).

> Do you have technical questions you want to ask the community? Use the `Template10` tag on StackOverflow. http://stackoverflow.com/questions/tagged/template10
