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

        private static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.RegisterAttached(
            "ScrollViewer",
            typeof(ScrollViewer),
            typeof(ListBoxItem),
            new PropertyMetadata(default(ScrollViewer)));

        private static readonly DependencyProperty ListBoxItemsProperty = DependencyProperty.RegisterAttached(
            "ListBoxItems",
            typeof(List<System.Windows.Controls.ListBoxItem>),
            typeof(ListBoxItem),
            new PropertyMetadata(default(List<System.Windows.Controls.ListBoxItem>)));

        private static readonly DependencyProperty IsScrolledIntoViewProperty = IsScrolledIntoViewPropertyKey.DependencyProperty;

        static ListBoxItem()
        {
            EventManager.RegisterClassHandler(typeof(ScrollViewer), ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(OnScrollChanged));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.ListBoxItem), FrameworkElement.SizeChangedEvent, new RoutedEventHandler(OnSizeChanged));
        }

        private static void SetIsScrolledIntoView(System.Windows.Controls.ListBoxItem element, ScrolledIntoView value)
        {
            element.SetValue(IsScrolledIntoViewPropertyKey, value);
        }

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
            var items = (IReadOnlyList<System.Windows.Controls.ListBoxItem>)scrollViewer.GetValue(ListBoxItemsProperty);
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                // looping them all can potentially be expensive, profiler will tell
                var isInView = IsInView(scrollViewer, item);
                SetIsScrolledIntoView(item, isInView);
            }
        }

        private static void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            var listBoxItem = (System.Windows.Controls.ListBoxItem)sender;
            var value = listBoxItem.GetValue(ScrollViewerProperty);
            var scrollViewer = value as ScrollViewer;
            if (value == null)
            {
                scrollViewer = listBoxItem.VisualAncestors()
                                          .OfType<ScrollViewer>()
                                          .FirstOrDefault();
                if (scrollViewer == null)
                {
                    listBoxItem.SetValue(ScrollViewerProperty, "No scrollviewer");
                }
                else
                {
                    var items = (List<System.Windows.Controls.ListBoxItem>)scrollViewer.GetValue(ListBoxItemsProperty);
                    if (items == null)
                    {
                        items = new List<System.Windows.Controls.ListBoxItem>(1) { listBoxItem };
                        scrollViewer.SetValue(ListBoxItemsProperty, items);
                    }
                    else
                    {
                        items.Add(listBoxItem);
                    }
                }
            }

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
            var intersection = Rect.Intersect(scrollViewerBounds, transformedBounds);
            //if result is Empty then the element is not in view
            if (intersection == Rect.Empty)
            {
                return ScrolledIntoView.Nope;
            }

            if (intersection == transformedBounds)
            {
                return ScrolledIntoView.Fully;
            }

            return ScrolledIntoView.Partly;
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
