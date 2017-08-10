# Table of Contents

- [CustomTitleBar](#customtitlebar)
- [HamburgerButtonInfo](#hamburgerbuttoninfo)
- [HamburgerButtonTemplateSelector](#hamburgerbuttontemplateselector)
- [HamburgerMenu](#hamburgermenu)
- [ModalDialog](#modaldialog)
- [PageHeader](#pageheader)
- [Resizer](#resizer)
- [RingSegment](#ringsegment)

# CustomTitleBar
The control to provide the means to customize the application `TitleBar`.

- [Inspiration](#customtitlebar-inspiration)
- [Key features](#customtitlebar-key-features)
- [Properties](#customtitlebar-properties)
- [Syntax](#customtitlebar-syntax)

## <a name="customtitlebar-inspiration"></a>Inspiration
Every app has a `TitleBar` and most designers enjoy branding as much of the application visuals as possible. The out of the box customization experience requires a fair degree of effort to achieve this, so the `CustomTitleBar` control endeavors to simplify that.

## <a name="customtitlebar-key-features"></a>Key features
- Easy way to specify if the application extends rendering into the `TitleBar` area.
- Easy look & feel style customization

## <a name="customtitlebar-properties"></a>Properties
| Name | Type | Notes |
|:---|:---|:---|
| BackgroundColor| Brush| The default background color of the `TitleBar`. |
| ButtonBackgroundColor| Brush| The default background color of all buttons on the `TitleBar`. |
| ButtonForegroundColor| Brush| The default color for the foreground of all buttons on the `TitleBar`. |
| ButtonHoverBackgroundColor| Brush| The background color used when the mouse cursor hovers over a button on the `TitleBar`. |
| ButtonHoverForegroundColor| Brush| The foreground color used when the mouse cursor hovers over a button on the `TitleBar`. |
| ButtonInactiveBackgroundColor| Brush| The background color of all buttons on the `TitleBar` when the application is inactive. |
| ButtonInactiveForegroundColor| Brush| The foreground color of all buttons on the `TitleBar` when the application is inactive. |
| ButtonPressedBackgroundColor| Brush| The background color used when a button on the `TitleBar` is in a pressed state. |
| ButtonPressedForegroundColor| Brush| The foreground color used when a button on the `TitleBar` is in a pressed state. |
| Extended| bool| true if the client area of the application is extended into the `TitleBar` area; otherwise false. This setting allows the developer to take control of rendering into the `TitleBar`.|
| ForegroundColor| Brush| The default background color of the `TitleBar` - i.e. the title text color. |
| InactiveBackgroundColor| Brush| The background color of the `TitleBar` when the application is inactive. |
| InactiveForegroundColor| Brush| The foreground color of the `TitleBar` when the application is inactive. |

## <a name="customtitlebar-syntax"></a> Syntax
Before you can use the control, you should specify your desired colors in App.xaml:

````XAML
<Application.Resources>
    <Style TargetType="controls:CustomTitleBar">
        <Setter Property="BackgroundColor" Value="SteelBlue" />
        <Setter Property="ButtonBackgroundColor" Value="Maroon" />
        <Setter Property="ButtonForegroundColor" Value="White" />
        <Setter Property="ButtonHoverBackgroundColor" Value="Orange" />
        <Setter Property="ButtonHoverForegroundColor" Value="White" />
        <Setter Property="ButtonInactiveBackgroundColor" Value="DimGray" />
        <Setter Property="ButtonInactiveForegroundColor" Value="White" />
        <Setter Property="ButtonPressedBackgroundColor" Value="Green" />
        <Setter Property="ButtonPressedForegroundColor" Value="White" />
        <Setter Property="Extended" Value="False" />
        <Setter Property="ForegroundColor" Value="White" />
        <Setter Property="InactiveBackgroundColor" Value="LightSteelBlue" />
        <Setter Property="InactiveForegroundColor" Value="DimGray" />
    </Style>
</Application.Resources>
````

If you are using the Template10 BootStrapper to start your application (and why wouldn't you...?), the presence of the style in App.xaml will be detected and your color scheme applied to the `TitleBar` for you. If, however, you are striking out alone, the following code in your start up class will achieve the same effect:

````CSHARP
// setup custom titlebar
foreach (var resource in Application.Current.Resources
    .Where(x => x.Key.Equals(typeof(Controls.CustomTitleBar))))
{
    var control = new Controls.CustomTitleBar();
    control.Style = resource.Value as Style;
}
````

# HamburgerButtonInfo
This control is the base control to display a button in the `HamburgerMenu`. This exists to make it easier
to quickly recreate buttons like the ones you would find in some of the Microsoft-supplied apps.

`HamburgerButtonInfo` derives from the `RadioButton` control, which is the best choice for this scenario since:

- It already handles the fact that the current selected item should be highlighted.
- It already supports mutual selection: if one of the buttons in the group is selected, all the other are automatically unselected.

Here is how a sample HamburgerMenu implementation looks like:

```XAML
<controls:HamburgerMenu x:Name="Menu"
                HamburgerBackground="#FFD13438"
                HamburgerForeground="White"
                NavAreaBackground="#FF2B2B2B"
                NavButtonBackground="#FFD13438"
                SecondarySeparator="White"
                NavButtonForeground="White" >

    <controls:HamburgerMenu.PrimaryButtons>
        <controls:HamburgerButtonInfo PageType="views:MainPage" ClearHistory="True">
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <SymbolIcon Symbol="Home" Width="48" Height="48" />
                <TextBlock Text="Home" Margin="12, 0, 0, 0" VerticalAlignment="Center" />
            </StackPanel>
        </controls:HamburgerButtonInfo>

        <controls:HamburgerButtonInfo PageType="views:DetailPage" >
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <SymbolIcon Symbol="Calendar" Width="48" Height="48" />
                <TextBlock Text="Calendar" Margin="12, 0, 0, 0" VerticalAlignment="Center" />
            </StackPanel>
        </controls:HamburgerButtonInfo>
    </controls:HamburgerMenu.PrimaryButtons>

    <controls:HamburgerMenu.SecondaryButtons>
        <controls:HamburgerButtonInfo PageType="views:DetailPage">
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <SymbolIcon Symbol="Setting"  Width="48" Height="48"  />
                <TextBlock Text="Settings" Margin="12, 0, 0, 0" VerticalAlignment="Center"/>
            </StackPanel>
        </controls:HamburgerButtonInfo>
    </controls:HamburgerMenu.SecondaryButtons>

</controls:HamburgerMenu>
```

The HamburgerButtonInfo control has the following properties:

- `PageType` is a reference to the page type connected to the section. When the user taps on this button, he will be automatically redirected to that page.
- `PageParameter` is an optional parameter which can be passed to the destination page and retrieved using the `OnNavigatedTo()` event handler.
- `ClearHistory` is a boolean. When it's set to true, the navigation to the selected page will also clear the back stack. This is typically applied to the button connected to the main page of the application, to avoid circular navigations.

The look & feel of the button is up to the developer, who can use an arbitrary XAML to define its layout. The sample shows a standard implementation using a symbol (with the `SymbolIcon` control) and a label (with a `TextBlock` control). This implementation makes easier to recreate the look & feel of many native application: when the panel is closed, only the symbol will be displayed; when the panel is opened, both the symbol and the label will be displayed.
 
![](http://i.imgur.com/xYkujJ3.png)

# HamburgerButtonTemplateSelector
This is a template selector rather than a control but it is intended to allow different templates to be specified for the different
button types that can be assigned in the HamburgerButtonInfo control. As such, this is here for completeness.

Here is the implementation from HamburgerMenu.xaml:

````XAML
<ItemsControl.ItemTemplateSelector>
    <local:HamburgerButtonTemplateSelector CommandTemplate="{StaticResource NavCommandButtonTemplate}"
                                            LiteralTemplate="{StaticResource NavButtonLiteralTemplate}"
                                            ToggleTemplate="{StaticResource NavToggleButtonTemplate}" />
</ItemsControl.ItemTemplateSelector>
````

# HamburgerMenu 
The control to create a hamburger menu based navigation pattern in your application.

![](http://i.imgur.com/YXAtzYy.png)

- [Inspiration](#hamburgermenu-inspiration)
- [SplitView](#splitview)
- [Key features](#hamburgermenu-key-features)
- [Properties](#hamburgermenu-properties)
- [Syntax](#hamburgermenu-syntax)
- [Customization](#hamburgermenu-customization)
- [Buttons](#buttons)
- [Implementing a shell](#implementing-a-shell)
- [PageHeader and the HamburgerMenu](#hamburgermenu-and-the-pageheader)
- [Visual States for the HamburgerMenu](#visual-states-for-the-hamburgermenu)
- [Controlling the Visual States](#hamburgermenu-controlling-the-visual-states)

## <a name="hamburgermenu-inspiration"></a>Inspiration
The Hamburger Menu approach is one of the most widely used navigation pattern nowadays in mobile applications and
websites. It is made by using a panel, which is usually hidden, that contains the links to browse through the different
sections of the applications. By tapping a button, the user is able to show or hide the panel, so that he can quickly
jump from one section to another. The name of this navigation pattern comes from the icon that is used to show / hide
the panel: three lines, one on top of the other, which resemble a hamburger placed in the middle of two pieces of bread.

Hamburger Menu is one of the many navigation patterns available in Windows 10 and it's effective when your application has multiple and separate sections.

## SplitView
The Universal Windows Platform has added a new control called `SplitView` to implement hamburger menu navigation's patterns. The goal of the SplitView control is to leave the maximum freedom to the developer, since it simply takes care of splitting the page in two sections:

- A panel, which can be placed in any margin of the page and that can behave in multiple ways (always visible, manually activated by the user, etc.)
- The main content of the page: typically, when the user selects one of the items in the panel, the main section is reloaded to display the selected page.

The downside of this freedom is that, if you want to implement a "standard hamburger menu experience" (like the one offered by the MSN News app), you'll have a lot of work to do.

The HamburgerMenu control helps you to quickly implement this kind of experience.

## <a name="hamburgermenu-key-features"></a>Key features
- Support for Primary buttons / commands (displayed at the top of the panel; they provide access to the most frequently used sections of the application)
- Support for Secondary buttons / commands (displayed at the bottom of the panel with, optionally, a separator; they provide access to the least frequently used sections of the application)
- Built-in styles to quickly create the navigation buttons for the different sections of the application
- Easy look & feel style customization
- Works best with the PageHeader control

## <a name="hamburgermenu-properties"></a>Properties
| Name | Type | Notes |
|:---|:---|:---|
| HamburgerBackground| SolidColorBrush| This property controls the background color of the button used to show / hide the panel of the hamburger menu. |
| HamburgerForeground| SolidColorBrush| This property controls the foreground color of the button used to show / hide the panel of the hamburger menu. |
| NavAreaBackground| SolidColorBrush| This property controls the background color of the panel. |
| NavButtonBackground| SolidColorBrush| This property controls the background color of the button connected to the currently selected section. |
| NavButtonForeground| SolidColorBrush| This property controls the foreground color of the button connected to the currently selected section. |
| SecondarySeparator| SolidColorBrush| This property controls the color of the separator line which is added before the secondary commands. |
| NavigationService| NavigationService| This property holds a reference to the [`NavigationService`](./Services#navigationservice) instance provided by Template10. It's required to handle the navigation between the different sections of the application. |
| PrimaryButtons| IEnumerable| This property allows developers to add in the panel the main sections of the application, which are displayed at the top. Each section is represented with a `HamburgerButtonInfo` control, which offers some built in features like predefined styles, automaticatic navigation, etc. |
| SecondaryButtons| IEnumerable| This property works in the same way of the PrimaryButtons one, but it's used to add to the panel the secondary sections of the applications, which are displayed on the bottom. |
| IsOpen| bool| This property controls the visibility of the panel. |
| IsFullScreen| bool| This property controls the visibility of the entire splitview pane. |
| PaneWidth| double| This property controls the size of the panel. |
| Selected| HamburgerButtonInfo| This property contains a reference to the selected section in the panel. |
| VisualStateNarrowMinWidth| integer| This property indicates the width when the Narrow Visual State will be applied. This Visual State does only one thing, it completely hides the panel when the window is too small (like on a smarpthone). |
| VisualStateNormalMinWidth| integer| This property indicates the width when the Normal Visual State will be applied. In this state, the SplitView control is displayed in Minimal mode. |

## <a name="hamburgermenu-syntax"></a>Syntax
Before you can add the control, you must add the Template10.Controls namespace:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

</Page>
```

With the namespace in place, add the HamburgerMenu control to your page:

```XAML
<controls:HamburgerMenu x:Name="Menu" />
```

To properly support navigation, you need to assign a value to the NavigationService property of the control, like in the following sample:

```C#
public sealed partial class Shell : Page
{
    public Shell(NavigationService navigationService)
    {
        this.InitializeComponent();
        Menu.NavigationService = navigationService;
    }
}
```

You'll understand better how to pass a reference to the NavigationService object to a page in the next sections, when we'll discuss how to embed the HamburgerMenu into a shell.

## <a name="hamburgermenu-customization"></a>Customization
The easiest customization properties of the HamburgerMenu control are:

- `HamburgerBackground` to define the background color of the button used to show / hide the panel.
- `HamburgerForeground` to define the foreground color of the button used to show / hide the panel.
- `NavAreaBackground` to define the background color of the panel.
- `NavButtonBackground` to define the background color of the highlighted section in the panel.
- `NavButtonForeground` to define the foreground color of the highlighted section.
- `SecondarySeparator` to define the color of the separator which is added before the secondary buttons.

For example, the following code:

```XAML
<controls:HamburgerMenu x:Name="Menu"
                    HamburgerBackground="#FFD13438"
                    HamburgerForeground="White"
                    NavAreaBackground="#FF2B2B2B"
                    NavButtonBackground="#FFD13438"
                    SecondarySeparator="White"
                    NavButtonForeground="White" />
```

will create the following result:

![](http://i.imgur.com/tFrrdA7.png)

## Buttons
The HamburgerMenu control offers an easy way to define the sections of your applications, by providing two categories of buttons:

- PrimaryButtons are displayed at the top of the panel and they are used to provide to the user quick access to the most used sections of the application.
- SecondaryButtons are displayed at the bottom of the panel and, as such, they are used to provide to the user access to the less frequently used sections of the applications (like Settings).

The original SplitView control allows developers to add, into the panel, arbitrary XAML without any constraint. However, it doesn't provide
built-in controls to quickly recreate the buttons you can find, for example, in the MSN News app. To solve that, Template 10 provides the
[`HamburgerButtonInfo` control](#hamburgerbuttoninfo).

## Implementing a shell
The HamburgerMenu is a XAML control and, as such, can be placed in any page of the application. However, since it's used to provide a global navigation pattern, it's unlikely that it will be placed in just one page of the application. To provide a seamless experience, you would need to place the same control in every page of your application. However, this approach would have many downsides: redundancy, harder to mantain, etc.

A better approach is to place the HamburgerMenu control into a **shell**, which is a page that will be used as a container in replacement of the standard Frame of the application. The standard behavior in a Universal Windows app is to set, as content of the main Window, an empty frame, which will host all the pages of the application and provide navigation capabilities between them.
With this new approach, we're going to use a new page, which will contain the HamburgerMenu control, as a content of the main window. All the other pages of the application will be hosted by this new container.

The first step is to add a new XAML page in your project, which will contain just the definition of the HamburgerMenu control, like in the following sample:

```XAML
<Page
    x:Class="HamburgerSample.Views.Shell"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:views="using:HamburgerSample.Views"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:controls="using:Template10.Controls"
    mc:Ignorable="d">

    <controls:HamburgerMenu x:Name="Menu">

        <controls:HamburgerMenu.PrimaryButtons>
            <controls:HamburgerButtonInfo PageType="views:MainPage" ClearHistory="True">
                <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                    <SymbolIcon Symbol="Home" Width="48" Height="48" />
                    <TextBlock Text="Home" Margin="12, 0, 0, 0" VerticalAlignment="Center" />
                </StackPanel>
            </controls:HamburgerButtonInfo>

            <controls:HamburgerButtonInfo PageType="views:DetailPage" >
                <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                    <SymbolIcon Symbol="Calendar" Width="48" Height="48" />
                    <TextBlock Text="Calendar" Margin="12, 0, 0, 0" VerticalAlignment="Center" />
                </StackPanel>
            </controls:HamburgerButtonInfo>
        </controls:HamburgerMenu.PrimaryButtons>

        <controls:HamburgerMenu.SecondaryButtons>
            <controls:HamburgerButtonInfo PageType="views:DetailPage">
                <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                    <SymbolIcon Symbol="Setting"  Width="48" Height="48"  />
                    <TextBlock Text="Settings" Margin="12, 0, 0, 0" VerticalAlignment="Center"/>
                </StackPanel>
            </controls:HamburgerButtonInfo>
        </controls:HamburgerMenu.SecondaryButtons>

    </controls:HamburgerMenu>
</Page>
```

The only operation to do in code-behind is, as we've previously seen, to assign the NavigationService property of the HamburgerMenu control:

```C#
public sealed partial class Shell : Page
{
    public Shell(NavigationService navigationService)
    {
        this.InitializeComponent();
        Menu.NavigationService = navigationService;
    }
}
```
Now we need to:

1. Define this new page as main content of the application's window, in replacement of the standard Frame
2. Pass a reference to the NavigationService to the page, so that it can be assigned to the HamburgerMenu control

Both objectives can be achieved in the bootstrapper class, which provides a method called `OnInitializeAsync()` that is invoked every time the app is initialized, right before triggering the navigation to main page. Here is how we can override this method for our purpose:

```C#
sealed partial class App : BootStrapper
{
    public App()
    {
        this.InitializeComponent();
    }

    public override Task OnInitializeAsync(IActivatedEventArgs args)
    {
        var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
        Window.Current.Content = new Views.Shell(nav);
        return Task.FromResult<object>(null);
    }

    public override Task OnStartAsync(BootStrapper.StartKind startKind, IActivatedEventArgs args)
    {
        NavigationService.Navigate(typeof(Views.MainPage));
        return Task.FromResult<object>(null);
    }
}
```

`Window.Current.Content` is the property that holds a reference to the main frame of the application. By default, it simply contains a new instance of the Frame class. For our scenario, we need to override this behavior and to assign to the property a new instance of the page we've created to act as a shell.

To get a reference to the NavigationService we need for the HamburgerMenu control we use a method, provided by the BootStrapper class, called `NavigationServiceFactory`: we pass the returned object to the constructor of the shell page.

After these changes, we'll notice that the application, at startup, will be indeed redirected to the main page (as per the navigation triggered in the `OnStartAsync()` method), but the HamburgerMenu control defined in the shell page will be visible too.

## <a name="hamburgermenu-and-the-pageheader"></a>PageHeader and the HamburgerMenu
The HamburgerMenu control is a great companion of the PageHeader one, also provided by Template10. You can learn more about how to use them together in the [PageHeader documentation](#pageheader).

## Visual States for the HamburgerMenu
The HamburgerMenu control has two built-in visual states - VisualStateNormal and VisualStateNarrow. 

1. In VisualStateNormal, the HamburgerMenu applies the Minimal layout to the SplitView control. It means that the full panel is closed, but it's always visible in minimal mode, which means that only the icons of the buttons are visible.
2. In VisualStateNarrow, the HamburgerMenu is completely hidden, except for the Hamburger button that shows / hides the panel.

## <a name="hamburgermenu-controlling-the-visual-states"></a>Controlling the Visual States
You can control the HamburgerMenu's visual states by defining the minimum widths that triggers them. The VisualStateNarrowMinWidth and VisualStateNormalMinWidth properties accomplish this.

You can apply these values like this:

```XAML
<Page
    x:Class="HamburgerSample.Views.Shell"
    xmlns:controls="using:Template10.Controls">

    <controls:HamburgerMenu x:Name="Menu"
               VisualStateNarrowMinWidth="0"
               VisualStateNormalMinWidth="800" />

</Page>
```

In the case above, when the width of the window is greater than 0 effective pixels but less than 800, the VisualStateNarrow visual state will be triggered. In this scenario, the panel will be hidden and only the hamburger button will be visible. Concurrently, when the width of the window is equal to or greater than 800 effective pixels, the VisualStateNormal visual state will be triggered. In this case, the panel will be visible in minimal mode.

![](http://i.imgur.com/uRfQur0.gif)

# ModalDialog 
The control to provide the means to display a Modal dialog in your application.

- [Inspiration](#modaldialog-inspiration)
- [Key features](#modaldialog-key-features)
- [Properties](#modaldialog-properties)
- [Syntax](#modaldialog-syntax)

## <a name="modaldialog-inspiration"></a>Inspiration
Almost every app has the need to display some form of a Modal Dialog - a UI element that blocks interaction with the rest of an application until either the user has dismissed it or some application task has completed.

## <a name="modaldialog-key-features"></a>Key features
- Easy way to display content on top of the rest of the application UI and an opacity layer that blocks access to UI elements displayed "beneath" the `ModalContent`.
- Easy look & feel style customization

## <a name="modaldialog-properties"><a/>Properties
| Name | Type | Notes |
|:---|:---|:---|
| IsModal| bool <br/> default: false| true if the modal content is being displayed; otherwise false.|
| CanBackButtonDismiss| bool <br/> default: false| true if the modal dialog can be closed by hitting a back button; otherwise false. **note** if true, the modal is closed and the back navigation request is marked as handled, therefore no page navigation will occur. |
| Content| UIElement | Optional content that will be overlaid by the modal content when the dialog is open. |
| DisableBackButtonWhenModal| bool <br\> default: false| true if back navigation is not allowed if the modal is open; otherwise false; **note** For true modality, this should be set to true. |
| ModalBackground| Brush | The background color for the dialog. |
| ModalContent| UIElement | The content to be displayed in the modal dialog when open. |

## <a name="modaldialog-syntax"></a>Syntax
````XAML
<Controls:ModalDialog x:Name="BusyModal" CanBackButtonDismiss="False" DisableBackButtonWhenModal="True">
    <Controls:ModalDialog.ModalContent>
        <Viewbox Height="32" VerticalAlignment="Center" HorizontalAlignment="Center">
            <StackPanel Orientation="Horizontal">
                <ProgressRing x:Name="BusyRing" Width="24" Height="24" Margin="12,0"
                            Foreground="White" IsActive="{Binding IsModal, ElementName=BusyModal}" />
                <TextBlock x:Name="BusyText" VerticalAlignment="Center" Foreground="White" />
            </StackPanel>
        </Viewbox>
    </Controls:ModalDialog.ModalContent>
</Controls:ModalDialog>

````

# PageHeader
The control to create a common page element including header text, primary buttons, and secondary buttons. 

![](http://i.imgur.com/BFG3pSB.png)

- [Inspiration](#pageheader-inspiration)
- [CommandBar](#commandbar)
- [Key features](#pageheader-key-features)
- [Properties](#properties)
- [Syntax](#pageheader-syntax)
- [Customization](#pageheader-customization)
- [Navigation](#navigation)
- [Built-in behavior](#built-in-behavior)
- [Overriding built-in behavior](#overriding-built-in-behavior)
- [Commands](#commands)
- [Phone versus Desktop](#phone-versus-desktop)
- [PageHeader and the HamburgerMenu](#pageheader-and-the-hamburgermenu)
- [Visual States for the Hamburger Button](#visual-states-for-the-hamburger-button)
- [Controlling the Visual States](#pageheader-controlling-the-visual-states)
- [Disable the Visual States](#disable-the-visual-states)

## <a name="pageheader-inspiration"></a>Inspiration
Every page needs some kind of header/title. It's the boilerplate code developers write over and over. The Template
10 PageHeader does this. It is inspired by the design of Microsoft's MSN News app. The Template 10 PageHeader control
is 90% representative of that implementation, but not identical. You'll like it.

## CommandBar
In Universal Windows Platform, the CommandBar control can be placed anywhere - not just top and bottom like in Windows 8.x.
PageHeader is a UI control that extends the CommandBar. The PageHeader allows developers to create a uniform page UI with
primary and secondary buttons presented in a consistent, easy way.

## <a name="pageheader-key-features"></a>Key features
- Support for Primary buttons/commands (always visible)
- Support for Secondary buttons/commands (hidden until the ellipse is tapped)
- Standard on-canvas back button, wired to navigate
- Standard on-canvas forward button, wired to navigate
- Easy look & feel style customization

## <a name="pageheader-properties"></a>Properties
| Name | Type | Notes |
|:---|:---|:---|
| BackButtonVisiblity| Visibility (default: Collapsed)| This property controls the visibility of the on-canvas back button. The on-canvas back button requires the PageHeader.Frame property to be set in order to function properly.|
| Content| object| This property allows for advanced scenarios for developers. With the Text property, the developer can set a simple string header. Content allows for any XAML control for a custom experience.|
| Frame| Frame| This property sets the context for the on-canvas back button. This property not only enables the back button but allows it to operate against any off-canvas frame, if desired.|
| Background| Brush| This property overrides the default background of the PageHeader. This property can be applied through a style to allow developers the ability to theme their application.|
| Foreground| Brush| This property overrides the default foreground (text) of the PageHeader. This property can be applied through a style to allow developers the ability to theme their application.|
| PrimaryCommands| IEnumerable<ICommandBarElement>| This property allows developers to add any `ICommandBarElement` to the always-visible collection of buttons in the header. This, in effect, is the PrimaryCommands property of the CommandBar.|
| SecondaryCommands| IEnumerable<ICommandBarElement>| This property allows developers to add any `ICommandBarElement` to the collection of buttons only visible when the user clicks the ellipses button. This, in effect, is the SecondaryCommands property of the CommandBar.|
| Text| string| This simple property sets the header text for the control. The text is displayed on the left side of the header and does not wrap. The color of the font is controlled by the HeaderForegroundBrush property.|
| VisualStateNarrowMinWidth| integer| This property indicates the width when the Narrow Visual State will be applied. This Visual State does only one thing, it increases the left margin of the Text by 48 pixels to accommodate a hamburger button.|
| VisualStateNormalMinWidth| integer| This property indicates the width when the Normal Visual State will be applied. This Visual State has no visual impact on the PageHeader other than removing the effects of the VisualStateNarrowMinWidth property.|
| EllipsisVisibility| Visibility (default: Auto)| By default is set to Auto.  If either Primary or Secondary has children Ellipsis will be shown. This can be force hidden by setting to Collapsed.

## <a name="pageheader-syntax"></a>Syntax
Before you can add the control, you must add the Template10.Controls namespace:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

</Page>
```

With the namespace in place, add the PageHeader control to your page:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Frame="{x:Bind Frame}" 
        BackButtonVisibility="Visible"
        Text="Detail" />

</Page>
```

> Remember that setting the Frame property on the PageHeader is important if you intend to use the Back navigation button functionality built-in to the PageHeader control.

Your initial UI will look like this:

![](http://i.imgur.com/BFG3pSB.png)

## <a name="pageheader-customization"></a>Customization
The easiest customization properties of the PageHeader control are:

- **Text** to define the text displayed in the header (typically, the title of the page)
- **HeaderForeground** to define the text color of the header
- **HeaderBackground** to define the background color of the header

For example:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Frame="{x:Bind Frame}" 
        Text="Main Page" 
        HeaderBackground="Orange" 
        HeaderForeground="Red" />

</Page>
```
Your custom UI will look like this:

![](http://i.imgur.com/xvwCFXf.png)

## Navigation
Built-in navigation is a handy PageHeader behavior. The BackButtonVisibility property lets the developer turn this
functionality on. Then, the back button has its own logic to hide itself (discussed below). 

Specifically:

1. The developer is responsible to connect the PageHeader control to the XAML Frame using the `PageHeader.Frame` property.
Without it, the PageHeader does not know the context of the navigation stack. 

1. The Back button will only display if there are pages in the BackStack. For example, the user will never see the back
button on the main page of the application, since the back stack is empty.

1. The developer may choose to let the operating system draw a back button in the chrome of the application. This can be
set using the `Bootstrapper.ShowShellBackButton` property. When the shell-drawn back button is visible, the PageHeader's
on-canvas back button will not be visible. 

- When DeviceFamily=Desktop + Mode=Mouse, the shell-drawn back button is in the application's title bar. 
- When DeviceFamily=Desktop + Mode=Touch, the shell-drawn back button is in the task bar.
- When DeviceFamily=Mobile, the shell-drawn back button is in the navigation bar.

Note: When (DeviceFamily=Desktop + Mode=Touch) or (DeviceFamily=Mobile) setting the `PageHeader.BackButtonVisibility`
property has no effect. The shell-drawn back button is always visible.

## Built-in behavior
Clicking the on-canvas or the shell-drawn back button is automatically handled by Template 10. It will `Frame.GoBack`
automatically. If there is nowhere to go back, the on-canvas button will no longer be visible. In this state, the
shell-drawn back button will be visible; it does not auto-hide. Clicking the shell-drawn back button in this state
will have variable behavior.

- When DeviceFamily=Desktop + Mode=Mouse, clicking the back button will do nothing. 
- When DeviceFamily=Desktop + Mode=Touch, clicking the back button will show the start menu.
- When DeviceFamily=Mobile, clicking the back button will show the next open app.

## Overriding built-in behavior
The developer might want to handle BackRequested behavior manually. To do override the native behavior, handle
the `Bootstrapper.BackRequested` event and (optionally) set `args.Handled` to true. 

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Frame="{x:Bind Frame}" 
        BackButtonVisibility="Visible"
        Text="Detail" />

</Page>
```

The on-canvas back button would look like this:

![](http://i.imgur.com/rKLWSCm.png)

## Commands
The PageHeader control extends the standard XAML CommandBar control.

PageHeader offers two categories of commands:

1. PrimaryCommands are always visible.
2. SecondaryCommands are hidden by default.
 
The most common control is `AppBarButton`. This represents a button with a label and an icon. You manage interaction by either handling the button's Click event or bind to the Command property for apps using the MVVM design pattern. 

The following sample code shows how to define a set of commands:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Text="Main Page" Frame="{x:Bind Frame}">
        <controls:PageHeader.PrimaryCommands>
            <AppBarButton Icon="Save" Label="Save" Click="Save_Clicked" />
            <AppBarButton Icon="Undo" Label="Undo" Click="Undo_Clicked" />
        </controls:PageHeader.PrimaryCommands>
        <controls:PageHeader.SecondaryCommands>
            <AppBarButton Label="Option 1" Click="Op1_Clicked" />
            <AppBarButton Label="Option 2" Click="Op2_Clicked" />
            <AppBarButton Label="Option 3" Click="Op3_Clicked" />
        </controls:PageHeader.SecondaryCommands>
    </controls:PageHeader>

</Page>
```

The resulting PageHeader with buttons looks like this:

![](http://i.imgur.com/NYQTfCg.png)

## Phone versus Desktop
From a UI point of view, the size of the PageHeader control can be very different when DeviceFamily=Desktop versus DeviceFamily=Mobile. As such, on mobile you should privilege the secondary commands and add as primary commands no more than four buttons; otherwise, when space is limited, primary buttons are not visible in your UI. In addition, when you are designing your UI, you must remember the space taken by the text/title of your page. In such cases it might make sense to have even fewer primary buttons. 

> The developer can manipulate primary and secondary buttons either through view-model binding or via code-behind at any time during runtime to account for size or device family.

Here's a comparison to consider:

![](http://i.imgur.com/3KUiKFs.png)

## PageHeader and the HamburgerMenu
The PageHeader control is the perfect companion for the HamburgerMenu control (also part of Template 10). 

> In this case, a developer would typically choose to set the `PageHeader.HeaderBackground` property to match
the `HamburgerMenu.HamburgerBackground` property. 

## Visual States for the Hamburger Button
The PageHeader control has two built-in visual states - VisualStateNarrow and VisualStateNormal. These have been specifically created to support the HamburgerMenu control which has the identical visual states. 

1. The PageHeader's VisualStateNarrow affects the UI in only one way, it shifts the Text of the control 48 pixels to the right. This provides the on-screen real estate required to display the stand alone hamburger button. 

2. The PageHeader's VisualStateNormal affects the UI in no way, other than removing the effects applied by the PageHeader's VisualStateNarrow - effectively shifting the Text left 48 pixels. 

## <a name="pageheader-controlling-the-visual-states"></a>Controlling the Visual States

You can control the PageHeader's visual states by defining the minimum widths that triggers them. The
`VisualStateNarrowMinWidth` and `VisualStateNormalMinWidth` properties accomplish this.

You can apply these values like this:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Text="Main page" 
        Frame="{x:Bind Frame}"
        VisualStateNarrowMinWidth="0"
        VisualStateNormalMinWidth="800" />

</Page>
```

In the case above, when the width of the window is greater than 0 effective pixels but less than 800, the
VisualStateNarrow visual state will be triggered. Concurrently, when the width of the window is equal to or greater
than 800 effective pixels, the VisualStateNormal visual state will be triggered - shifting the `PageHeader.Text` 48 pixels
to the right. 

> Remember: to achieve the best result, the `VisualStateNarrowMinWidth` property of the HamburgerMenu control should be set to the same value.

Here's how your UI will behave:

![](http://i.imgur.com/07deVoH.gif)

## Disable the Visual States
Some developers may not want the Narrow View State to be applied to the PageHeader. This will certainly be true if
the developer is not implementing the hamburger button. In this case, setting the `ViewStateNarrowMinWidth` to the
value of -1 will cause it to never qualify and never be applied.

You would handle this scenario like this:

```XAML
<Page x:Class="Controls.MainPage"
      xmlns:controls="using:Template10.Controls">

    <controls:PageHeader Text="Main page" 
        Frame="{x:Bind Frame}"
        VisualStateNarrowMinWidth="-1" />

</Page>
```

# Resizer 
A container that will size the a single UIElement by dragging a customizable thumb control.

- [Inspiration](#resizer-inspiration)
- [Properties](#resizer-properties)
- [Syntax](#resizer-syntax)

## <a name="resizer-inspiration"></a>Inspiration
You have all seen web sites that provide the ability to stretch a textbox to give you more room to type - well this control can provide the same capability for the TextBox, Image and more!

## <a name="resizer-properties"></a>Properties
| Name | Type | Notes |
|:---|:---|:---|
|ElementControl| Control| The control that will be resized.|
|GrabberVisibility |Visibility | The visibility of the grabber control. |
|GrabberSize |Size | The size of the grabber control. |
|GrabberBrush| Brush| The color of the grabber control. |

## <a name="resizer-syntax"></a>Syntax
The `ResizerControl` can be styled:
````XAML
<Style TargetType="controls:Resizer">
    <Setter Property="GrabberBrush" Value="{StaticResource CustomColorBrush}" />
    <Setter Property="GrabberVisibility" Value="Visible" />
</Style>
````
The `ResizerControl` is used:
````XAML
<controls:Resizer x:Name="parameterResizer" Margin="16,16,16,0">
    <TextBox MinWidth="250" MinHeight="62"
                Header="Parameter to pass"
                Text="{Binding Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    </TextBox>
</controls:Resizer>
````
Here is an example of sizing a TextBox with a lot of content:
![ResizingImage](http://i.imgur.com/RZbcfs6.gif)

# RingSegment
Derived from `Path`, this control allows you to draw any part of a circle:

- [Properties](#ringsegment-properties)
- [Methods](#ringsegment-methods)
- [Syntax](#ringsegment-syntax)
- [Using with a splash screen](#using-with-a-splash-screen)

## <a name="ringsegment-properties"></a>Properties
| Name | Type | Notes |
|:---|:---|:---|
|StartAngle| Double| The start point of the segment's path, from 0-360|
|EndAngle| Double| The end point of the segment's path, from 0-360|
|Radius| Double| The distance from the center of the ring to the outside edge of the ring|
|InnerRadius| Double| The distance from the center of the ring to the inner edge of the ring|
|Center| Point?| The position of the centre of the ring. If null, the ring is positioned at (Radius + StrokeThickness / 2, Radius + StrokeThickness / 2)| 

The segment is drawn from `StartAngle` to `EndAngle` in an clockwise direction.

Since the control is derived from `Path`, it inherits the properties from `Path` to allow you to style the ring
segment to a finer degree.

## <a name="ringsegment-methods"></a>Methods
````csharp
// Suspends path updates until EndUpdate is called
public void BeginUpdate()
// Resumes immediate path updates every time a component property value changes. Updates the path.
public void EndUpdate()
````

By default, any changes to the properties will result in the path being redrawn immediately. This behaviour can be
suspended by calling `BeginUpdate`, changing all of the properties as required then calling `EndUpdate`, at which
point the path will be drawn just once to reflect the new values.

## <a name="ringsegment-syntax"></a>Syntax
The `RingSegment` is used:
````XAML
<Controls:RingSegment x:Name="MyRingSlice" HorizontalAlignment="Center"
                        VerticalAlignment="Center" EndAngle="0"
                        Fill="Transparent" Stroke="{StaticResource ExtendedSplashForeground}"
                        InnerRadius="90" Radius="300" StrokeThickness="2" />
````

This example doesn't fill the segment, thus clearly showing the difference between `InnerRadius` and `Radius`.

## Using with a splash screen
A good use of this control is to make the extended splash screen more interesting than a `ProgressRing`. To do this, the
following steps may help get you going.

Assuming that you've started with something like the Minimal template, edit Splash.xaml and replace the occurence of

````XAML
<Image Source="ms-appx:///Assets/SplashScreen.png" />
````

with

````XAML
<Grid>
    <Image Source="ms-appx:///Assets/SplashScreen.png" />
    <Controls:RingSegment x:Name="MyRingSlice" HorizontalAlignment="Center"
                            VerticalAlignment="Center" EndAngle="0"
                            Fill="{StaticResource ExtendedSplashForeground}"
                            InnerRadius="90" Radius="100" />
</Grid>
````

Add the following near the top of that file, just before this line:

````XAML
<Grid Background="{StaticResource ExtendedSplashBackground}">
````

````XAML
<UserControl.Resources>
    <Storyboard x:Name="RingStoryboard">
        <DoubleAnimationUsingKeyFrames RepeatBehavior="Forever"
                                        EnableDependentAnimation="True"
                                        Storyboard.TargetName="MyRingSlice"
                                        Storyboard.TargetProperty="EndAngle">
            <EasingDoubleKeyFrame KeyTime="0" Value="0" />
            <EasingDoubleKeyFrame KeyTime="0:0:3" Value="359.9999" />
        </DoubleAnimationUsingKeyFrames>
    </Storyboard>
</UserControl.Resources>
````

Finally, in Splash.xaml.cs, add the following line to the end of the `Splash` constructor:

````csharp
RingStoryboard.Begin();
````

When the app is then run, if there is a long-running activity during `OnStartAsync`, a ring segment will be drawn that eventually
forms a complete circle and then starts again. That process will take 3 seconds (the "0:0:3" in the `EasingDoubleKeyFrame`
statement). If you want the drawing of the circle to take long, increase that time.
