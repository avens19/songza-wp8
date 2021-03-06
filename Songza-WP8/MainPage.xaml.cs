﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Songza_WP8.Resources;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using SongzaClasses;
using System.Diagnostics;

namespace Songza_WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        List<string> Days = new List<string>(new string[] { AppResources.Days_0, AppResources.Days_1, AppResources.Days_2, AppResources.Days_3, AppResources.Days_4, AppResources.Days_5, AppResources.Days_6 });
        List<string> Periods = new List<string>(new string[] { AppResources.Periods_0, AppResources.Periods_1, AppResources.Periods_2, AppResources.Periods_3, AppResources.Periods_4, AppResources.Periods_5 });

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            AppBarStrings();

            Day.ItemsSource = Days;
            Period.ItemsSource = Periods;

            Login();

            int day, period;

            API.DayAndPeriod(out day, out period);

            Day.SelectedIndex = day;
            Period.SelectedIndex = period;

            Day.SelectionChanged += SelectionChanged;
            Period.SelectionChanged += SelectionChanged;

            LittleWatson.CheckForPreviousException();

            LoadConcierge(day, period);
        }

        private async void Login()
        {
            try
            {
                if (settings.Contains("login"))
                    await API.Login((string)settings["login"], (string)settings["password"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AppBarStrings()
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = AppResources.AppBar_Search;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = AppResources.AppBar_Popular;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).Text = AppResources.AppBar_Browse;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = AppResources.AppBar_NowPlaying;
        }

        private void SetUpAppBar()
        {
            ApplicationBar.MenuItems.Clear();

            if (settings.Contains("login"))
            {
                var it1 = new ApplicationBarMenuItem(AppResources.AppBar_Recent);
                it1.Click += Recent_Click;
                var it2 = new ApplicationBarMenuItem(AppResources.AppBar_My);
                it2.Click += MyLists_Click;
                var it3 = new ApplicationBarMenuItem(AppResources.AppBar_Logout);
                it3.Click += Logout_Click;

                ApplicationBar.MenuItems.Add(it1);
                ApplicationBar.MenuItems.Add(it2);
                ApplicationBar.MenuItems.Add(it3);
            }
            else
            {
                var it1 = new ApplicationBarMenuItem(AppResources.AppBar_Login);
                it1.Click += Login_Click;
                var it2 = new ApplicationBarMenuItem(AppResources.AppBar_Signup);
                it2.Click += SignUp_Click;

                ApplicationBar.MenuItems.Add(it1);
                ApplicationBar.MenuItems.Add(it2);
            }
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            WebBrowserTask wbtask = new WebBrowserTask();
            wbtask.Uri = new Uri("http://songza.com/signup");
            wbtask.Show();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

        private void MyLists_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Favorites.xaml", UriKind.Relative));
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            settings.Remove("login");
            settings.Remove("password");
            settings.Remove("userid");

            SetUpAppBar();
        }

        private async void Recent_Click(object sender, EventArgs e)
        {
            List<Station> stns = await API.Recent();

            string stations = "";

            for (int i = 0; i < stns.Count; i++)
            {
                stations += stns[i].Id;

                if (i < stns.Count - 1)
                    stations += ",";
            }

            NavigationService.Navigate(new Uri("/StationsPage.xaml?stations=" + stations, UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetUpAppBar();
        }

        private async void LoadConcierge(int day, int period)
        {
            try
            {
                Progress.Visibility = System.Windows.Visibility.Visible;

                List<Scenario> list = await API.ConciergeCategories(Day.SelectedIndex, Period.SelectedIndex);

                ScenarioList.ItemsSource = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Progress.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            Button c = (Button)sender;
            Scenario s = (Scenario)c.Tag;

            if (s.Situations.Count > 0)
            {
                string json = SimpleJson.SerializeObject(s);

                json = Uri.EscapeDataString(json);

                NavigationService.Navigate(new Uri("/SituationsPage.xaml?json=" + json, UriKind.Relative));
            }
            else
            {
                string stations = "";

                for (int i = 0; i < s.StationIds.Count; i++)
                {
                    stations += s.StationIds[i];

                    if (i < s.StationIds.Count - 1)
                        stations += ",";
                }

                NavigationService.Navigate(new Uri("/StationsPage.xaml?stations=" + stations, UriKind.Relative));
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void Popular_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PopularPage.xaml", UriKind.Relative));
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrowsePage.xaml", UriKind.Relative));
        }

        private void NowPlaying_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NowPlaying.xaml", UriKind.Relative));
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int day = Day.SelectedIndex;
            int period = Period.SelectedIndex;

            LoadConcierge(day, period);
        }
    }
}