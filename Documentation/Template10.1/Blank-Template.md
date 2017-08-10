#Welcome to the Blank template

The Blank template extends the Blank App (Universal Windows) template by introducing the convention of using folders for Views, ViewModels and Styles.

Hit `F5` right now and run your new app! To refresh Visual Studio's intellisense, follow this: 

1. Press Ctrl+Q, type `pac man gen` 
1. Ensure `Allow NuGet to download missing packages` is checked.
1. Ensure `Automatically check for missing packages during build in Visual Studio` is checked.  
1. Right-click your Solution, and select `Clean`.
1. Right-click your Solution, and select `Rebuild`.
1. Select your project, and click the `Refresh` button at the top of Solution Explorer.

Congratulations, you can now use your project without "missing assemblies" errors. If you want, return the Package Manager Settings back to their original values. This will make your builds significantly faster.

![](https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/GetStarted.gif)

The App.xaml file, rather than being based off the Application class, uses the Template10 [BootStrapper](./Bootstrapper) class. It also pulls in the `Custom.xaml` style file. This is empty by default but provides a central place for you to define your styles.

Since the App class is derived from the BootStrapper class, this allows the code-behind file to be much simpler than the one provided by the Blank App template.

The MainPage included in this template demonstrates a simple use of the Template10 [PageHeader](./Controls#pageheader) control.

If you are seeing squiggly lines after creating a project with this template, please follow the [instructions to fix missing assemblies](./Fixing-Missing-Assemblies).

> Do you have technical questions you want to ask the community? Use the `Template10` tag on StackOverflow. http://stackoverflow.com/questions/tagged/template10
