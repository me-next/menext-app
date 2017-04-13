using System;
using Xamarin.Forms;

namespace MeNext.Layout
{
    /// <summary>
    /// This page comprises the entire screen. It serves the purpose of allowing us to navigate the entire screen,
    /// while when using the tabbed view as the main page we can only navigate within the tabs (on android) which is
    /// ugly.
    /// </summary>
    public class FullWrapperView : NavigationPage
    {
        public FullWrapperView(MainController mainController)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.PushAsync(new MainPage(mainController, this));
        }
    }
}
