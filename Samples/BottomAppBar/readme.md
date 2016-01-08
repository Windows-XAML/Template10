It turns out that the BottomAppBar property of Page, when set, occludes the SpltView.Pane. This is because it is drawn over the content, even though the page is nested inside the SplitView. The SplitView is a good choice for developers who want a bottom app bar and want their bottom app bar to move when the soft keyboard is displayed. I personally think this is a 1% case, but it is real. 

What to do?

There is nothing built-in in Template 10 to handle this but you, as the developer, can easily handle it. On pages that have a bottom app bar, you can adjust its margin as a response to the HamburgerMenu's PaneOpen and PaneClosed events. This sample demonstrates the simple approach. The interesting code follows:

````csharp
public MainPage()
{
    InitializeComponent();

    var ham = Shell.HamburgerMenu;
    ham.PaneOpen += (s, e) => UpdateMargin(ham);
    ham.PaneClosed += (s, e) => UpdateMargin(ham);
    UpdateMargin(ham);
}

private void UpdateMargin(HamburgerMenu ham)
{
    var value = (ham.IsOpen) ? ham.PaneWidth : 48d;
    BottomAppBar.Margin = new Thickness(value, 0, 0, 0);
}
````

One final note. In Template 10, we have the Modal Dialog control to help developers show a busy 
indicator. Since the BottomAppBar is drawn over the content of the Window, it is up to the developer
to disable the BottomAppBar manually when they need it to be disabled. 

This is how the sample does this:

````csharp
private void SampleClick(object sender, RoutedEventArgs e)
{
    Shell.SetBusy(true, "Please wait...");
    BottomAppBar.IsEnabled = false;
    Template10.Common.WindowWrapper.Current().Dispatcher.Dispatch(() =>
    {
        Shell.SetBusy(false);
        BottomAppBar.IsEnabled = true;
    }, 3000);
}
````