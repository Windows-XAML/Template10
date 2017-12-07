using System.Text;
using System.Threading;
using Template10.Common;

namespace Template10.Common
{
    // the interface for this is in portable

    public partial class PropertyBagEx : IPropertyBagEx2
    {
        public IPropertyBagExPersistenceStrategy PersistenceStrategy { get; set; }

        public IPropertyBagExSerializationStrategy SerializationStrategy { get; set; }
    }
}
