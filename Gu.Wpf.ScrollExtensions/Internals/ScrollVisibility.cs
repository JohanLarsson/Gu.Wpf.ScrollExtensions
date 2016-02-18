namespace Gu.Wpf.ScrollExtensions
{
    internal struct ScrollVisibility
    {
        internal readonly ScrolledIntoView X;
        internal readonly ScrolledIntoView Y;

        public ScrollVisibility(ScrolledIntoView x, ScrolledIntoView y)
        {
            X = x;
            Y = y;
        }
    }
}