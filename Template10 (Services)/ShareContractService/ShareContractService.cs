namespace Template10.Services.ShareContractService
{
    public class ShareContractService : IShareContractService
    {
        ShareContractHelper _helper = new ShareContractHelper();

        public void ShowUI()
        {
            _helper.ShowUI();
        }
    }
}
