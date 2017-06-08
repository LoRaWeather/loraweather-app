using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using LoraWeather.Models;
using LoraWeather.ViewModels;
using LoraWeather.Views;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LoraWeather
{
    public partial class App
    {
        //private static HttpClient client = new HttpClient();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
