namespace Gu.Wpf.ScrollExtensions.Demo
{
    using System.Windows;
    using System.Windows.Controls;

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
            var textBlock = (TextBlock)listBoxItem.Content;
            textBlock.Text = "Appeared" + textBlock.Text;
        }

        private void ListBoxItem_OnScrolledIntoViewChanged(object sender, RoutedEventArgs e)
        {
        }
    }
}
