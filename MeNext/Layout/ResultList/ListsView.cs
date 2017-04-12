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
    public class ListsView<T> : ListView where T : IMetadata
    {
        //private IResultList<T> resultList;
        //private BetterObservableCollection<ResultItemWrapper<T>> resultCollection;
        //ResultItemFactory<T> resultItemFactory;

        private List<ResultsGroup<T>> allGroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MeNext.QueuesView`1"/> class. This constructor is for
        /// multiple lists of elements, represented by GroupWrappers.
        /// </summary>
        /// <param name="groups">The list of GroupWrappers</param>
        public ListsView(params ResultsGroup<T>[] groups)
        {
            this.HasUnevenRows = true;

            this.IsGroupingEnabled = true;
            this.GroupDisplayBinding = new Binding("Title");  // TODO

            //this.allGroups = new BetterObservableCollection<MinimalGroupWrapper<ResultItemWrapper<T>>>();

            this.allGroups = new List<ResultsGroup<T>>(groups);
            this.ItemsSource = this.allGroups;

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new ResultListCell();
            });

            //this.GroupHeaderTemplate = new DataTemplate(() =>
            //{
            //    return ell();
            //});
        }

        // Do not call if using result list
        public void UpdateLists(params IEnumerable<T>[] itemses)
        {
            for (int i = 0; i < itemses.Length; ++i) {
                var items = itemses[i];
                var group = this.allGroups[i];
                var wrappedItems = new List<ResultItemWrapper>();
                foreach (var item in items) {
                    var wrappedItem = group.Factory.GetResultItem(item);
                    wrappedItems.Add(new ResultItemWrapper(wrappedItem));
                }
                group.SetAll(wrappedItems);
            }
            //var items = new List<MinimalGroupWrapper<ResultItemWrapper<T>>>();
            //foreach (var wrapper in wrappedLists) {
            //    var factory = wrapper.Factory;
            //    var newWrapped = new BetterObservableCollection<ResultItemWrapper<T>>();
            //    foreach (var x in wrapper.Items) {
            //        var item = factory.GetResultItem(x);
            //        newWrapped.Add(new ResultItemWrapper<T>(item));
            //    }
            //    var newWrapper = new MinimalGroupWrapper<ResultItemWrapper<T>>(wrapper.Title, newWrapped);
            //    items.Add(newWrapper);
            //}
            //this.allGroups.SetAll(items);

            //Debug.WriteLine("Set groups!");
            //foreach (var x in this.allGroups) {
            //    Debug.WriteLine("Group: " + x.Title);
            //    foreach (var y in x.Items) {
            //        Debug.WriteLine("\tItem: " + y.ResultItem.Title);
            //    }
            //}
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

        //// Do not call if using groups
        //public void UpdateResultList(IResultList<T> resultList)
        //{
        //    Debug.Assert(this.allGroups == null);
        //    // TODO
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

    }

    /// <summary>
    /// This wrapper allows us to use the Xamarin property reflection witchcraft to pass the entire ResultItem instead
    /// of binding all of its properties separately, which is tedious and more brittle.
    /// </summary>
    public class ResultItemWrapper
    {
        public ResultItemData ResultItem { get; private set; }

        public ResultItemWrapper(ResultItemData resultItem)
        {
            this.ResultItem = resultItem;
        }
    }


}
