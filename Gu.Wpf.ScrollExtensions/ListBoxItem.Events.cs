namespace Gu.Wpf.ScrollExtensions
{
    using System.Windows;

    public static partial class ListBoxItem
    {
        public static readonly RoutedEvent FirstAppearanceEvent = EventManager.RegisterRoutedEvent(
            "FirstAppearance",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ListBoxItem));

        public static readonly RoutedEvent ScrolledIntoViewChangedEvent = EventManager.RegisterRoutedEvent(
            "ScrolledIntoViewChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ListBoxItem));

        public static void AddScrolledIntoViewChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(ScrolledIntoViewChangedEvent, handler);
        }

        public static void RemoveScrolledIntoViewChangedHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(ScrolledIntoViewChangedEvent, handler);
        }

        public static void AddFirstAppearanceHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.AddHandler(FirstAppearanceEvent, handler);
        }

        public static void RemoveFirstAppearanceHandler(this UIElement o, RoutedEventHandler handler)
        {
            o.RemoveHandler(FirstAppearanceEvent, handler);
        }
    }
}
