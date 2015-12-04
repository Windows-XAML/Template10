using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.NavigationService
{
    public class JournalEntry
    {
        public Type SourcePageType { get; internal set; }
        public object Parameter { get; internal set; }

        public override bool Equals(object obj)
        {
            var je = obj as JournalEntry;

            if (je == null)
            {
                return false;
            }

            bool ret =
                this.SourcePageType.Equals(je.SourcePageType) &&
                ((this.Parameter == null && je.Parameter == null) ||
                 (this.Parameter.Equals(je.Parameter)));

            return ret;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            if (this.Parameter != null)
            {
                hash = hash * 23 + this.Parameter.GetHashCode();
            }
            else
            {
                hash = hash * 23;
            }

            hash = hash * 23 + this.SourcePageType.GetHashCode();

            return hash;
        }
    }
}
