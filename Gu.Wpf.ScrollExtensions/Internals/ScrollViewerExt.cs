namespace Gu.Wpf.ScrollExtensions
{
    using System.Windows;
    using System.Windows.Controls;

    internal static class ScrollViewerExt
    {
        // http://blogs.msdn.com/b/llobo/archive/2007/01/18/elements-visibility-inside-scrollviewer.aspx
        internal static ScrollVisibility IsChildInView(this ScrollViewer scrollViewer, UIElement item)
        {
            if (!item.IsArrangeValid)
            {
                return new ScrollVisibility(ScrolledIntoView.Nope, ScrolledIntoView.Nope);
                //item.UpdateLayout();
            }

            var itemBounds = new Rect(new Point(0, 0), item.RenderSize);
            var childTransform = item.TransformToAncestor(scrollViewer);
            var transformedBounds = childTransform.TransformBounds(itemBounds);

            //Check if the elements Rect intersects with that of the scrollviewer's
            var scrollViewerBounds = new Rect(new Point(0, 0), scrollViewer.RenderSize);
            if (scrollViewerBounds.Contains(transformedBounds))
            {
                return new ScrollVisibility(ScrolledIntoView.Fully, ScrolledIntoView.Fully);
            }

            if (scrollViewerBounds.IntersectsWith(transformedBounds))
            {
                scrollViewerBounds.Intersect(transformedBounds);
                ScrolledIntoView x = scrollViewerBounds.Width == transformedBounds.Width ? ScrolledIntoView.Fully : ScrolledIntoView.Partly;
                ScrolledIntoView y = scrollViewerBounds.Height == transformedBounds.Height ? ScrolledIntoView.Fully : ScrolledIntoView.Partly;
                return new ScrollVisibility(x, y);
            }

            return new ScrollVisibility(ScrolledIntoView.Nope, ScrolledIntoView.Nope);
        }
    }
}
