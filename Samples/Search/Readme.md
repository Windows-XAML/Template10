#Search (and Login)

This project demonstrates how to have an overlay while using the HamburgerMenu control. This is accomplished by handling the HamburgerButtonInfo.Tapped event and showing the correct UI.

In this sample, the correct UI is not the result of navigating to another page, but overlaying the UI over the HamburgerButton. Each of the different views is controlled by Visual States in Shell.xaml. 

##Search example:

The search example takes advantage of the fact that when HamburgerButtonInfo.PageType is null, the logic used to select the button is not executed. As a result, handling the Hamburgerinfo.Info.Tapped event can be declared in XAML like this (found in Shell.xaml):

````XAML
<Controls:HamburgerButtonInfo Checked="SearchChecked" Unchecked="SearchTapped">
    <StackPanel Orientation="Horizontal">
        <SymbolIcon Width="48" Height="48" Symbol="Find" />
        <TextBlock Margin="12,0,0,0" VerticalAlignment="Center" Text="Search" />
    </StackPanel>
</Controls:HamburgerButtonInfo>
````

The search UI margin is "48,0,0,0". This allows the HamburgerMenu to remain visible. The user can then select any other button in the menu. As a result, we handle both the HamburgerButtonInfo.Checked and HamburgerButtonInfo.Unchecked events with the same logic. Tapping on any other button in the HamburgerMenu will cause it to be Checked, and the SearchButton to be UnChecked. 

````csharp
private void SearchChecked(object sender, RoutedEventArgs e)
{
    VisualStateManager.GoToState(this, SearchVisualState.Name, true);
}

private void SearchUnchecked(object sender, RoutedEventArgs e)
{
    VisualStateManager.GoToState(this, NormalVisualState.Name, true);
}
````

##Login example:

In the case of the Login example, the overlayed UI has no margin - the idea is that we want the user to complete the workflow before gaining access to the HamburgerMenu. We also set the HamburgerMenu.IsEnabled property to false in the VisualState so the user doesn't sneak to it using tabs.

This scenario is quite a bit simpler as we only need to handle the Tapped event of the HamburgerUserInfo. We could have jsut as easily handled the Checked event, but not the Selected event as this is only raised as a result of selection which occurs only for HamburgerButtonInfo items with PageType not null.

````csharp
private void LoginTapped(object sender, RoutedEventArgs e)
{
    (sender as HamburgerButtonInfo).IsChecked = false;
    VisualStateManager.GoToState(this, LoginVisualState.Name, true);
}

private void LoginHide(object sender, System.EventArgs e)
{
    VisualStateManager.GoToState(this, NormalVisualState.Name, true);
}
````

That second handler (LoginHide) is a custom event we put on our user control as a way to communicate back to our Shell. This event is effectively asking to return to the Normal visual state. You can handle this any way you want in your app, including messaging. This event is the easy approach.

Good luck!