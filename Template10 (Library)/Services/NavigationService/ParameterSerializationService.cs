using System;

namespace Template10.Services.NavigationService
{
    public class ParameterSerializationService
    {
        private static volatile ParameterSerializationService instance = new ParameterSerializationService();

        /// <summary>
        /// Gets or sets the instance that should be used to serialize/deserialize.
        /// </summary>
        public static ParameterSerializationService Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        public virtual object SerializeParameter(object parameter)
        {
            return parameter;
        }

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        public virtual object DeserializeParameter(object parameter)
        {
            return parameter;
        }
    }
}