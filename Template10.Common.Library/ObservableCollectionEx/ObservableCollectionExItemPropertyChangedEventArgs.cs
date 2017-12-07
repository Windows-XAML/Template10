using System;
using System.ComponentModel;

namespace Template10.Common
{
    public class ObservableCollectionExItemPropertyChangedEventArgs: EventArgs
    {
        public ObservableCollectionExItemPropertyChangedEventArgs(object item, int changedIndex, PropertyChangedEventArgs e)
        {
            ChangedItem = item;
            ChangedItemIndex = changedIndex;
            PropertyChangedArgs = e;
        }
        public object ChangedItem { get; set; }

        public int ChangedItemIndex { get; set; }

        public PropertyChangedEventArgs PropertyChangedArgs { get; set; }
    }
}
