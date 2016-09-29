using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;

namespace Template10.Utils
{
    public static class InkUtils
    {
        public async static Task SaveAsync(this InkCanvas inkCanvas, string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.TemporaryFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Any())
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                }
            }
        }

        public async static Task LoadAsync(this InkCanvas inkCanvas, string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.TemporaryFolder;
            var file = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (file != null)
            {
                try
                {
                    using (var stream = await file.OpenSequentialReadAsync())
                    {
                        await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                    }
                }
                catch { }
            }
        }

        public static void Clear(this InkCanvas inkCanvas)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        public static async Task<string> Recognize(this InkCanvas inkCanvas)
        {
            var strokes = inkCanvas.InkPresenter.StrokeContainer;
            if (strokes.GetStrokes().Any())
            {
                var recognizer = new InkRecognizerContainer();
                var results = await recognizer.RecognizeAsync(strokes, InkRecognitionTarget.All);
                var candidates = results.Select(x => x.GetTextCandidates().First());
                return string.Join(" ", candidates);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
