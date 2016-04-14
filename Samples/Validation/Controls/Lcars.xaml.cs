using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.ValidationSample.Controls
{
    public sealed partial class Lcars : UserControl
    {
        public Lcars()
        {
            this.InitializeComponent();
            Loaded += Lcars_Loaded;
        }

        private void Lcars_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.1) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        Random random = new Random((int)DateTime.Now.Ticks);
        private void Timer_Tick(object sender, object e)
        {
            TextBlock text = null;
            switch (random.Next(1, 6))
            {
                case 1: text = Text1; break;
                case 2: text = Text2; break;
                case 3: text = Text3; break;
                case 4: text = Text4; break;
                case 5: text = Text5; break;
                case 6: text = Text6; break;
            }
            text.Text = (int.Parse(text.Text) + random.Next(-1, 1)).ToString();
        }

        public string Stardate => DateTime.Now.ToString("yyyyMMdd");
    }
}
