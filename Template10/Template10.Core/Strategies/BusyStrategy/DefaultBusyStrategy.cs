using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Strategies
{
    public static partial class Settings
    {
    }

    public interface IBusyStrategy
    {
        DataTemplate DataTemplate { get; set; }
        void Show(TimeSpan timeout, params string[] param);
        void Hide();
    }

    public class DefaultBusyStrategy : IBusyStrategy
    {
        public DataTemplate DataTemplate { get; set; }
        public void Show(TimeSpan timeout, params string[] param)
        {
            // TODO
        }
        public void Hide()
        {
            // TODO
        }
    }
}