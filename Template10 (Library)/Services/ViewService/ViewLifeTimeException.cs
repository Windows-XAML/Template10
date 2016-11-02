using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Template10.Common;
using Template10.Services.NavigationService;

namespace Template10.Services.ViewService
{
    public sealed partial class ViewLifetimeControl
    {

        public class ViewLifeTimeException : Exception
        {
            public ViewLifeTimeException()
            {
            }

            public ViewLifeTimeException(string message) : base(message)
            {
            }

            public ViewLifeTimeException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }

}