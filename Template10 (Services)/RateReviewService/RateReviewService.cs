using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateAndReview
{
    class RateReviewService : IRateReview
    {
        private readonly RateReviewHelper _helper;        
        
         public RateReviewService()
        {
            _helper = new RateReviewHelper();
        }

        public void LaunchRateAndReview(int count)
        {
            _helper.LaunchRateAndReview(count);
        }
    }
}
