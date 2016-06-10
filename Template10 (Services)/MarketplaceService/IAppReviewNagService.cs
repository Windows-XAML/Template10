using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.MarketplaceService
{
    interface IAppReviewNagService
    {
        void NagAfterLaunches(int launches);
        void NagAfterTimeSpan(TimeSpan timespan);
        void ShowNag();


    }
}
