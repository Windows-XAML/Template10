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
