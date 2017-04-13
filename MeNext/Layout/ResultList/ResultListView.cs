using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using MeNext.MusicService;
using System.Diagnostics;

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

            this.ItemAppearing += (sender, e) =>
            {
                //vaItem;
            };
        }

        public void UpdateResultList(IResultList<T> resultList)
        {
            this.resultList = resultList;

            // TODO: Combine this into 1 op so we don't get the flicker
            this.resultCollection.Clear();
            this.AddCurrentPage();
        }


        //// TODO: Handle result lists where pages can contain 0 items
        ///// <summary>
        ///// Initializes a new instance of the <see cref="T:MeNext.QueuesView`1"/> class. This constructor is for a
        ///// single result list, which will be loaded lazily as the user scrolls.
        ///// </summary>
        ///// <param name="resultList">Result list.</param>
        //public ListsView(IResultList<T> resultList, ResultItemFactory<T> resultItemFactory)
        //{
        //    this.GenericSettings();
        //    this.allGroups = null;
        //    this.resultItemFactory = resultItemFactory;
        //    this.resultList = resultList;
        //    this.resultCollection = new BetterObservableCollection<ResultItemWrapper<T>>();

        //    // Adds the current page of results
        //    this.addCurrentPage();

        //    //this.ItemAppearing += (sender, e) =>
        //    //{
        //    //    var cue.Item;
        //    //};
        //}

        //private void addCurrentPage()
        //{
        //    var items = new List<ResultItemWrapper<T>>();
        //    foreach (var x in this.resultList.Items) {
        //        var item = this.resultItemFactory.GetResultItem(x);
        //        items.Add(new ResultItemWrapper<T>(item));
        //    }
        //}



        //public void LoadMore()
        //{
        //    if (currentList != null && currentList.FirstResult.HasNextPage) {
        //        this.currentResult = this.currentResult.NextPage;
        //        // TODO Add the header
        //    } else if (resultLists.Count > 0) {
        //        this.currentList = resultLists.Dequeue();
        //        this.currentResult = this.currentList.FirstResult;
        //    } else {
        //        // Nothing else to load
        //        return;
        //    }
        //    // TODO: Add the items ist

        //}

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
