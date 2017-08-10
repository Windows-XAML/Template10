# First Steps

The first thing to do is to get Template 10 onto your computer. There are three ways that this can be done:

1. The Template 10 Template Pack from the Visual Studio Gallery.

    This can be installed using the following methods:

    - Download from https://visualstudiogallery.msdn.microsoft.com/60bb885a-44e9-4cbf-a380-270803b3f6e5

        Click on the link to go to the page then click on the Download button. Once the file has downloaded, double-click on it to install the extension into your copy of Visual Studio.

    - Inside Visual Studio, go to Tools > Extensions and Updates, click on Online in the left hand side of the window then search for Template 10.

1. Via NuGet.

    This requires you to open a solution first in Visual Studio. Then, in the NuGet console, run this command:

    `Install-Package Template10`

    This adds Template10 as a reference to the project so that you can start using the framework, but it doesn't give you the opportunity to start your project with one of the templates (Blank, Minimal and Hamburger) that are included when you install the Template Pack (above).

1. Via GitHub.

    This gives you access to the latest and greatest version of the code ... but it is in source code so you have to download the source and then build the solution.

    To download the source, go to https://github.com/Windows-XAML/Template10 and click on the green *Clone or download* button. You can then click on *Open in Desktop* (if you have the GitHub Desktop software installed) or *Open in Visual Studio* or *Download ZIP*. If you choose the latter, you will need to extract the files from the Zip file you download. The advantage of using one of the two "open" options is that the repository is cloned to your computer which means that it becomes easier to keep your local copy up to date with changes made on the GitHub server.

# Which method should I use?
Unless you are in need of the latest code (e.g. if you are after a fix for a bug in the "released" version), installing via Visual Studio is the easiest way to get started. Visual Studio will also let you know when a new release has been made available.

Building the source code does get you the latest version of the source but you then need to take it upon yourself to check when the source code has been updated although, as noted above, using one of the "open" options will make this easier.

Installing the package via NuGet is OK if you don't need the Visual Studio templates. If you do want to use any of the three templates as your starting point, you are better off installing the Visual Studio extension (which actually then installs the NuGet package).

# What next?
Once you've got Template 10 on your computer, you can then start to use the framework or, if you need a bit more help getting started, you can use one of the three supplied templates:

- [Blank Template](./Blank-Template)
- [Minimal Template](./Minimal-Template)
- [Hamburger Template](./Hamburger-Template)
