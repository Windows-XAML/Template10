using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Utils
{
    public static class ObservableCollectionUtils
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list) => new ObservableCollection<T>(list);

        public static int AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> items, bool clearFirst = false)
        {
            if (clearFirst)
            {
                list.Clear();
            }
            foreach (var item in items)
            {
                list.Add(item);
            }
            return list.Count;
        }
    }
}
