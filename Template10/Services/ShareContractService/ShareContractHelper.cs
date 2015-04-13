using System;
using Windows.ApplicationModel.DataTransfer;

namespace Template10.Services.ShareContractService
{
    public class ShareContractHelper 
    {
        public ShareContractHelper()
        {
            DataTransferManager.GetForCurrentView().DataRequested += ShareContractHelper_DataRequested;
        }

        private void ShareContractHelper_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            if (DataRequested != null)
            {
                var d = e.Request.GetDeferral();
                DataRequested(e);
                d.Complete();
            }
        }

        public Action<DataRequestedEventArgs> DataRequested { get; set; }

        public void ShowUI()
        {
            DataTransferManager.ShowShareUI();
        }
    }
}