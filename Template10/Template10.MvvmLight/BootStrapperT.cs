using System;
using System.Collections.Generic;

namespace Template10
{
    public abstract class BootStrapper<T>
        : BootStrapper
        where T : struct, IConvertible
    {
        public BootStrapper()
            => SetupPageKeys(PageKeys);

        public static IDictionary<T, Type> PageKeys
            => Navigation.Settings.PageKeys<T>();

        public abstract void SetupPageKeys(IDictionary<T, Type> keys);
    }
}
