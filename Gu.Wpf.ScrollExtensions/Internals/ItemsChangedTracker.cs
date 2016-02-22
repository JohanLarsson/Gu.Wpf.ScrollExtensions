namespace Gu.Wpf.ScrollExtensions
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;

    public sealed class ItemsChangedTracker : IWeakEventListener, IDisposable
    {
        private readonly WeakReference<System.Windows.Controls.ListBox> targetReference;
        public ItemsChangedTracker(System.Windows.Controls.ListBox target)
        {
            targetReference = new WeakReference<System.Windows.Controls.ListBox>(target);
            CollectionChangedEventManager.AddListener(target.Items, this);
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                System.Windows.Controls.ListBox target;
                if (targetReference.TryGetTarget(out target))
                {
                    target.RaiseEvent(new RoutedEventArgs(ListBox.ItemsChangedEvent));
                }

                return true;
            }
            return false;
        }

        public void Dispose()
        {
            System.Windows.Controls.ListBox target;
            if (targetReference.TryGetTarget(out target))
            {
                CollectionChangedEventManager.RemoveListener(target.Items, this);
            }
        }
    }
}
