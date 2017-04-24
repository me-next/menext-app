using System;

using Xamarin.Forms;

namespace MeNext
{
    public class AboutPage : ScrollView
    {
        public View prevView;
        private ContentPage homeScreen;

        public AboutPage(ContentPage home)
        {
            this.homeScreen = home;
            prevView = home.Content;

            var closeButton = new Button
            {
                Text = "Back",
            };
            closeButton.Clicked += (sender, e) =>
            {
                homeScreen.Content = prevView;
            };

            var logoImage = new Image { Source = "menext_logo.png", };

            var titleLabel = new Label 
            { 
                Text = "MeNext", 
                FontAttributes = FontAttributes.Bold, 
                HorizontalTextAlignment = TextAlignment.Center 
            };
            var schoolLabel = new Label 
            { 
                Text = "RPI SD&D Spring 2017",
                HorizontalTextAlignment = TextAlignment.Center
            };
            var namesLabel = new Label 
            { 
                Text = "Daniel Centore\nConnor Foody\nSamuel Johnston\nMickey Luquette\nSam Saks-Fithian", 
                FontAttributes = FontAttributes.Italic,
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalTextAlignment = TextAlignment.Center
            };
            var gitLabel = new Label { Text = "https://github.com/me-next", HorizontalTextAlignment = TextAlignment.Center };

            var licenseCredit = new Label
            {
                Text = "Uses GPL v3\n(view license on the github page)",
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var softwareCredit = new Label 
            { 
                Text = "Multiplatform app created with Xamarin Studio Community.\nhttps://www.xamarin.com/",
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var iconCredit = new Label 
            {
                Text = "Icons found via\nhttps://icons8.com/web-app/",
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var jsonCredit = new Label
            {
                Text = "Uses Newtonsoft Json Parser\nhttp://www.newtonsoft.com/json",
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            Content = new StackLayout
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    closeButton,
                    logoImage,
                    titleLabel,
                    schoolLabel,
                    gitLabel,
                    namesLabel,
                    softwareCredit,
                    licenseCredit,
                    iconCredit,
                    jsonCredit
                }
            };
        }
    }
}

