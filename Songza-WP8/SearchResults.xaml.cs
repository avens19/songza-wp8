using System;
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
    public partial class SearchResults : PhoneApplicationPage
    {
        public SearchResults()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string query;

            if (!NavigationContext.QueryString.TryGetValue("query", out query) || query == "")
            {
                NavigationService.GoBack();
                return;
            }

            List<Station> stations = await API.QueryStations(query);
            Stations.ItemsSource = stations;
            List<Track.Artist> artists = await API.QueryArtists(query);
            ArtistList.ItemsSource = artists;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button sp = (Button)sender;
            Station s = (Station)sp.Tag;

            Track t = await API.NextTrack(s.Id);

            AudioTrack at = API.CreateTrack(t, s.Id.ToString());

            BackgroundAudioPlayer.Instance.Track = at;

            BackgroundAudioPlayer.Instance.Play();

            NavigationService.Navigate(new Uri("/NowPlaying.xaml", UriKind.Relative));
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            Station station = (Station)mi.Tag;

            NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}", station.Id), UriKind.Relative));
        }

        private async void Artist_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Track.Artist a = (Track.Artist)b.Tag;

            List<Station> stns = await API.StationsForArtist(a.Id);

            string stations = "";

            for (int i = 0; i < stns.Count; i++)
            {
                stations += stns[i].Id;

                if (i < stns.Count - 1)
                    stations += ",";
            }

            NavigationService.Navigate(new Uri("/StationsPage.xaml?stations=" + stations, UriKind.Relative));
        }
    }
}