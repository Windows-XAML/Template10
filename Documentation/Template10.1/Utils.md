# Table of contents

- [Introduction](#introduction)
- [ApiUtils](#apiutils)
- [AudioUtils](#audioutils)
- [ColorUtils](#colorutils)
- [DeviceUtils](#deviceutils)
- [ExpressionUtils](#expressionutils)
- [IEnumerableUtils](#ienumerableutils)
- [InkUtils](#inkutils)
- [MonitorUtils](#monitorutils)
- [Template10Utils](#template10utils)
- [TypeUtils](#typeutils)
- [UriUtils](#uriutils)
- [XamlUtils](#xamlutils)

# Introduction
Template 10 comes with a number of utilities to act as extensions or to simplify certain areas of interacting with Windows.

# ApiUtils
````csharp
// Return true if the device has hardware buttons. This is currently for phone devices as those without hardware
// buttons lose some screen estate for the soft buttons.
public static bool IsHardwareButtonsApiPresent { get; }
````

# AudioUtils
````csharp
// Return true if able to set up audio capture for speech.
public async static Task<bool> RequestMicrophonePermission()
````

# ColorUtils
This class provides a few extensions:

## ToSolidColorBrush()
This converts a `Color` to a `SolidColorBrush`. E.g.

````csharp
Colors.Green.ToSolidColorBrush()
````

## Darken()
This darkens a color by a specified percentage (from an enum defined in the class). E.g.

````csharp
Colors.White.Darken(ColorUtils.Add._20p)
````

This example returns a variant of white that is 20% darker.

## Lighten()
This performs the opposite of `Darken()`, lightening the color by the specified percentage.

## Brightness percentages
To be used in conjunction with `Darken()` and `Lighten()`, there is an enum called `Add` which
specifies percentages in steps of 10, e.g. _10p, _20p, _30p and so on, up to _90p.

# DeviceUtils
````csharp
public static DeviceUtils Current(Common.WindowWrapper windowWrapper = null)
````

`Current` returns an instance of `DeviceUtils` either for the current `WindowWrapper` or the one
specified in the call to Current().

The `DeviceUtils` class itself provides the following:

````csharp
// A set of flags to indicate the dispositions (capabilities) of the device.
public enum DeviceDispositions
// A set of flags to indicate the family that the device belongs to.
public enum DeviceFamilies

// Event handler that fires when device orientation changes, or the visible bounds of
// the application window changes.
public event EventHandler Changed;

// Which family does this device belong to?
public static DeviceFamilies DeviceFamily()
// What dispositions does this device have?
public DeviceDispositions DeviceDisposition()
// Does the device support touch?
public bool IsTouch()
// Is the device a phone? (defined as mobile device family and a screen size <= 7")
public bool IsPhone()
// Is Continuum in use? (defined as mobile device family, with touch and a screen size > 7")
public bool IsContinuum()
// Get the diagonal screen size in Inches (default) or Centimeters
public double DiagonalSize(Units units = Units.Inches)
````

Example:

````csharp
return DeviceUtils.Current().DeviceDisposition()
````

# ExpressionUtils
````csharp
// Evaluate the passed expression and return the name of the property
public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
````

Example:

````csharp
var propertyName = ExpressionUtils.GetPropertyName(propertyExpression);
````

# IEnumerableUtils
````csharp
// Convert the IEnumerable to an ObservableCollection.
// e.g.
// IEnumerable<Message> enumerableMessages;
// ObservableCollection<Message> _messages = enumerableMessages.ToObservableCollection();
public static ObservableCollection<T> ToObservableCollection<T>()

// If the specified key doesn't already exist in the dictionary, try to add it and the key.
// Returns true if successful, false if an error occurred or the key already existed.
// e.g.
// var keys = PageKeys<Pages>();
// keys.TryAdd(Pages.MainPage, typeof(Views.MainPage));
public static bool TryAdd<K, V>(K key, V value)

// Adds the items from an existing IEnumerable to the specified ObservableCollection.
// clearFirst specifies whether or not the ObservableCollection will be cleared before the items are added.
// Note: this method will fire the OnCollectionChanged event one time for each item added.
// Returns the number of items in the eventual ObservableCollection.
// e.g.
// IEnumerable<Message> enumerableMessages;
// ObservableCollection<Message> _messages = default(ObservableCollection<Models.Message>);
// _messages.AddRange(enumerableMessages);
public static int AddRange<T>(IEnumerable<T> items, bool clearFirst = false)

// Add the specified item to the IList<T> and return that item.
// e.g.
// var list = new List<DependencyObject>();
// var child = VisualTreeHelper.GetChild(parent, 0);
// list.AddAndReturn(child);
public static T AddAndReturn<T>(T item)

// For a given IEnumerable<T>, invoke the passed action on each item in the list.
// e.g.
// var priorKeys = _dictionary.Keys.ToArray();
// priorKeys.ForEach(x => RaiseMapChanged(CollectionChange.ItemRemoved, x));
public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
````

# InkUtils
A number of extensions that are used in conjunction with InkCanvas objects.

````csharp
// Save the InkCanvas to the specified file. If the folder isn't specified, the file is saved in
// ApplicationData.Current.TemporaryFolder.
// e.g.
// await inkCanvas.SaveAsync("SavedCanvas");
public async static Task SaveAsync(string fileName, StorageFolder folder = null)

// Load the specified file into the InkCanvas object. If the folder isn't specified, the file
// is looked for in ApplicationData.Current.TemporaryFolder.
// e.g.
// await inkCanvas.LoadAsync("SavedCanvas");
public async static Task LoadAsync(string fileName, StorageFolder folder = null)

// Clear the StrokeContainer in the InkCanvas.
// e.g.
// inkCanvas.Clear();
public static void Clear()

// Returns strings recognised from the strokes on the InkCanvas.
// e.g.
// string results = await inkCanvas.Recognize();
public static async Task<string> Recognize()
````

# MonitorUtils
````csharp
public static MonitorUtils Current(Common.WindowWrapper windowWrapper = null)
````

`Current` returns an instance of `MonitorUtils` either for the current `WindowWrapper` or the one
specified in the call to Current().

The `MonitorUtils` class itself provides the following:

````csharp
public InchesInfo Inches { get; }
public PixelsInfo Pixels { get; }

// Event handler that fires when device orientation changes, or the visible bounds of
// the application window changes.
public event EventHandler Changed;

// Attempt to make the current view fill the screen.
public void Maximize()
````

`InchesInfo` is a class that returns `Height`, `Width` and `Diagonal` sizes in inches.
`PixelsInfo` is a class that returns `Height`, `Width` and `Diagonal` sizes in pixels.

# Template10Utils
This class provides a number of extensions.

````csharp
// Gets the INavigationService for the frame.
// e.g. frame.GetNavigationService();
public static INavigationService GetNavigationService()

// Using the navigation service associated with the frame, perform NavigateAsync to the page.
// e.g. frame.NavigateAsyncEx(typeof(Views.MainPage), parameter);
public static async Task<bool> NavigateAsyncEx(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)

// Using the navigation service associated with the frame, perform NavigateAsync to the page associated with the specified key.
// e.g. frame.NavigateAsyncEx(Pages.MainPage);
public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible

// Get the WindowWrapper associated with the INavigationService.
// e.g. WindowWrapper ww = navService.GetWindowWrapper();
public static WindowWrapper GetWindowWrapper()

// Get the DispatcherWrapper associated with the INavigationService.
// e.g. IDispatcherWrapper dw = navService.GetDispatcherWrapper();
public static IDispatcherWrapper GetDispatcherWrapper()

// Create a DispatcherWrapper for the CoreDispatcher.
// e.g. IDispatcherWrapper dw = wrapper.GetDispatcherWrapper();
public static IDispatcherWrapper GetDispatcherWrapper() 

// Returns a list of submenu buttons with the same GroupName attribute as the command button upon which this
// extension is invoked (which is treated as Parent command button).
// If no submenu buttons found,  List is still returned with element count of 0.
// For added convenience, the GroupName attribute is detected with string.StartWith(groupName) rather than
// the straightforward string.Equals(groupName). That way we can tag submenu buttons as groupName1, groupName2, 
// groupName3, etc. With this scheme, the parent command button should be named by subset string, 
// which in this case is groupName.
// You don't have to use this scheme in which case you just stick to a single groupName for all buttons.
// e.g.
// List<HamburgerButtonInfo> items = parent.ItemsInGroup();
public static List<HamburgerButtonInfo> ItemsInGroup(bool IncludeSecondaryButtons = false)
````

# TypeUtils
Provides three arrays - `Primitives`, `NullablePrimitives` and `AllPrimitives`.

The class also provides:

````csharp
public static bool IsPrimitive(Type type)
````

# UriUtils
This class provides a number of extensions for use with Uris.

````csharp
// Returns the root of the Uri, i.e. everything before any query string in the Uri.
// e.g. Uri root = fullUri.GetRoot();
public static Uri GetRoot()

// Returns the query string from the Uri as an instance of WwwFormUrlDecoder.
// e.g. WwwFormUrlDecoder decoder = fullUri.QueryString();
public static WwwFormUrlDecoder QueryString()

// Returns the Uri without the specified query in it.
// e.g. Uri cleanUri = fullUri.RemoveQueryStringItem("filter");
public static Uri RemoveQueryStringItem(string name)

// Returns the Uri with the specified query and value added to it.
// Note: this extension does not check to see if the query is already present so
// use RemoveQueryStringItem first.
// e.g. Uri newUri = cleanUri.AppendQueryStringItem("filter", "today");
public static Uri AppendQueryStringItem(string name, string value)
````

# XamlUtils
This provides some static methods and some extensions.

````csharp
// Tries to get the named resource from Application.Current.Resources. If this fails,
// `otherwise` is returned.
public static T GetResource<T>(string resourceName, T otherwise)

// Force x:bind page bindings to update.
// e.g. XamlUtils.UpdateBindings(page);
public static void UpdateBindings(Page page)

// Initialize x:bind page bindings.
// e.g. XamlUtils.InitializeBindings(page);
public static void InitializeBindings(Page page)

public static void StopTrackingBindings(Page page)

// Returns a list of all children (and sub-children) of type T from the specified parent.
// e.g. var controls = XamlUtils.AllChildren<Control>(commandBar);
public static List<T> AllChildren<T>(DependencyObject parent) where T : DependencyObject
````

Extensions:

````csharp
// Find the first ancestor of the control calling the extension that is of type T.
// e.g. var ancestor = child.FirstAncestor<Control>();
public static T FirstAncestor<T>() where T : DependencyObject

// Find the first child of the control calling the extension that is of type T.
// e.g. var child = parent.FirstChild<Control>();
public static T FirstChild<T>() where T : DependencyObject

// Return a list of all children of the calling control, regardless of type.
// e.g. List<DependencyObject> allChildren = parent.AllChildren();
public static List<DependencyObject> AllChildren()

// Returns the `ElementTheme` that corresponds with the calling `ApplicationTheme`.
// e.g. var et = appTheme.ToElementTheme();
public static ElementTheme ToElementTheme()

// Returns the `ApplicationTheme` that corresponds with the calling `ElementTheme`.
// e.g. var appTheme = et.ToApplicationTheme();
public static ApplicationTheme ToApplicationTheme()

// For the specified `DependencyProperty`, mark it as unset.
// e.g.
// var ham = new HamburgerMenu();
// ham.SetAsNotSet(NavAreaBackgroundProperty);
public static void SetAsNotSet(DependencyProperty dp)

// For the specified `DependencyProperty`, if it is unset, set it to the given value.
// e.g.
// var ham = new HamburgerMenu();
// ham.SetIfNotSet(NavAreaBackgroundProperty, Colors.Gainsboro.Darken(ColorUtils.Add._80p).ToSolidColorBrush());
public static void SetIfNotSet(DependencyProperty dp, object value)
````
