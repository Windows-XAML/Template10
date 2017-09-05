Congratulations, you are about to upgrade to the best build of Template 10. 1.1.0+ includes legacy 10240 support and 10586 support. There are a few steps you need to follow in the upgrade. 

##Step by step

1. Update Windows 10 to the November Update (version 1511) (your project can still target 10240).
1. Install the Windows 10586 SDK to Visual Studio 2015 {any edition} Update 1/2. 
1. Use NuGet to upgrade Template 10 v1.0.* to v1.1.* in your project(s).
1. In Project/Resources delete "Behaviors SDK (XAML)" or "BehaviorsXamlSDKManaged".
1. In Project.JSON change `"Newtonsoft.Json": "7.0.1"` to `"Newtonsoft.Json": "8.0.0"`.
1. Add `Microsoft.Xaml.Behaviors.Uwp.Managed` through Nuget, if you use behaviors.
1. Select your project, and click the Refresh in the Solution Explorer (this helps).
1. (custom) Look at the breaking changes below. You may need to update your view-models.
1. Clean and rebuild.

Congratulations, you have upgraded to the best build of Template 10! 

> Please note that if you use behaviors, you need to add the behaviors SDK reference to your Project.JSON file. Yes, Blend does this for you, but you should do it manually if you aren't using Blend. 

````
"Microsoft.Xaml.Behaviors.Uwp.Managed": "1.0.3",
````

##Read me
[What's new in version 1.1.0?](https://github.com/Windows-XAML/Template10/issues?q=milestone%3A%22NuGet+Library+v1.0.9%22+is%3Aclosed)

##About version 1.1.0
It's worth pointing out that because of breaking changes, we skipped version 1.0.9. This makes existing templates that reference 1.0.* in their Project.JSON not experience negative side-effects, unless they manually upgrade by following the instructions in this document.

##Breaking changes
Read about [breaking changes in version 1.1.1](https://github.com/Windows-XAML/Template10/issues/560)

1. In Shell.xaml.cs `public Shell(NavigationService navigationService)` -> `public Shell(INavigationService navigationService)`
1. In any view-model `void OnNavigatedTo` -> `Task OnNavigatedToAsync` (you might need `return Task.CompletedTask;`)
1. In any view-model `void OnNavigatingFrom` -> `Task OnNavigatingFromAsync` (you might need `return Task.CompletedTask;`)
1. If you use it, the `INavigable` implementation for a custom ViewModelBase reflects the changes above. 

// please add other breaking changes if you find them. This is a wiki.