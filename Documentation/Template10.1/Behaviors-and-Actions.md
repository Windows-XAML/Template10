# Table of Contents

1. [What is a XAML Behavior?](#what-is-a-xaml-behavior)
1. [`EllipsisBehavior`](#ellipsisbehavior)
1. [`NavButtonBehavior`](#navbuttonbehavior)
1. [`DeviceDispositionBehavior`](#devicedispositionbehavior)
1. [`BackButtonBehavior`](#backbuttonbehavior)
1. [`TextBoxEnterKeyBehavior`](#textboxenterkeybehavior)
1. [`KeyBehavior`](#keybehavior)
1. [What is a XAML Action?](#what-is-a-xaml-action)
1. [`CloseFlyoutAction`](#closeflyoutaction)
1. [`ConditionalAction`](#conditionalaction)
1. [`FocusAction`](#focusaction)
1. [`OpenFlyoutAction`](#openflyoutaction)
1. [`TimeoutAction`](#timeoutaction)
1. [`MessageDialogAction`](#messagedialogaction)
1. [`NavToPageAction`](#navtopageaction)

# What is a XAML Behavior?
A XAML behavior is typically, though not necessarily, responsible for listening for a preconfigured or manually configured event.

XAML behaviors are a mechanism to encapsulate code with logic and configuration parameters such that it can then be
declared in XAML without requiring code-behind. Most XAML Behaviors have design-time support. They can easily be
written in C# and implement the simple interface [`IBehavior`](https://msdn.microsoft.com/en-us/library/microsoft.xaml.interactivity.ibehavior(v=vs.120).aspx) and typically contain XAML Actions `IAction`.

An example of a XAML behavior is the PropertyChangedBehavior (which is native to the framework) which waits for the
value of a property to change (using INotifypropertyChanged). XAML behaviors, once triggered, invoke one or more XAML
actions - also part of the behavior framework. 

#### Syntax
````csharp
[ContentProperty(Name = nameof(Actions))]
public class MyBehavior : DependencyObject, IBehavior
{
    public DependencyObject AssociatedObject { get; set; }

    public void Attach(DependencyObject associatedObject)
    {
        AssociatedObject = associatedObject;
    }

    public void Detach()
    {
        // TODO
    }

    public ActionCollection Actions
    {
        get
        {
            var actions = (ActionCollection)base.GetValue(ActionsProperty);
            if (actions == null)
                base.SetValue(ActionsProperty, actions = new ActionCollection());
            return actions;
        }
    }
    public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(nameof(Actions),
        typeof(ActionCollection), typeof(MyBehavior), new PropertyMetadata(null));
}
````

# EllipsisBehavior
The intent of the `EllipsisBehavior` is to fix a flaw in the native XAML `CommandBar` control. By default, the ellipsis
which is used to show additional elements is always visible, even when not necessary. This behavior allows the developer
to specify whether the ellipsis is always `Visible`, `Collapsed` or `Auto` (determines visibility based upon whether
there is content present).
> This is valuable when your design requires you to remove the ellipse.

#### Syntax
````XAML
<!--  header  -->
<controls:PageHeader Frame="{x:Bind Frame}" Text="{x:Bind ViewModel.Article.Headline, Mode=OneWay}">
    <Interactivity:Interaction.Behaviors>
        <Behaviors:EllipsisBehavior Visibility="Auto" />
    </Interactivity:Interaction.Behaviors>
</controls:PageHeader>
````

# NavButtonBehavior
The intent of the `NavButtonBehavior` is to add behavior to any native XAML `Button` or `AppBarButton`. Once set to
either [ Forward | Back ] then clicking the `Button` will navigate the referenced `Frame` accordingly.
> This is valuable when wanting to create navigation buttons.

#### Syntax
````XAML
<AppBarButton Icon="Forward" Label="Forward">
    <Interactivity:Interaction.Behaviors>
        <Behaviors:NavButtonBehavior Direction="Forward" Frame="{x:Bind Frame, Mode=OneWay}" />
    </Interactivity:Interaction.Behaviors>
</AppBarButton>
````
# DeviceDispositionBehavior
The intent of the `DeviceDispositionBehavior` is to add behavior to any page that should occur on one or more
specified device families. **Note:** this behavior is typically added to a top level element as it is listening
to system events, (say transitioning to Continuum) rather than element level events.

> This is valuable when you want to change some aspect of your apps behavior based upon one or more device families. An
example could be hiding UIElements that are not applicable on a device, such as in-app back buttons on a Phone.

#### Properties
````CSHARP
// true if the behavior applies to IoT devices; otherwise false
public bool IoT { get; set }
// true if the behavior applies to Xbox devices; otherwise false
public bool Xbox { get; set }
// true if the behavior applies to Team devices; otherwise false
// note: Team is the Surface Hub
public bool Team { get; set }
// true if the behavior applies to HoloLens devices; otherwise false
public bool HoloLens { get; set }
// true if the behavior applies to Desktop devices; otherwise false
public bool Desktop { get; set }
// true if the behavior applies to Mobile devices; otherwise false
public bool Mobile { get; set }
// true if the behavior applies to Phone devices; otherwise false
// note: determined if diagonal inches is less 7
public bool Phone { get; set }
// true if the behavior applies to Continuum devices; otherwise false
public bool Continuum { get; set }
// true if the behavior applies to Virtual devices; otherwise false
public bool Virtual { get; set }
````

#### Syntax
````XAML
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <Interactivity:Interaction.Behaviors>
        <Behaviors:DeviceDispositionBehavior Desktop="True">
            <Core:GoToStateAction StateName="DesktopOnlyState"/>
        </Behaviors:DeviceDispositionBehavior>
    </Interactivity:Interaction.Behaviors>
</Grid>
````

# BackButtonBehavior
The intent of the `BackButtonBehavior` is to add behavior to any page or view. The behavior will be triggered
whenever the `BootStrapper.BackRequested` event is raised. Setting `e.Handled = true` in the code invoked by
the behavior/action will prevent the system from executing a back navigation. **Note:** this behavior is typically
added to a top level element as it is listening to the `BootStrapper.BackRequested` rather than element level events.
> This is valuable when you want to intercept the `BootStrapper.BackRequested` and perform a specific action, then mark the event as handled. An example would be intecepting the `BootStrapper.BackRequested` event and closing a `PopUp` instead of navigating back.

#### Properties
`none`

#### Syntax
````XAML
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <Interactivity:Interaction.Behaviors>
        <Behaviors:BackButtonBehavior>
            <Core:CallMethodAction MethodName="ClosePopUp" TargetObject="{Binding ElementName=page}"/>
        </Behaviors:BackButtonBehavior>
    </Interactivity:Interaction.Behaviors>
</Grid>
````

# TextBoxEnterKeyBehavior
This is now obsolete - use [`KeyBehavior`](#keybehavior) instead.

# KeyBehavior
The intent of the `KeyBehavior` is to add behavior to any `UIElement` that supports key events. The behavior
has properties that allow the developer to specifiy whether the action is performed on key up or key down,
and whether modifier keys can also be used.
> This is valuable when you wish to perform an action based upon a key press.

#### Properties
````CSHARP
// The key that triggers the behavior
public VirtualKey Key { get; set; } = VirtualKey.None;
// true if Control must be held also; otherwise false
public bool AndControl { get; set; } = false;
// true if Alt must be held also; otherwise false
public bool AndAlt { get; set; } = false;
// true if Shift must be held also; otherwise false
public bool AndShift { get; set; } = false;
// Detemines if the behavior is triggered on the KeyUp or the KeyDown event
public Kinds Event { get; set; } = Kinds.KeyUp;
````
#### Syntax
````XAML
<TextBox MinWidth="250" MinHeight="62"
            Header="Parameter to pass"
            Text="{Binding Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    <Interactivity:Interaction.Behaviors>
        <!--  enable submit on enter key  -->
        <Behaviors:KeyBehavior Key="Enter">
            <Core:CallMethodAction MethodName="GotoDetailsPage" TargetObject="{Binding}" />
        </Behaviors:KeyBehavior>
        <!--  focus on textbox when page loads  -->
        <Core:EventTriggerBehavior>
            <Behaviors:FocusAction />
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</TextBox>
`````

# What is a XAML Action?
A XAML action (implementing [IAction](https://msdn.microsoft.com/en-us/library/microsoft.xaml.interactivity.iaction(v=vs.120).aspx)) does not typically listen to events, but instead takes some action - which is limited only by the
developer's imagination.

An example of a XAML action is the ChangePropertyAction (which is native to the framework) which changes the value
of some property to a new value, typically leveraging data-binding. 

#### Syntax
````csharp
public sealed class MyAction : DependencyObject, IAction
{
    public object Execute(object sender, object parameter)
    {
        // TODO
    }
}
````

# CloseFlyoutAction
The intent of the `CloseFlyoutAction` is to close the first `FlyOut` parent in the Visual Tree. When
invoked by a behavior, it will hunt up the XAML tree for the first `FlyOut` and set its `IsOpen` property to false. 
> This is valuable for small forms inside a `FlyOut` and is commonly used on Submit buttons in those forms.

#### Syntax
````XAML
<Flyout x:Key="FormFlyout">
    <StackPanel>
        <TextBox Header="Name" Text="{x:Bind VM.Name, Mode=TwoWay}"/>
        <Button>
            <Interactivity:Interaction.Behaviors>
                <Core:EventTriggerBehavior EventName="Click">
                    <Behaviors:CloseFlyoutAction/>
                </Core:EventTriggerBehavior>
            </Interactivity:Interaction.Behaviors> Submit</Button>
    </StackPanel>
</Flyout>
````

# ConditionalAction
The intent of the `ConditionalAction` is to prevent subsequent actions unless a condition is met. When
invoked by a behavior, it will evaluate the condition and invoke child actions if the condition is met.
> This is valuable if a child action cannot be executed until some condition is satisfied.

#### Syntax
````XAML
<Button>
    <Core:EventTriggerBehavior EventName="Clicked"> 
        <b:ConditionalAction xmlns:b="using:Template10.Behaviors" 
            LeftValue="{Binding IsLoggedIn}" Operator="IsTrue"> 
            <Core:GoToStateAction StateName="LoggedInViewState" /> 
        </b:ConditionalAction> 
    </Core:EventTriggerBehavior> 
</Button>
````

# FocusAction
The intent of the `FocusAction` is to call `Control.Focus()` on some referenced control. When invoked by
a behavior, it will call Focus() and ignore if it succeed or not.
> This is valuable in situations like Page.Load, focusing on the first element. It can also change the focus when
used in conjunction with `KeyBehavior`.

#### Syntax
````XAML
<TextBox MinWidth="250" MinHeight="62"
            Header="Parameter to pass"
            Text="{Binding Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    <Interactivity:Interaction.Behaviors>
        <!--  enable submit on enter key  -->
        <Behaviors:KeyBehavior Key="Enter">
            <Core:CallMethodAction MethodName="GotoDetailsPage" TargetObject="{Binding}" />
        </Behaviors:KeyBehavior>
        <!--  focus on textbox when page loads  -->
        <Core:EventTriggerBehavior>
            <Behaviors:FocusAction />
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</TextBox>
`````

# OpenFlyoutAction
The intent of the `OpenFlyoutAction` is to open the FlyoutBase on the specified XAML element. When invoked by
a behavior, it will look for the `FlyOut` and call `Show()`.
> This is valuable because it can be coupled with actions like `ConditionalAction` that can prevent the `FlyOut` until
a condition. Or on controls that otherwise don't support a `FlyOut`.

#### Syntax
````XAML
<AppBarButton Icon="Find" Label="Search">
	<FlyoutBase.AttachedFlyout>
		<Flyout>
			<StackPanel>
				<TextBlock Text="Awesome Flyout!" />
			</StackPanel>
		</Flyout>
	</FlyoutBase.AttachedFlyout>
	<Interactivity:Interaction.Behaviors>
		<Core:EventTriggerBehavior EventName="Tapped">
			<Behaviors:OpenFlyoutAction />
		</Core:EventTriggerBehavior>
	</Interactivity:Interaction.Behaviors>
</AppBarButton>
````

# TimeoutAction
The intent of the `TimeoutAction` is to invoke child actions only after a specified number of seconds passes. When
invoked by a behavior, a timer starts and child Actions are called once the time has passed. 
> This is valuable when you want to delay a response or invoke a secondary action after some time.

#### Syntax
````XAML
<Button>
    <Interactivity:Interaction.Behaviors>
        <Core:EventTriggerBehavior EventName="Click">
            <Core:CallMethodAction MethodName="ShowBusy" TargetObject="{Binding Mode=OneWay}" />
            <Behaviors:TimeoutAction Milliseconds="5000">
                <Core:CallMethodAction MethodName="HideBusy" TargetObject="{Binding Mode=OneWay}" />
            </Behaviors:TimeoutAction>
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</Button>
````

# MessageDialogAction
The intent of the `MessageDialogAction` is to display a `ContentDialog` with the specified `Title`, `Content` and
option `OkText` values. 
> This is valuable when you want a simple way to show a notification dialog.

#### Properties
````CSHARP
// The title of the dialog
public string Title { get; set }
// The content of the dialog
public string Content { get; set }
// The Ok Text of the dialog
public string OkText { get; set } = "Ok";
````

#### Syntax
````XAML
<Button Content="Delete">
    <Interactivity:Interaction.Behaviors>
        <Core:EventTriggerBehavior EventName="Click">
            <Behaviors:MessageDialogAction Content="This is the Content" 
                                           Title="A Title"
                                           OkText="Agreed!"/>
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</Button>
````

# NavToPageAction

> This behavior is still in development.

The intent of the `NavToPageAction` is to navigate to the specified page using the supplied parameters. **Note:** Incomplete.

> This is valuable when you want to simply navigate to page.

#### Properties
````CSHARP
* **Frame**, Window.UI.XAML.Controls.Frame, required
* **Page**, string, required, full name (App.Views.MainPage)
* **Parameter**, string, optional
* **InfoOverride**, optional, Window.UI.XAML.Media.Animation.NavigationTransitionInfo
````
#### Syntax
````XAML
<Button Content="Click me">
    <Interactivity:Interaction.Behaviors>
        <Core:EventTriggerBehavior EventName="Click">
            <b:NavToPageAction 
                Frame="{x:Bind Frame}" 
                Page="MultiplePageHeaders.Views.ContainerPage" />
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</Button>
````
