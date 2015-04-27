using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
    /// <summary>
    /// A trigger that can be used to check for available APIs.
    /// </summary>
    public class ApiInformationTrigger : StateTriggerBase
    {

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="ApiInformationTrigger"/> instance.
        /// </summary>
        public ApiInformationTrigger()
        {
            EvaluateTrigger();
        }
        #endregion // Constructors


        #region Internal Methods
        /// <summary>
        /// Reevaluates the trigger value.
        /// </summary>
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
                {
                    anyMet = true;
                }
                else
                {
                    allMet = false;
                }
            }

            // Check contract availability?
            if (!string.IsNullOrEmpty(contractName))
            {
                anySpecified = true;

                // Evaluate, using minor version if specified
                bool contractMet = (contractMinorVersion.HasValue ? ApiInformation.IsApiContractPresent(contractName, contractMajorVersion, contractMinorVersion.Value) : ApiInformation.IsApiContractPresent(contractName, contractMajorVersion));
                if (contractMet)
                {
                    anyMet = true;
                }
                else
                {
                    allMet = false;
                }
            }

            // We don't want to trigger if no APIs were specified at all
            if (!anySpecified)
            {
                SetActive(false);
            }
            // Are all required?
            else if (requireAll)
            {
                SetActive(allMet);
            }
            // Only one is required
            else
            {
                SetActive(anyMet);
            }
        }
        #endregion // Internal Methods


        #region Public Properties
        private ushort contractMajorVersion = 1;
        /// <summary>
        /// Gets or sets the major version of the contract that must be present to satisfy the trigger.
        /// </summary>
        /// <value>
        /// The major version of the contract that must be present to satisfy the trigger. The default is 1.
        /// </value>
        public ushort ContractMajorVersion
        {
            get
            {
                return contractMajorVersion;
            }
            set
            {
                if (contractMajorVersion != value)
                {
                    contractMajorVersion = value;
                    EvaluateTrigger();
                }
            }
        }

        private ushort? contractMinorVersion = null;
        /// <summary>
        /// Gets or sets the minor version of the contract that must be present to satisfy the trigger.
        /// </summary>
        /// <value>
        /// The minor version of the contract that must be present to satisfy the trigger or <see langword="null"/> to not require a minor version. The default is null.
        /// </value>
        public ushort? ContractMinorVersion
        {
            get
            {
                return contractMinorVersion;
            }
            set
            {
                if (contractMinorVersion != value)
                {
                    contractMinorVersion = value;
                    EvaluateTrigger();
                }
            }
        }

        private string contractName;
        /// <summary>
        /// Gets or sets the name of the contract that will be tested with ApiInformation.IsApiContractPresent.
        /// </summary>
        /// <value>
        /// The name of the contract that will be tested with ApiInformation.IsApiContractPresent.
        /// </value>
        /// <remarks>
        /// An example value for this property is "Windows.Devices.Scanners.ScannerDeviceContract" which checks for the availability of a document scanner.
        /// </remarks>
        public string ContractName
        {
            get
            {
                return contractName;
            }
            set
            {
                if (contractName != value)
                {
                    contractName = value;
                    EvaluateTrigger();
                }
            }
        }

        private bool requireAll = true;
        /// <summary>
        /// Gets or sets a value that indicates if all specified APIs must be present to satisfy the trigger.
        /// </summary>
        /// <value>
        /// <c>true</c> if all specified APIs must be present to satisfy the trigger; otherwise <c>false</c>. The default is <c>true</c>.
        /// </value>
        public bool RequireAll
        {
            get
            {
                return requireAll;
            }
            set
            {
                if (requireAll != value)
                {
                    requireAll = value;
                    EvaluateTrigger();
                }
            }
        }

        private string typeName;
        /// <summary>
        /// Gets or sets the name of the type that will be tested with ApiInformation.IsTypePresent.
        /// </summary>
        /// <value>
        /// The name of the type that will be tested with ApiInformation.IsTypePresent.
        /// </value>
        /// <remarks>
        /// An example value for this property is "Windows.Phone.UI.Input.HardwareButtons" which represents the hardware back button on phone devices.
        /// </remarks>
        public string TypeName
        {
            get
            {
                return typeName;
            }
            set
            {
                if (typeName != value)
                {
                    typeName = value;
                    EvaluateTrigger();
                }
            }
        }
        #endregion // Public Properties

    }
}
