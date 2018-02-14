namespace Prism.Windows.Mvvm
{
    public interface IView<T>
    {
        T ViewModel { get; set; }
    }
}
