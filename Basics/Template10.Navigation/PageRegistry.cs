using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Navigation
{
    public class PageRegistry : Dictionary<string, (Type Page, Type ViewModel)>
    {
        public bool TryGetPageInfoType(string path, out (Type Page, Type ViewModel) info)
        {
            try
            {
                var key = path.Replace("/", string.Empty).Split("?").First();
                if (TryGetValue(key, out var value))
                {
                    info = (value.Page, value.ViewModel);
                    return true;
                }
                else
                {
                    throw new Exception($"Path [{path}] Key [{key}] not found in {nameof(PageRegistry)}");
                }
            }
            catch (Exception ex)
            {
                info = (default(Type), default(Type));
                return false;
            }
        }
    }
}
