#Welcome to the Minimal template
The Minimal template builds on the [Blank template](./Blank-Template).

Hit `F5` right now and run your new app! To refresh Visual Studio's intellisense, follow this: 

1. Press Ctrl+Q, type `pac man gen` 
1. Ensure `Allow NuGet to download missing packages` is checked.
1. Ensure `Automatically check for missing packages during build in Visual Studio` is checked.  
1. Right-click your Solution, and select `Clean`.
1. Right-click your Solution, and select `Rebuild`.
1. Select your project, and click the `Refresh` button at the top of Solution Explorer.

Congratulations, you can now use your project without "missing assemblies" errors. If you want, return the Package Manager Settings back to their original values. This will make your builds significantly faster.

![](https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/GetStarted.gif)

Note in this template:

- Empty folders for Converters and Models, extending the folder convention introduced in the Blank template.
- A Services folder in turn containing a SettingsServices folder with an example of how to support settings in a UWP app.
- A Custom.xaml styles file that contains some example styles.
- More ViewModels and Views to demonstrate features described below.

The template demonstrates the following Template10 features:

- [Splash page](./Bootstrapper#splash-page)
- [SettingsService](./Services#settingsservice)
- [KeyBehavior](./Behaviors-and-Actions#keybehavior)
- Secondary commands in the [PageHeader](,/Controls#pageheader)
- [Resizer control](./Controls#resizer)
- [ModalDialog](./Controls#modaldialog)

and the following UWP features:

- [Adaptive triggers](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.adaptivetrigger.aspx)
- [XAML Behaviors](https://github.com/Microsoft/XamlBehaviors/wiki)

If you are seeing squiggly lines after creating a project with this template, please follow the [instructions to fix missing assemblies](./Fixing-Missing-Assemblies).

> Do you have technical questions you want to ask the community? Use the `Template10` tag on StackOverflow. http://stackoverflow.com/questions/tagged/template10
