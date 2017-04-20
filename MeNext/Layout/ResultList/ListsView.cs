using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using MeNext.MusicService;
using System.Diagnostics;

namespace MeNext
{
    public class ListsView<T> : ListView where T : IMetadata
    {
        private List<ResultsGroup<T>> allGroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.QueuesView`1"/> class. This constructor is for
        /// multiple lists of elements, represented by GroupWrappers.
        /// </summary>
        /// <param name="controller">The main controller</param>
        /// <param name="groups">The list of GroupWrappers</param>
        public ListsView(MainController controller, params ResultsGroup<T>[] groups)
        {
            this.RowHeight = LayoutConsts.ROW_HEIGHT;
            this.IsGroupingEnabled = true;
            this.GroupDisplayBinding = new Binding("Title");

            this.allGroups = new List<ResultsGroup<T>>(groups);
            this.ItemsSource = this.allGroups;

            // Disable selected items
            this.ItemSelected += (sender, e) =>
            {
                if (e == null) {
                    return;
                }
                this.SelectedItem = null;
            };

            // Tapping item
            this.ItemTapped += (sender, e) =>

            {
                var data = e.Item as ResultItemData;
                if (data.TapCommand != null && data.TapCommand.CanExecute(data)) {
                    data.TapCommand.Execute(data);
                }
            };

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new ResultListCell(controller);
            });
        }

        /// <summary>
        /// Updates the list contents of all the groups, parallel to the groups supplied during construction.
        /// </summary>
        /// <param name="itemses">The lists of group contents</param>
        public void UpdateLists(params IEnumerable<T>[] itemses)
        {
            Debug.Assert(itemses.Length == this.allGroups.Count);
            for (int i = 0; i < itemses.Length; ++i) {
                var items = itemses[i];
                var group = this.allGroups[i];
                var wrappedItems = new List<ResultItemData>();
                foreach (var item in items) {
                    var wrappedItem = group.Factory.GetResultItem(item);
                    wrappedItems.Add(wrappedItem);
                }
                group.SetAll(wrappedItems);
            }
        }

    }


}
