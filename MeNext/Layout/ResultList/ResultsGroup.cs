using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// Represents a group of results which would be shown in a list of groups. E.g., this is used to represent the
    /// suggestion and play next queues within the queue view.
    /// </summary>
    public class ResultsGroup<T> : BetterObservableCollection<ResultItemData> where T : IMetadata
    {
        /// <summary>
        /// The title of the group, which is shown above all the elements of said group.
        /// </summary>
        public string Title { get; private set; }  // Reflection, do not rename

        /// <summary>
        /// The factory used for producing data for items in this group.
        /// </summary>
        public IResultItemFactory<T> Factory { get; private set; }

        /// <summary>
        /// Creates a new results group
        /// </summary>
        /// <param name="title">The title of the group.</param>
        /// <param name="factory">The factory used for items in the group.</param>
        public ResultsGroup(string title, IResultItemFactory<T> factory)
        {
            this.Title = title;
            this.Factory = factory;
        }
    }
}
