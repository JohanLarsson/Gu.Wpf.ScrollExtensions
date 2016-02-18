namespace Gu.Wpf.ScrollExtensions
{
    using System.Collections.Generic;
    using System.Windows.Media;

    internal static class VisualTreeHelperExt
    {
        internal static T FirstVisualAncestorOfType<T>(this Visual child)
            where T : Visual
        {
            var parent = VisualTreeHelper.GetParent(child) as Visual;
            while (parent != null)
            {
                var match = parent as T;
                if (match != null)
                {
                    return match;
                }
                child = parent;
                parent = VisualTreeHelper.GetParent(child) as Visual;
            }

            return null;
        }

        internal static IEnumerable<Visual> VisualAncestors(this Visual child)
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