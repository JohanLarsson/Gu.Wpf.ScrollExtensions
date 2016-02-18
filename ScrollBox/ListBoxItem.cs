namespace ScrollBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Controls;

    public static partial class ListBoxItem
    {
        private static readonly DependencyPropertyKey IsScrolledIntoViewPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsScrolledIntoView",
            typeof(ScrolledIntoView),
            typeof(ListBoxItem),
            new PropertyMetadata(ScrolledIntoView.Nope, OnScrolledIntoViewChanged));

        private static readonly DependencyProperty HasAppearedProperty = DependencyProperty.RegisterAttached(
            "HasAppeared",
            typeof(bool),
            typeof(ListBoxItem),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsScrolledIntoViewProperty = IsScrolledIntoViewPropertyKey.DependencyProperty;

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
                        item.SetValue(HasAppearedProperty, true);
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
            var listBox = scrollViewer.VisualAncestors()
                                    .OfType<ListBox>()
                                    .FirstOrDefault();
            if (listBox == null)
            {
                return;
            }

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                var item = listBox.ItemContainerGenerator.ContainerFromIndex(i) as System.Windows.Controls.ListBoxItem;
                if (item == null)
                {
                    continue;
                }

                // looping them all can potentially be expensive, profiler will tell
                var isInView = IsInView(scrollViewer, item);
                SetIsScrolledIntoView(item, isInView);
            }
        }

        private static void OnListBoxItemSizeChanged(object sender, RoutedEventArgs e)
        {
            var listBoxItem = (System.Windows.Controls.ListBoxItem)sender;
            var scrollViewer = listBoxItem.VisualAncestors()
                                          .OfType<ScrollViewer>()
                                          .FirstOrDefault();
            if (scrollViewer != null)
            {
                var scrolledIntoView = IsInView(scrollViewer, listBoxItem);
                SetIsScrolledIntoView(listBoxItem, scrolledIntoView);
            }
        }

        // http://blogs.msdn.com/b/llobo/archive/2007/01/18/elements-visibility-inside-scrollviewer.aspx
        private static ScrolledIntoView IsInView(ScrollViewer scrollViewer, UIElement item)
        {
            if (!item.IsArrangeValid)
            {
                return ScrolledIntoView.Nope;
                //item.UpdateLayout();
            }

            var itemBounds = new Rect(new Point(0, 0), item.RenderSize);
            var childTransform = item.TransformToAncestor(scrollViewer);
            var transformedBounds = childTransform.TransformBounds(itemBounds);

            //Check if the elements Rect intersects with that of the scrollviewer's
            var scrollViewerBounds = new Rect(new Point(0, 0), scrollViewer.RenderSize);
            if (scrollViewerBounds.Contains(transformedBounds))
            {
                return ScrolledIntoView.Fully;
            }

            if (scrollViewerBounds.IntersectsWith(transformedBounds))
            {
                return ScrolledIntoView.Partly;
            }

            return ScrolledIntoView.Nope;
        }

        private static IEnumerable<Visual> VisualAncestors(this Visual child)
        {
            var parent = VisualTreeHelper.GetParent(child) as Visual;
            while (parent != null)
            {
                yield return parent;
                child = parent;
                parent = VisualTreeHelper.GetParent(child) as Visual;
            }
        }
    }
}
