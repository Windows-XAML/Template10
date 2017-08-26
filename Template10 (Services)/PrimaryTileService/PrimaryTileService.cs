namespace Template10.Services.PrimaryTileService
{
    public class PrimaryTileService : IPrimaryTileService
    {
        PrimaryTileHelper _helper = new PrimaryTileHelper();

        public void UpdateBadge(int value)
        {
            this._helper.UpdateBadge(value);
        }
    }

}
