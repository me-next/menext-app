using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class ResultsGroup<T> : BetterObservableCollection<ResultItemWrapper> where T : IMetadata
    {
        public string Title { get; private set; }
        public ResultItemFactory<T> Factory { get; private set; }

        public ResultsGroup(string title, ResultItemFactory<T> factory)
        {
            this.Title = title;
            this.Factory = factory;
        }
    }

    //public class MinimalGroupWrapper<T>
    //{
    //    public string Title { get; private set; }
    //    public BetterObservableCollection<T> Items { get; private set; }

    //    public MinimalGroupWrapper(string title, BetterObservableCollection<T> items)
    //    {
    //        this.Title = title;
    //        this.Items = items;
    //    }
    //}
}
