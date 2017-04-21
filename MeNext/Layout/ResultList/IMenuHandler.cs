using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MeNext
{
    /// <summary>
    /// An interface which represents a factory which can produce a menu out of a ResultItemData.
    /// </summary>
    public interface IMenuHandler
    {
        List<MenuCommand> ProduceMenu(ResultItemData data);
    }
}
