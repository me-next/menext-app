using System;

using Xamarin.Forms;
using System.Collections.Generic;
using MeNext.MusicService;
using System.Threading.Tasks;

namespace MeNext
{
    /// <summary>
    /// This is a view which allows us to display an IResultList of items which automatically handles loading new
    /// pages of results when the bottom of the page is hit.
    /// </summary>
    public class ResultListView<T> : ListView where T : IMetadata
    {
        private IResultList<T> resultList;
        private BetterObservableCollection<ResultItemData> resultCollection;
        private IResultItemFactory<T> resultItemFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.ResultListView`1"/> class.
        /// </summary>
        /// <param name="controller">The main controller.</param>
        /// <param name="resultItemFactory">The factory for creating ResultItemData.</param>
        public ResultListView(MainController controller, IResultItemFactory<T> resultItemFactory)
        {
            this.RowHeight = LayoutConsts.ROW_HEIGHT;

            this.resultItemFactory = resultItemFactory;

            this.resultCollection = new BetterObservableCollection<ResultItemData>();
            this.ItemsSource = this.resultCollection;

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new ResultListCell(controller);
            });

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

            // Load new items when we hit the bottom
            this.ItemAppearing += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    // If the item being loaded is (probably) the last one
                    // Note that in principle a list with multiple of the same item could start to load early, but this
                    // would be unusual and isn't detrimental to the user experience if we happen to load an occassional
                    // page early.
                    // TODO Actually test that
                    if (e.Item == resultCollection[resultCollection.Count - 1]) {
                        this.AddNextPage();
                    }
                });
            };
        }

        /// <summary>
        /// Updates the result list with a new one
        /// </summary>
        /// <param name="resultList">The new result list</param>
        /// <param name="jumpToTop">If set to <c>true</c> jump to top. Otherwise, leave the scroll position alone.</param>
        public void UpdateResultList(IResultList<T> resultList, bool jumpToTop = true)
        {
            this.resultList = resultList;

            // It might be best to combine this into 1 op so we don't get a flicker, but none seems to be noticable,
            // so w/e
            this.resultCollection.Clear();
            this.AddCurrentPage();

            // Jump back to top of list
            if (this.resultCollection.Count > 0 && jumpToTop) {
                this.ScrollTo(this.resultCollection[0], ScrollToPosition.Start, false);
            }
        }

        /// <summary>
        /// Loads the next page of results
        /// </summary>
        private void AddNextPage()
        {
            if (this.resultList.HasNextPage) {
                this.resultList = this.resultList.NextPage;
                this.AddCurrentPage();
            }
        }

        /// <summary>
        /// Loads the current page of results (assumption: the current page hasn't been loaded yet).
        /// </summary>
        private void AddCurrentPage()
        {
            // Keep going until we find a page with > 0 elements or run out of pages
            while (this.resultList.Items.Count == 0) {
                if (this.resultList.HasNextPage) {
                    this.resultList = this.resultList.NextPage;
                } else {
                    return;
                }
            }

            // Add all the items in the page
            var wrappedItems = new List<ResultItemData>();
            foreach (var item in this.resultList.Items) {
                var wrappedItem = this.resultItemFactory.GetResultItem(item);
                wrappedItems.Add(wrappedItem);
            }

            this.resultCollection.AddMultiple(wrappedItems);
        }
    }


}
