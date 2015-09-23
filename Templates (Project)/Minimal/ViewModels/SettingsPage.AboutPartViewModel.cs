using System;

namespace Sample.ViewModels
{
    public class AboutPartViewModel : Mvvm.ViewModelBase
    {
        public Uri Logo
        {
            get { return Windows.ApplicationModel.Package.Current.Logo; }
        }

        public string DisplayName
        {
            get { return Windows.ApplicationModel.Package.Current.DisplayName; }
        }

        public string Publisher
        {
            get { return Windows.ApplicationModel.Package.Current.PublisherDisplayName; }
        }

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe
        {
            get { return new Uri("http://bing.com"); }
        }
    }
}
