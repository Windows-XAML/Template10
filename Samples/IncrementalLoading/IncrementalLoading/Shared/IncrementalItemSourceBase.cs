using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Samples.IncrementalLoadingSample.Shared
{
    public abstract class IncrementalItemSourceBase<TItem>
    {
        public event EventHandler<bool> HasMoreItemsChanged;

        public void RaiseHasMoreItemsChanged(bool value)
        {
            if (this.HasMoreItemsChanged != null)
            {
                this.HasMoreItemsChanged(this, value);
            }
        }

        protected internal abstract Task LoadMoreItemsAsync(ICollection<TItem> collection, uint suggestLoadCount);

        protected internal virtual void OnRefresh(ICollection<TItem> collection)
        {
            // clearup the collection.
            collection.Clear();

            // reset has more items.
            this.RaiseHasMoreItemsChanged(true);
        }
    }
}