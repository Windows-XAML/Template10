using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateAndReview
{
    public interface IRateReview
    {
        void LaunchRateAndReview(int count); // Count is the number of times after 
        //                                      the app is opened Rate And Review MessageBox should pop up
    }
}
