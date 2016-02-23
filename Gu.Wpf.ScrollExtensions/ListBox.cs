namespace Gu.Wpf.ScrollExtensions
{
    using System.Windows;

    public static class ListBox
    {
        internal static readonly RoutedEvent ItemsChangedEvent = EventManager.RegisterRoutedEvent(
            "ItemsChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ListBox));

        internal static void AddItemsChangedHandler(this System.Windows.Controls.ListBox o, RoutedEventHandler handler)
        {
            o.AddHandler(ItemsChangedEvent, handler);
        }

        internal static void RemoveItemsChangedHandler(this System.Windows.Controls.ListBox o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ItemsChangedEvent, handler);
        }
    }
}
