# Table of contents

1. [What is Template 10?](#what-is-template-10)
1. [Template 10 Documentation](#template-10-documentation)
1. [Template 10 Philosophy and Other Information](#template-10-philosophy-and-other-information)
1. [Is Template 10 right for me?](#is-template-10-right-for-me)
1. [Known Issues with Template10](#known-issues-with-template10)

# What is Template 10?

![T10](https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/T10%2056x56.png) 

> Do you have technical questions you want to ask the community? Use the `Template10` tag on StackOverflow. http://stackoverflow.com/questions/tagged/template10

Template 10 is a set of Visual Studio project templates. They sling-shot developer productivity by getting ~80% of the boilerplate stuff delivered in the template - things like navigation, suspension, and even a Hamburger control. 

Template 10 is intended for Window XAML apps written in C#.

# Template 10 Documentation 
If you want some video training about Template 10, please see the [Microsoft Virtual Academy Template 10 Training Videos](https://mva.microsoft.com/en-US/training-courses/getting-started-with-template-10-16336).

For Template 10 documentation, please refer to the sidebar ---> as this avoids the need to duplicate listing the pages
here :).

# Template 10 Philosophy and Other Information
## Credit where credit is due

Template 10 is the brainchild of Microsoft Developer Evangelism and was started there. Lots of learnings from Windows 8, including lots from the Pattern's and Practices Prism.StoreApps framework are in the code base.

## Why are you saying T10 is convention-based?

The philosophy is this: we want you do it our way unless you want to do it your way. You are the developer and only you know your app well enough to make big decisions. But, without a good reason to do it another way, do it our way. It's not about telling you to do it our way, it's about telling you to do it our way unless you don't want to. It's about guidance. It's about conventions. And, it's about flexibility.

**Here are some of our conventions:**

- We put views (XAML files) in a /Views folder (and ns)
- We only have one view-model for one view
- We put our view-models in a /ViewModels folder (and ns)
- We use OnNavigatedTo in view-models, not pages
- We put our models in a /Models folder (and ns)
- We often use the façade pattern with our models
- We navigate using a NavigationService
- We communicate with a Messenger
- We like Dependency Injection
- We use Template 10 ;-)

## What's in Template 10?

- There are [controls](./Controls)
- There are [behaviors](./Behaviors-And-Actions)
- There are [services](./Services)
- There are [converters](./Converters)
- There are [MVVM classes](./MVVM) (BindableBase, DelegateCommand, and ViewModelBase)
- There are [utility classes](./Utils)
- There are project templates ([Blank](./Blank-Template), [Minimal](./Minimal-Template), [Hamburger](./Hamburger-Template))
- There is a [NuGet package](http://nuget.org/packages/template10)
- There are [samples, many samples](https://github.com/Windows-XAML/Template10/tree/master/Samples)!

## Does Template 10 require MVVM?

No. Though it's difficult to imagine any XAML app not using model-view-view-model, there is nothing in Template 10 that requires
you to use it. Template 10 is compatible with any MVVM framework.

See the [MVVM documentation](./MVVM) for more information.

## Where have I heard about Template 10?

- On Microsoft Virtual Academy's Developer's Guide to Windows 10 
- On Microsoft official Windows 10 Hands-on-Labs 
- At the Microsoft Windows 10 Tour events
- In Microsoft Press Windows 10 exam-prep books 

## Who's contributing to Template 10?

- Jerry Nixon, co-author of Developer's Guide to Windows 10.
- Daren May, co-author of official Windows 10 hands on labs.
- Robert Evans, lead field engineer and consultant for Windows apps.
- Microsoft (PFE) premier field engineers, worldwide.
- Internal Microsoft product teams who contribute and advise.
- Internal Microsoft platform teams who contribute and advise.
- Community developers, like you, who submit pull requests all the time.

# Is Template 10 right for me?

Probably. Template 10 packs in as many time-saving lessons-learned as possible. It's perfect for new apps, or apps wanting to leverage the library. That being said, it's not for everyone (probably 90% of Windows XAML apps). 

# Known Issues with Template10
[Issues are logged here](https://github.com/Windows-XAML/Template10/issues)
