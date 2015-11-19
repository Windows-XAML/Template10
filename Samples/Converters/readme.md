# Converters

This sample demonstrates the use of various Converters included in the Template10 library. There is a single **View** (MainPage.xaml) and a single **ViewModel** (MainPageViewModel.cs).

To learn more about Converters you can check the [wiki documentation](https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Converters).

![Converters Demo](img/sampleapp.png)

## Date Conversions

This demo uses a DatePicker control to set a DateTime property in the ViewModel. The date is then bound to various Text elements and the value is formatted usign the StringFormatConverter with the following parameters:

| Description | Parameter |
| -- | -- |
| Long date | D |
| Full date/time (short time) | f |
| Year month | Y |
| RFC1123 | r |

## Number Conversions

This demo uses a ComboBox control to set a Double property in the ViewModel. The number is then bound to various Text elements and the value is formatted usign the StringFormatConverter with the following parameters:

| Description | Parameter |
| -- | -- |
| Currency | C |
| Exponential (3) | e3 |
| Fixed-point (4) | F4 |
| Percent | p |

## Value-When Conversions

This demo uses a CheckBox control to set a Boolean property in the ViewModel.  The ValueWhenConverter is set with the following properties:

| Property | Value |
| -- | -- |
| When | True |
| Value | Checkbox is selected |
| Otherwise | Checkbox is NOT selected, please select the checkbox |

In the rendered TextBlock, the value passed in for binding is the Boolean property from the ViewModel.  If the Boolean is *True*, then the TextBlock renders **Checkbox is selected**.  If the Boolean is *False*, then the TextBlock renders **Checkbox is NOT selected, please select the checkbox**.