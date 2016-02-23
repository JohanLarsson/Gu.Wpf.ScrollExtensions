namespace Gu.Wpf.ScrollExtensions.Demo
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ViewModel
    {
        public ObservableCollection<int> Ints { get; } = new ObservableCollection<int>(Enumerable.Range(0, 12));
    }
}
