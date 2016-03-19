using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace DyanmicFontSize.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var textService = new Services.TextService.TextService();
            Paragraphs = await textService.GetTextAsync();
        }

        IEnumerable<Models.IBlock> _Paragraphs = default(IEnumerable<Models.IBlock>);
        public IEnumerable<Models.IBlock> Paragraphs { get { return _Paragraphs; } set { Set(ref _Paragraphs, value); } }
    }
}

