using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Blank1.Triggers
{
    public class ApiInformationTrigger : StateTriggerBase
    {
        public ApiInformationTrigger()
        {
            EvaluateTrigger();
        }

        private void EvaluateTrigger()
        {
            // Flags for evaluation
            bool anySpecified = false;
            bool anyMet = false;
            bool allMet = true;

            // Check type availability?
            if (!string.IsNullOrEmpty(typeName))
            {
                anySpecified = true;
                if (ApiInformation.IsTypePresent(typeName))
                    anyMet = true;
                else
                    allMet = false;
            }

            // Check contract availability?
            if (!string.IsNullOrEmpty(contractName))
            {
                anySpecified = true;

                // Evaluate, using minor version if specified
                bool contractMet = (contractMinorVersion.HasValue ? ApiInformation.IsApiContractPresent(contractName, contractMajorVersion, contractMinorVersion.Value) : ApiInformation.IsApiContractPresent(contractName, contractMajorVersion));
                if (contractMet)
                    anyMet = true;
                else
                    allMet = false;
            }

            if (!anySpecified)
                // no APIs were specified
                SetActive(false);
            else if (requireAll)
                // Are all required?
                SetActive(allMet);
            else
                // Only one is required
                SetActive(anyMet);
        }

        private ushort contractMajorVersion = 1;
        public ushort ContractMajorVersion
        {
            get { return contractMajorVersion; }
            set
            {
                if (contractMajorVersion == value)
                    return;
                contractMajorVersion = value;
                EvaluateTrigger();
            }
        }

        private ushort? contractMinorVersion = null;
        public ushort? ContractMinorVersion
        {
            get { return contractMinorVersion; }
            set
            {
                if (contractMinorVersion == value)
                    return;
                contractMinorVersion = value;
                EvaluateTrigger();
            }
        }

        private string contractName;
        public string ContractName
        {
            get { return contractName; }
            set
            {
                if (contractName == value)
                    return;
                contractName = value;
                EvaluateTrigger();
            }
        }

        private bool requireAll = true;
        public bool RequireAll
        {
            get { return requireAll; }
            set
            {
                if (requireAll == value)
                    return;
                requireAll = value;
                EvaluateTrigger();
            }
        }

        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set
            {
                if (typeName == value)
                    return;
                typeName = value;
                EvaluateTrigger();
            }
        }

    }
}
