﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.BackgroundAudio;

namespace Songza_WP8
{
    public partial class StationsPage : PhoneApplicationPage
    {
        public StationsPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Progress.Visibility = System.Windows.Visibility.Visible;

            string s;

            if (!NavigationContext.QueryString.TryGetValue("stations", out s) || s == "")
            {
                NavigationService.GoBack();
                return;
            }

            string[] stns = s.Split(',');

            List<Station> stations = await API.ListStations(stns);

            StationList.ItemsSource = stations;

            Progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void but_Click(object sender, RoutedEventArgs e)
        {
            Progress.Visibility = System.Windows.Visibility.Visible;

            Button sp = (Button)sender;
            Station s = (Station)sp.Tag;

            Track t = await API.NextTrack(s.Id);

            AudioTrack at = API.CreateTrack(t, s.Id.ToString());

            BackgroundAudioPlayer.Instance.Track = at;

            BackgroundAudioPlayer.Instance.Play();

            Progress.Visibility = System.Windows.Visibility.Collapsed;

            API.SetCurrentStation(s);

            NavigationService.Navigate(new Uri("/NowPlaying.xaml", UriKind.Relative));
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            Station station = (Station)mi.Tag;

            NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}",station.Id), UriKind.Relative));
        }

    }
}