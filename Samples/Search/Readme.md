#Template10.Samples.SearchSample (and Login)

This project demonstrates how to have an overlay while using the HamburgerMenu control. This is accomplished by handling the HamburgerButtonInfo.Tapped event and showing the correct UI.

In this sample, the correct UI is not the result of navigating to another page, but overlaying the UI over the HamburgerButton. Each of the different views is controlled by two (2) Visual States in ModalDialog Control.

##Template10.Samples.SearchSample example:

The search example takes advantage of the fact that when HamburgerButtonInfo.ButtonType is Command, underlying logic allows for HamburgerButtonInfo.Tapped event to be Raised. As a result, handling the HamburgerButtonInfo.Tapped event can be declared in XAML like this (found in Shell.xaml):

````XAML
<Controls:HamburgerButtonInfo ButtonType="Command" Tapped="SearchTapped">
    <StackPanel Orientation="Horizontal">
        <SymbolIcon Width="48" Height="48" Symbol="Find" />
        <TextBlock Margin="12,0,0,0" VerticalAlignment="Center" Text="Template10.Samples.SearchSample" />
    </StackPanel>
</Controls:HamburgerButtonInfo>
````

````csharp
 private void SearchTapped(object sender, RoutedEventArgs e)
 {	
    SearchModal.IsModal = true;
 }

 // request to hide search (from inside search)
 private void SearchHide(object sender, EventArgs e)
 {
     SearchModal.IsModal = false;
 }

 // request to goto detail
 private void SearchNav(object sender, string item)
 {
    SearchModal.IsModal = false;
    MyHamburgerMenu.NavigationService.Navigate(typeof(Views.DetailPage), item);
 }
````

##Login example:

In the case of the Login example, the overlayed UI has no margin - the idea is that we want the user to complete the workflow before gaining access to the HamburgerMenu.

This scenario is quite a bit simpler as we only need to handle the Tapped event of the LoginButton (HamburgerButtonInfo). .

````csharp
private void LoginTapped(object sender, RoutedEventArgs e)
{
    LoginModal.IsModal = true;
}

private void LoginHide(object sender, System.EventArgs e)
{
    LoginModal.IsModal = false;
}
````

That second handler (LoginHide) is a custom event we put on our user control as a way to communicate back to our Shell. This event is effectively asking to return to the Normal visual state. You can handle this any way you want in your app, including Template10.Samples.SearchSample. This event is the easy approach.

Good luck!