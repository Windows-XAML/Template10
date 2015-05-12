namespace Blank1.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        private string _Value = "Hello Template 10";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }
    }
}
