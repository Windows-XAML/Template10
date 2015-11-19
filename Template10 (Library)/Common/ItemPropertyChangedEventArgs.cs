using System.ComponentModel;

namespace Template10.Common
{
    public class ItemPropertyChangedEventArgs
    {
        public ItemPropertyChangedEventArgs(object item, PropertyChangedEventArgs e)
        {
            this.ItemChanged = item;
            this.PropertyChangedArgs = e;
        }
        public object ItemChanged { get; set; }

        public PropertyChangedEventArgs PropertyChangedArgs { get; set; }
    }
}
