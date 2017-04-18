using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MeNext
{
    public interface IMenuHandler
    {
        List<MenuCommand> ProduceMenu(ResultItemData data);
    }
}
