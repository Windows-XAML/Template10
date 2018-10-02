using Prism.Mvvm;
using Prism.Navigation;
using Sample.Services;
using Template10.SampleData.StarTrek;
using System.Threading.Tasks;
using Prism.Services;
using System;

namespace Sample.ViewModels
{

    internal class ItemPageViewModel : BindableBase, INavigatedAware, INavigatedAwareAsync
    {
        private readonly IGestureService _gestureService;
        private readonly INavigationService _navigationService;
        private readonly IJumpListService _jumpListService;
        private readonly IDataService _dataService;

        public ItemPageViewModel(IDataService dataService, Lazy<IGestureService> gestureService,
            INavigationService navigationService, IJumpListService jumpListService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _jumpListService = jumpListService ?? throw new ArgumentNullException(nameof(jumpListService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _gestureService = gestureService.Value ?? throw new ArgumentNullException(nameof(gestureService));
        }

        public async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            try
            {
                Busy = true;
                Status = "Loading";

                if (parameters.TryGetValue<string>(nameof(Member), out var json))
                {
                    if (Member.TryFromJson(json, out var member))
                    {
                        await AddToJumpList(Member = member);
                    }
                    else { Status = "Parameter is invalid."; }
                }
                else if (parameters.TryGetValue<string>(nameof(Member.Character), out var character))
                {
                    if (await LookupMember(character))
                    {
                        await AddToJumpList(Member);
                    }
                    else { Status = $"{character} not found."; }
                }
                else { Status = "Parameter(s) not found."; }
            }
            finally
            {
                Busy = false;
                Status = string.Empty;
            }

            async Task<bool> LookupMember(string character)
            {
                await _dataService.OpenAsync();
                return !((Member = _dataService.Find(character: character)) is null);
            }

            async Task AddToJumpList(Member member)
            {
                var deep_link = $"/MainPage/ItemPage?{nameof(Member.Character)}={member.Character}";
                await _jumpListService.AddAsync(deep_link, member.Character, member.Image.Path);
            }
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _gestureService.KeyDown += GestureService_KeyDown;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _gestureService.KeyDown -= GestureService_KeyDown;
        }

        bool _busy = false;
        public bool Busy
        {
            get => _busy;
            set => SetProperty(ref _busy, value);
        }

        string _status = string.Empty;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private Member _member = default(Member);
        public Member Member
        {
            get => _member;
            set => SetProperty(ref _member, value);
        }

        public async void GoBack()
            => await _navigationService.GoBackAsync();

        private void GestureService_KeyDown(object sender, KeyDownEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Escape)
            {
                _gestureService.RaiseBackRequested();
            }
        }
    }
}