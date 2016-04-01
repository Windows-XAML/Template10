using System;
using Template10.Mvvm;

namespace Template10.Samples.ShareTargetSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        string _Date = default(string);
        public string Date { get { return _Date; } set { Set(ref _Date, value); } }

        public void Do()
        {
            Date = DateTime.Now.ToString();
        }
    }
}
