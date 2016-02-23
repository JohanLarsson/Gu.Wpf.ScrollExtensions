namespace Gu.Wpf.ScrollExtensions
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static partial class ListBoxItem
    {
        private static readonly DependencyPropertyKey IsScrolledIntoViewPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsScrolledIntoView",
            typeof(ScrolledIntoView),
            typeof(ListBoxItem),
            new PropertyMetadata(ScrolledIntoView.Nope, OnScrolledIntoViewChanged));

        public static readonly DependencyProperty IsScrolledIntoViewProperty = IsScrolledIntoViewPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey HasAppearedPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasAppeared",
            typeof(bool),
            typeof(ListBoxItem),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty HasAppearedProperty = HasAppearedPropertyKey.DependencyProperty;

        static ListBoxItem()
        {
            EventManager.RegisterClassHandler(typeof(ScrollViewer), ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(OnScrollChanged));
            EventManager.RegisterClassHandler(typeof(ScrollViewer), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnScrollChanged));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.ListBoxItem), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnListBoxItemSizeChanged));
        }

        private static void SetIsScrolledIntoView(System.Windows.Controls.ListBoxItem element, ScrolledIntoView value)
        {
            element.SetValue(IsScrolledIntoViewPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.ListBoxItem))]
        public static ScrolledIntoView GetIsScrolledIntoView(System.Windows.Controls.ListBoxItem element)
        {
            return (ScrolledIntoView)element.GetValue(IsScrolledIntoViewProperty);
        }


        private static void SetHasAppeared(System.Windows.Controls.ListBoxItem element, bool value)
        {
            element.SetValue(HasAppearedPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.ListBoxItem))]
        public static bool GetHasAppeared(System.Windows.Controls.ListBoxItem element)
        {
            return (bool)element.GetValue(HasAppearedProperty);
        }
        private static void OnScrolledIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = (System.Windows.Controls.ListBoxItem)d;
            item.RaiseEvent(new RoutedEventArgs(ScrolledIntoViewChangedEvent));
            var scrolledIntoView = (ScrolledIntoView)e.NewValue;
            if (Equals(item.GetValue(HasAppearedProperty), false))
            {
                switch (scrolledIntoView)
                {
                    case ScrolledIntoView.Fully:
                    case ScrolledIntoView.Partly:
                        item.RaiseEvent(new RoutedEventArgs(FirstAppearanceEvent));
                        item.SetValue(HasAppearedPropertyKey, true);
                        break;
                    case ScrolledIntoView.Nope:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void OnScrollChanged(object sender, RoutedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            var listBox = scrollViewer.FirstVisualAncestorOfType<System.Windows.Controls.ListBox>();
            if (listBox == null)
            {
                return;
            }

            UpdateItemsScrolledIntoView(listBox, scrollViewer);
        }

        private static void OnListBoxItemSizeChanged(object sender, RoutedEventArgs e)
        {
            var listBoxItem = (System.Windows.Controls.ListBoxItem)sender;
            var scrollViewer = listBoxItem.FirstVisualAncestorOfType<ScrollViewer>();
            if (scrollViewer != null)
            {
                var scrolledIntoView = scrollViewer.IsChildInView(listBoxItem);
                SetIsScrolledIntoView(listBoxItem, scrolledIntoView.Y);
            }
        }

        private static void UpdateItemsScrolledIntoView(System.Windows.Controls.ListBox listBox, ScrollViewer scrollViewer)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                var item = listBox.ItemContainerGenerator.ContainerFromIndex(i) as System.Windows.Controls.ListBoxItem;
                if (item == null)
                {
                    continue;
                }

                // looping them all can potentially be expensive, profiler will tell
                var isInView = scrollViewer.IsChildInView(item);
                SetIsScrolledIntoView(item, isInView.Y);
            }
        }
    }
}
