using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10
{
    public class BusyIndicatorDataContext
    {
        public string Text { get; set; }
    }
}

namespace Template10.Strategies
{
    public static partial class Settings
    {
    }

    public interface IBusyStrategy
    {
        DataTemplate DataTemplate { get; set; }
        bool IsShowing { get; }
        void Show(TimeSpan timeout, params string[] param);
        void Hide();
    }

    public class DefaultBusyStrategy : IBusyStrategy
    {
        public DataTemplate DataTemplate { get; set; }
        public bool IsShowing { get; private set; } = false;
        public void Show(TimeSpan timeout, params string[] param)
        {
            if (IsShowing)
            {
                Hide();
            }
            try
            {
                // TODO
            }
            catch { Debugger.Break(); }
            finally { IsShowing = true; }
        }
        public void Hide()
        {
            if (IsShowing)
            {
                try
                {
                    // TODO
                }
                catch { Debugger.Break(); }
                finally { IsShowing = false; }
            }
        }
    }
}