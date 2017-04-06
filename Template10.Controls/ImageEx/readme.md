## Template10.Controls.ImageEx

**ImageEx** is a drop-in replacement for the XAML `<Image />` control for the special use case when the `Source` property is changed quickly. A known bug in the control is that the image will flash (revealing the background visual) as it changes sourcesto the new source. 

### Nuget information

![nuget syntax](_images/InstallPackage.png "Install package")
[Click here to visit the Nuget page for this control](https://www.nuget.org/packages/Template10.Controls.ImageEx/ "Nuget page")

### Example of the problem 
The standard `<Image />` is on the left, the `<ImageEx />` is on the right. As the images cycle through the `ListView` on the top, the `<Image />` flickers as it changes, `<ImageEx />` does not.

![the demo applicaton](_images/ImageExDemo.gif "The demo application")

### Syntax

````
<Page xmlns:controls="using:Template10.Controls">
    <controls:ImageEx Source="Image.png" />
</Page>
````

### Members

> Notice not all `Image` properties are exposed through this control. This is not a replacement for the `Image` control, it is a solution to a problematic use case. Having said that, this control is not `sealed` and you are free to extend it or improve it through apull request.

`ImageSource Source { get; set; }` property. Setting the Source property is inherently an asynchronous action. Because it's a property, there isn't an awaitable syntax, but for most scenarios you don't need to interact with the asynchronous aspects of image source file loading. The framework will wait for the image source to be returned, and will rerun layout when the image source file becomes available.

`Stretch Stretch { get; set; }` property. The value of the Stretch property is only relevant if the Image instance is not already using explicitly set values for the Height and/or Width property, and if the Image instance is inside a container that can stretch the image to fill some available space in layout. If you set the value of the Stretch property to None, the image always retains its natural size, even if there's a layout container that might stretch it otherwise. For more info on image sizing, see Remarks in Image. 

`ImageOpened` event. Conditions in which this event can occur include: File not found, Invalid(unrecognized or unsupported) file format, Unknown file format decoding error after upload, Qualified resource reload by the system You might be able to use the ErrorMessage in event data to determine the nature of the failure. ImageFailed and ImageOpened are mutually exclusive.One event or the other will always file whenever an Image has a Source value set or reset.

`ImageFailed` event.
Conditions in which this event can occur include: File not found, Invalid(unrecognized or unsupported) file format, Unknown file format decoding error after upload, Qualified resource reload by the system You might be able to use the ErrorMessage in event data to determine the nature of the failure. ImageFailed and ImageOpened are mutually exclusive.One event or the other will always file whenever an Image has a Source value set or reset.

