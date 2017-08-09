using System;
using Template10.Common;
using Template10.Services.Serialization;

namespace Template10.Core
{
    public class PropertyBagSerialziationStrategy : IPropertyBagExSerializationStrategy
    {
        private ISerializationService _service;

        public PropertyBagSerialziationStrategy(ISerializationService service)
        {
            _service = service;
        }

        public T Deserialize<T>(object value)
        {
            if (value is string s)
            {
                return _service.Deserialize<T>(s);
            }
            else
            {
                // in order to save to settings we require a string
                throw new ArgumentException($"{nameof(value)} must be string : {value}");
            }
        }

        public object Serialize(object obj)
        {
            return _service.Serialize(obj);
        }
    }
}
