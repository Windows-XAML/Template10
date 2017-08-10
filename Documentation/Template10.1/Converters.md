# Table of Contents

Template10 ships with a collection of useful converters that you can use in your application.
- [StringFormatConverter](#stringformatconverter)
- [ValueWhenConverter](#valuewhenconverter)
- [ChangeTypeConverter](#changetypeconverter)

# StringFormatConverter
This converter can take in a value and format it using format strings provided as either a parameter or property of the converter.  

- Formatting Types in .NET: [here](https://msdn.microsoft.com/en-us/library/26etazsy.aspx)
- Standard Date/Time Format Strings: [here](http://msdn.microsoft.com/en-us/library/az4se3k1.aspx)
- Custom Date/Time Format Strings: [here](http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx)
- Standard Numeric Format Strings: [here](http://msdn.microsoft.com/en-us/library/dwhawy9k.aspx)
- Custom Numeric Format Strings: [here](http://msdn.microsoft.com/en-us/library/0c899ak8.aspx)

## Implementation

You can add this control as a Resource to another XAML element:

````xaml
<Page.Resources>
    <converters:StringFormatConverter x:Key="StrFormatConverter" />
    <converters:StringFormatConverter x:Key="PriceConverter" Format="{}{0:N4}"/>
    <converters:StringFormatConverter x:Key="ValueConverter" Format="{}{0:N2}"/>
</Page.Resources>
````

With the resource in place, you can use the resource as the **Converter** when binding a value on your page. The **ConverterParameter** binding property specifies the format string:

````xaml
<TextBlock Text="{Binding DateTimeValue, Converter={StaticResource StrFormatConverter}, ConverterParameter=\{0:D\}}" />
<TextBlock Text="{Binding PriceProperty, Converter={StaticResource PriceConverter}" />
<TextBlock Text="{Binding ValueProperty, Converter={StaticResource ValueConverter}" />
````

# ValueWhenConverter

This converter can display data from one of two binary choices.  If the data being bound is equivalent to the **When** property of the converter, then the result of the binding will be the **Value** property of the converter.  If they are not equivalent, the result of the binding will be the **Otherwise** converter.

## Properties

| Name | Type | Notes |
|:---|:---|:---|
| When | object | This is the object evaluated for equivalence with the bound value.  The bound value is technically an input parameter to this converter.|
| Value| object | This object is the result of the binding conversion if the originally bound value is equivalent to the value of the **When** property.|
| Otherwise| object | This object is the result of the binding conversion if the originally bound value is **NOT** equivalent to the value of the **When** property.|

## Implementation

You can add this binding as a Resource to another XAML element.

````xaml
<converters:ValueWhenConverter x:Key="VisibleWhenTrueConverter">
    <converters:ValueWhenConverter.When>
        <x:Boolean>True</x:Boolean>
    </converters:ValueWhenConverter.When>
    <converters:ValueWhenConverter.Value>
        <Visibility>Visible</Visibility>
    </converters:ValueWhenConverter.Value>
    <converters:ValueWhenConverter.Otherwise>
        <Visibility>Collapsed</Visibility>
    </converters:ValueWhenConverter.Otherwise>
</converters:ValueWhenConverter>
````

With the resource in place, you can use the resource as the **Converter** when binding a value on your page.

````xaml
<TextBlock Text="Hello Admin" Visibility="{Binding IsAdmin, Converter={StaticResource VisibleWhenTrueConverter}}" />
````

> Note, if you want to use a value of Null, use the following syntax:

```xaml
<converters:ValueWhenConverter x:Key="VisibleWhenNullConverter" When="{x:Null}">
	<converters:ValueWhenConverter.Value>
		<Visibility>Visible</Visibility>
	</converters:ValueWhenConverter.Value>
	<converters:ValueWhenConverter.Otherwise>
		<Visibility>Collapsed</Visibility>
	</converters:ValueWhenConverter.Otherwise>
</converters:ValueWhenConverter>
```
# ChangeTypeConverter

This converter can convert the type used in a binding - this is especially helpful when using the x:Bind syntax to bind a strongly-typed property in a View Model to a XAML control property that is declared as `object` - such as the `SelectedItem` on a `ListView`.

## Syntax
````XAML
<Page.Resources>
    <converters:ChangeTypeConverter x:Key="TypeConverter"/>
</Page.Resources>


<Grid>
    <ListView ItemsSource='{x:Bind ViewModel.Items, Mode=OneWay}'
              SelectedItem='{x:Bind ViewModel.CurrentItem, Mode=TwoWay, Converter={StaticResource TypeConverter}}'/>
</Grid>

````
