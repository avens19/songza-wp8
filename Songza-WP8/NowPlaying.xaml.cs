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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;

namespace Songza_WP8
{
    public partial class NowPlaying : PhoneApplicationPage
    {
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public NowPlaying()
        {
            InitializeComponent();

            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.pause.png",UriKind.Relative));
            else
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.play.png", UriKind.Relative));

            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            CurrentTrack.DataContext = BackgroundAudioPlayer.Instance.Track;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var track = BackgroundAudioPlayer.Instance.Track;

            string id = track.Tag;

            Station s = await API.GetStation(id);

            Station.DataContext = s;

            LoadSimilar(s);
        }

        async void LoadSimilar(Station s)
        {
            List<Station> stns = await API.SimilarStations(s);

            Similar.ItemsSource = stns;
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.pause.png", UriKind.Relative));
            else
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.play.png", UriKind.Relative));

            CurrentTrack.DataContext = BackgroundAudioPlayer.Instance.Track;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Paused)
            {
                BackgroundAudioPlayer.Instance.Play();
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.pause.png", UriKind.Relative));
            }
            else
            {
                BackgroundAudioPlayer.Instance.Pause();
                PlayPause.Source = new BitmapImage(new Uri("Images/transport.play.png", UriKind.Relative));
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.SkipNext();

            CurrentTrack.DataContext = BackgroundAudioPlayer.Instance.Track;
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Station s = (Station)b.Tag;

            NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}", s.Id), UriKind.Relative));
        }
    }
}