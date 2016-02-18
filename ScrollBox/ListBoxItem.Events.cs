namespace ScrollBox
{
    using System.Windows;

    public static partial class ListBoxItem
    {
        private static readonly DependencyProperty HasAppearedProperty = DependencyProperty.RegisterAttached(
            "HasAppeared",
            typeof(bool),
            typeof(ListBoxItem),
            new PropertyMetadata(default(bool)));

        public static readonly RoutedEvent FirstAppearanceEvent = EventManager.RegisterRoutedEvent(
            "FirstAppearance",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(ListBoxItem));

        public static readonly RoutedEvent ScrolledIntoViewChangedEvent = EventManager.RegisterRoutedEvent(
            "ScrolledIntoViewChanged",
            RoutingStrategy.Direct,
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
