# Table of Contents

- [Page navigation basics](#page-navigation-basics)
- [Navigation cache](#navigation-cache)
- [Cache considerations](#cache-considerations)
- [Using NavigationCacheMode](#using-navigationcachemode)

# Page navigation basics
When a user navigates from one page to another in a Windows 10 app, the target view and associated view-model is, by default,
instantiated every time. This is even true when the user navigates to a view then to a second view, then returns to the first view. 

This is good because the resulting view is always fresh. However, if the data displayed by a view hasn’t changed since the user was
viewing it last, that can result in wasted time spent setting up that data. It would be great if a developer could instruct a view
to cache itself in these cases.

# Navigation cache
For this reason, the XAML `page` class has `NavigationCacheMode`. By default it is `Disabled`. This means every time the user navigates
to the page, the page is instantiated. But when it’s set to `Enabled` or `Required` (both of which can only be set inside the page
constructor) the page instantiates only on the first visit.

> MSDN Docs: https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.page.navigationcachemode.aspx

Caching is difficult on machines that have limited memory. Because every cached page equals some amount of memory consumption, it
increases the opportunity for your app to be automatically terminated by the operating system. But an app’s memory threshold is
high and such a case is seldom. 

> To help developers more, the XAML frame has a `CacheSize` property. This is the limit to the number of pages that can be
cached. When cache mode is `Enabled`, this is obeyed meaning that if that limit is set and the user continues to navigate to new
pages, the older pages are removed from the cache. This, in turn, means that if the user navigates back to one of those pages, the
page will be instantiated as if for the first time. However, the third possible value to `NavagationCacheMode` is `Required`, which
behaves just like `Enabled` except it ignores `CacheSize`. In other words, those pages are always kept in the cache.

# Cache considerations
1. If some view causes the data displayed in another view to change, the data should keep itself up-to-date if you are sharing
models in memory. If, however, you are using copies of models in memory, the first view-model will need to signal the second
view-model through some type of messaging solution. 

> Messaging is a pattern for view-models to communicate without requiring a reference to one another. It’s powerful, common, and
gets the job done. Template 10 does not ship with a messaging solution, but we like the PubSubEvents created by Microsoft in NuGet,
and the Messenger class in MVVM Light. Again, there are many out there and they are all effective.

2. If some view causes the data displayed in another view to be deleted, the developer cannot allow the user to navigate forward
(or back) and view the data that is dead. In this case, the developer would need to clear the BackStack or ForwardStack of the
container XAML frame. In Template 10, this is done with `NavigationService.Frame.BackStack.Clear();`. Some of the visual controls
on your view may need to be manually updated to reflect the change in navigation.

3. You should assume your view is new every time. In other words, don't rely on caching. There could be circumstances that could
cause your view or view-model to be re-instantiated on navigation (e.g. if you have set `CacheSize` and the
page has dropped out of the cache). No matter how you use cache, you must write your code defensively so that if caching should
not work or be overloaded in some way, your app continues to deliver an excellent user experience.

# Using NavigationCacheMode
There are two parts to using `NavigationCacheMode` in a Template 10 app:

- Setting `NavigationCacheMode` to the appropriate value.
- Determining, when the page is navigated to, whether or not this is the first time or if the page is cached.

## Setting NavigationCacheMode
Remember you only need to set this in the constructor of those pages that enable or require the NavigationCache. Here's
how you set the cache mode value:

```c#
public DetailPage()
{
    InitializeComponent();
    NavigationCacheMode = NavigationCacheMode.Enabled;
}
```

As noted above, `Enabled` will obey the setting of `CacheSize`. If you have some views that must be kept in the cache, use
`Required` instead.

> Examples of the cache mode being set can be found in the Minmal template where the Settings page, for example, sets it to
`Required` while the Detail page sets it to `Disabled`.

## Handing page navigation
Ideally, a view-model in a Template 10 app derives from ViewModelBase. This allows the developer to override the `OnNavigatedToAsync`
method, which is called whenever the user navigates to a view.

One of the parameters of `OnNavigatedToAsync` is `NavigationMode`. When the user is navigating forward or back along the navigation
stack, this argument indicates how the user has navigated to the page. On the first visit to the page, it will have a value of
`New` while forward and back navigation will have the corresponding `Forward` or `Back` enum value.

If you have enabled the cache, you can use this value to **suggest** whether or not you need to reload any data. It can, however,
only be a suggestion because `Forward` and `Back` are the navigation mode values and not an indication of whether or not the page
was still in the cache. It would, therefore, be good practice to use this in conjunction with an *internal* value for the page
that could be tested to see if the page had already been initialised.

```C#
private bool viewModelInitialised; // defaults to false

public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
{
    if (mode == NavigationMode.New || mode == NavigationMode.Refresh || viewModelInitialised == false)
    {
        // Either the view model isn't in the cache or we're being told to refresh
        // so initialise as normal.
        ...

        // Show that we've now initialised the VM. That way, if we navigate forward or
        // back, we can depend on this to tell us whether or not we've come from the
        // cache.
        viewModelInitialised = true;
    }
}
```
