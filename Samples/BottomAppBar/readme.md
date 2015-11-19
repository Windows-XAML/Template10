It turns out that the BottomAppBar property of Page, when set, occludes the SpltView.Pane. This is because it is drawn over the content, even though the page is nested inside the SplitView. The SplitView is a good choice for developer who want a bottom app bar and want their bottom app bar to move when the soft keyboard is displayed. I personally think this is a 1% case, but it is real. 

How does this work? 

When the NavigationService is set in the HamburgerButton it immediately attaches to the FrameFacade Navigated and Navigating events. Both of those EventArgs have been changed to include the previous / new Page reference, respectively. With those references, we check for an existing BottomAppBar, check its visibility, and check its IsOpen state. We adjust only the SplitView.Panel bottom Margin value to match the ActualHeight of the AppBar. We also attach to the Opened and Closed events of the AppBar to increase or decrease the margin to match the new height, based on the AppBar content.

