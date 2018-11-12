## Template 10: Getting Started

**Ready to use Template 10?**

Just open Visual Studio 2017 & search "Template 10" in the Extension Manager. After installing, go File>New>Project and choose a Template 10 project template.

**Learn more:**

1. Docs: http://aka.ms/Template10
2. Training: https://mva.microsoft.com/en-us/training-courses/getting-started-with-template-10-16336

## About Template 10.1

The current version of Template 10 is good to go. Want to learn more about it? Watch this full-day course. 
Are you looking for the code currently in Nuget?

> https://github.com/Windows-XAML/Template10/tree/version_1.1.12

## About Template 10.2

The underlying framework of Template 10 has been rebraznded to Prism.Windows and moved to https://github.com/PrismLibrary/Prism/tree/master/Source/Windows10. When you use Nuget to get Prism for your UWP app, you will be getting the latest version of Template 10. You will notice it is basically Template 10 with new namespaces. This move was a good one - it moved Tempalte 10 into the .NET Foundation. 

The goodies in Template 10 did not get moved over to Prism. They don't belong there. Instead, they are still here and moved to Nuget as helper assemblies anybody can add to their UWP apps. When you look through the Source folder in this repo, you will see several projects - the separation is only because some things are valid in some versions of UWP. It should make sense. 
