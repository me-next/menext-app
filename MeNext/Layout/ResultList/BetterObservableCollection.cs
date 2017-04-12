using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MeNext
{
    /// <summary>
    /// An ObservableCollection with some extra goodies
    /// </summary>
    public class BetterObservableCollection<T> : ObservableCollection<T>
    {
        public BetterObservableCollection(IEnumerable<T> items) : base(items)
        {
        }

        public BetterObservableCollection()
        {
        }

        /// <summary>
        /// Allows the addition of multiple songs to the observable model at once
        /// Functionally the same as looping through newSongs and calling the normal Add function on each
        ///   except that the NotifyCollectionChangedEvent triggers only once, instead of for each item
        /// </summary>
        public void AddMultiple(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            foreach (var item in items) {
                this.Items.Add(item);
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Sets all items in the model. This is useful when updating the model with a new pull. This is the same as 
        /// AddMultiple, but with a clear
        /// </summary>
        /// <param name="items">New songs.</param>
        public void SetAll(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            this.Items.Clear();
            foreach (var item in items) {
                this.Items.Add(item);
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
