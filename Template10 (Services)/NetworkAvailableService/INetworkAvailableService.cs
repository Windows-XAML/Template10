namespace Template10.Services.NetworkAvailableService
{
    public interface INetworkAvailableService
    {
        System.Threading.Tasks.Task<bool> IsInternetAvailable();
    }
}