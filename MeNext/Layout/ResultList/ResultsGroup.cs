using System;
using System.Collections.Generic;
using MeNext.MusicService;
using Xamarin.Forms;

namespace MeNext
{
    public class ResultsGroup<T> : BetterObservableCollection<ResultItemData> where T : IMetadata
    {
        public string Title { get; private set; }
        public ResultItemFactory<T> Factory { get; private set; }

        public ResultsGroup(string title, ResultItemFactory<T> factory)
        {
            this.Title = title;
            this.Factory = factory;
        }
    }
}
