using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using MeNext.MusicService;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeNext
{
    /// <summary>
    /// This is a view which allows us to EITHER (1) display multiple lists of metadata with a title for each grouping
    /// OR (2) display a single incomplete list whose values are loaded dynamically. One cannot have multiple
    /// dynamically loaded lists.
    /// </summary>
    public class ResultListView<T> : ListView where T : IMetadata
    {
        private IResultList<T> resultList;
        private BetterObservableCollection<ResultItemData> resultCollection;
        private ResultItemFactory<T> resultItemFactory;


        public ResultListView(MainController controller, ResultItemFactory<T> resultItemFactory)
        {
            this.resultItemFactory = resultItemFactory;
            this.HasUnevenRows = true;

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

            this.ItemAppearing += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    if (e.Item == resultCollection[resultCollection.Count - 1]) {
                        this.AddNextPage();
                    }
                });
            };
        }

        public void UpdateResultList(IResultList<T> resultList, bool jumpToTop = true)
        {
            this.resultList = resultList;

            // TODO: Combine this into 1 op so we don't get a flicker
            // Although it doesn't really seem to be noticable...
            this.resultCollection.Clear();
            this.AddCurrentPage();

            // Jump back to top of list
            if (this.resultCollection.Count > 0 && jumpToTop) {
                this.ScrollTo(this.resultCollection[0], ScrollToPosition.Start, false);
            }
        }

        private void AddNextPage()
        {
            if (this.resultList.HasNextPage) {
                this.resultList = this.resultList.NextPage;
                this.AddCurrentPage();
            }
        }

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
