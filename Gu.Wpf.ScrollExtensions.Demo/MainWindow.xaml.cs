namespace Gu.Wpf.ScrollExtensions.Demo
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFirstAppearance(object sender, RoutedEventArgs e)
        {
            var listBoxItem = (System.Windows.Controls.ListBoxItem)sender;
            //listBoxItem.Content = "Appeared" + listBoxItem.Content;
        }

        private void ListBoxItem_OnScrolledIntoViewChanged(object sender, RoutedEventArgs e)
        {
            var listBoxItem = (ListBoxItem)e.OriginalSource;
            var isScrolledIntoView = Gu.Wpf.ScrollExtensions.ListBoxItem.GetIsScrolledIntoView(listBoxItem);
        }

        private void OnSortClick(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(IntListBox.Items);
            if (view.SortDescriptions.Count == 0)
            {
                view.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            }
            var description = view.SortDescriptions[0];
            view.SortDescriptions.Clear();
            var listSortDirection = description.Direction == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
            view.SortDescriptions.Add(new SortDescription("", listSortDirection));
            view.Refresh();
        }
    }
}
