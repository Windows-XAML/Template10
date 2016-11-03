using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Utils;
using Windows.Foundation.Collections;

namespace Template10.Services.Navigation
{

    public interface INavigationParameters : IPropertySet
    {
        T Parameter<T>(string name = nameof(Parameter));
    }

}