using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Template10.Utils;
using System.Collections.Specialized;
using System.Collections;

namespace Template10.Common
{
    public class TemplatePartNotFoundException : Exception
    {
        public TemplatePartNotFoundException(string message) : base(message) { }
    }
}