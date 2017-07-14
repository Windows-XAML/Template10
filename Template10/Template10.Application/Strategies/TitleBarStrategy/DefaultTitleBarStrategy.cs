using Template10.Controls;
using Windows.UI.Xaml;

namespace Template10.Strategies.TitleBarStrategy
{
    public class DefaultTitleBarStrategy : ITitleBarStrategy
    {
        public void Startup()
        {
            // this needless test is important due to a platform bug
            try
            {
                if (Application.Current.Resources.ContainsKey("ExtendedSplashBackground"))
                {
                    var unused = Application.Current.Resources["ExtendedSplashBackground"];
                }
            }
            catch { /* this is okay */ }

            // this wonky style of loop is important due to a platform bug
            var count = Application.Current.Resources.Count;
            foreach (var resource in Application.Current.Resources)
            {
                var key = resource.Key;
                if (key == typeof(CustomTitleBar))
                {
                    var style = resource.Value as Style;
                    var title = new CustomTitleBar();
                    title.Style = style;
                }
                count--;
                if (count == 0) break;
            }
        }
    }
}
