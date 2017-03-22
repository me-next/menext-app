﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MeNext.Layout;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MeNext
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}