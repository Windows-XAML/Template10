using SerializationService.Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SerializationService.Demo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            this.InitializeComponent();
            
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Person person = new Person();

                PersonPanel.Visibility = Visibility.Visible;
                SerializedText.Text = Services.PeopleService.Instance.
                    SerializePerson(person);
                IDText.Text = person.Id;
                NameText.Text = person.Name;
                AgeText.Text = person.Age.ToString();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string str)
            {
                SerializedText.Text =
                    str;
            }
        }

        private void OnDeserializeRequested(object sender, RoutedEventArgs e)
        {
            Person person =
                Services.PeopleService.Instance.DeserializePerson(SerializedText.Text);
            IDText.Text = person.Id;
            NameText.Text = person.Name;
            AgeText.Text = person.Age.ToString();
            PersonPanel.Visibility = Visibility.Visible;
        }
    }
}
