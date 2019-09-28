# MarketplaceService
`MarketplaceService` simplifies the common ways of interacting with the Store from within a UWP application.

```csharp
public interface IMarketplaceService
{
    Task LaunchAppInStore();

    Task LaunchAppReviewInStoreAsync();

    Task LaunchPublisherPageInStoreAsync();

    NagEx CreateAppReviewNag();

    NagEx CreateAppReviewNag(string message);
}
```