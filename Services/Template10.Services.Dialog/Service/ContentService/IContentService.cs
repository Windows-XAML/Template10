using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{
    public enum ContentOrientations { Fill, Top, Left, Right, Bottom, Center }

    public class ContentButtonInfo
    {
        public ContentButtonInfo(string text, Action callback)
        {
            Text = text;
            ClickCallback = callback;
        }
        public string Text { get; set; }
        public Action ClickCallback { get; set; }
    }

    public interface IContentService
    {
        Task<ContentDialogResult> ShowAsync(
            object content,
            object title = null,
            ContentOrientations orientation = ContentOrientations.Fill,
            ContentButtonInfo primaryButton = null,
            ContentButtonInfo secondaryButton = null,
            ContentButtonInfo closeButton = null,
            Action openedCallback = null,
            Action closedCallback = null,
            TimeSpan timeout = default(TimeSpan));
    }

    public class ContentService : IContentService
    {
        public Task<ContentDialogResult> ShowAsync(
            object content,
            object title = null,
            ContentOrientations orientation = ContentOrientations.Fill,
            ContentButtonInfo primaryButton = null,
            ContentButtonInfo secondaryButton = null,
            ContentButtonInfo closeButton = null,
            Action openedCallback = null,
            Action closedCallback = null,
            TimeSpan timeout = default(TimeSpan))
        {
            throw new NotImplementedException();
        }
    }
}
