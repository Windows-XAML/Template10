using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Template10.Services.ProfileService
{
    public class ProfileService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ProfileService()
        {
            StartAsync();
        }

        async void StartAsync()
        {
            try
            {
                var users = await Windows.System.User.FindAllAsync();
                var current = users.FirstOrDefault();
                FirstName = await current.GetPropertyAsync(Windows.System.KnownUserProperties.FirstName) as string;
                LastName = await current.GetPropertyAsync(Windows.System.KnownUserProperties.LastName) as string;
                var display = await current.GetPropertyAsync(Windows.System.KnownUserProperties.DisplayName) as string;
                DisplayName = string.IsNullOrEmpty(display) ? $"{FirstName} {LastName}" : display;
            }
            catch (Exception)
            {
                throw;
            }
        }

        string _FirstName = "None";
        public string FirstName { get { return _FirstName; } private set { Set(ref _FirstName, value); } }

        string _LastName = "None";
        public string LastName { get { return _LastName; } private set { Set(ref _LastName, value); } }

        string _DisplayName = "None";
        public string DisplayName { get { return _DisplayName; } private set { Set(ref _DisplayName, value); } }
    }
}
