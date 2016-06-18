using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.MarketplaceService
{
    public interface IAppNagService
    {
        Task RegisterAppReviewNag(int launches, TimeSpan duration);
        Task RegisterAppReviewNag(int launches);
        Task RegisterAppReviewNag(TimeSpan duration);
    }
}
