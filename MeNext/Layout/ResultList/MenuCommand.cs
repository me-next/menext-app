using System;
using Xamarin.Forms;

namespace MeNext
{
    public class MenuCommand
    {
        public string Title { get; set; }
        public Command<ResultItemData> Command { get; set; }
    }
}
