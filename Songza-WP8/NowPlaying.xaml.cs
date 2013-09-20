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

            try
            {
                BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(BackgroundAudioPlayer.Instance != null && BackgroundAudioPlayer.Instance.Track != null)
                CurrentTrack.DataContext = BackgroundAudioPlayer.Instance.Track;

            if (settings.Contains("login") && BackgroundAudioPlayer.Instance != null && BackgroundAudioPlayer.Instance.Track != null && BackgroundAudioPlayer.Instance.Track.Tag != null)
                Add.Visibility = System.Windows.Visibility.Visible;
            else
                Add.Visibility = System.Windows.Visibility.Collapsed;

            try
            {
                if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
                    PlayPause.Source = new BitmapImage(new Uri("Images/transport.pause.png", UriKind.Relative));
                else
                    PlayPause.Source = new BitmapImage(new Uri("Images/transport.play.png", UriKind.Relative));

                var track = BackgroundAudioPlayer.Instance.Track;

                string id = API.GetCurrentStation(track);

                string state = API.GetThumbUpState(track);

                if(state == "1")
                    ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle-blue.png", UriKind.Relative));
                else
                    ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle.png", UriKind.Relative));

                Station s = await API.GetStation(id);

                Station.DataContext = s;

                LoadSimilar(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

            try
            {
                var track = BackgroundAudioPlayer.Instance.Track;

                string state = API.GetThumbUpState(track);

                if (state == "1")
                    ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle-blue.png", UriKind.Relative));
                else
                    ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle.png", UriKind.Relative));
            }
            catch (Exception) {
                ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle.png", UriKind.Relative));
            }
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

        private void ThumbUp_Click(object sender, RoutedEventArgs e)
        {
            string station, track;
            API.GetCurrentStationAndTrack(BackgroundAudioPlayer.Instance.Track, out station, out track);

            if (string.IsNullOrWhiteSpace(station) || string.IsNullOrWhiteSpace(track))
                return;

            API.ThumbUpTrack(station, track);

            API.AddThumbUpToTrack(BackgroundAudioPlayer.Instance.Track);

            ThumbUpCircle.Source = new BitmapImage(new Uri("Images/basecircle-blue.png", UriKind.Relative));
        }

        private void ThumbDown_Click(object sender, RoutedEventArgs e)
        {
            string station, track;
            API.GetCurrentStationAndTrack(BackgroundAudioPlayer.Instance.Track,out station,out track);

            if (string.IsNullOrWhiteSpace(station) || string.IsNullOrWhiteSpace(track))
                return;

            API.ThumbDownTrack(station, track);
            BackgroundAudioPlayer.Instance.SkipNext();
            CurrentTrack.DataContext = BackgroundAudioPlayer.Instance.Track;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button sp = (Button)sender;
                Station s = (Station)sp.Tag;

                Track t = await API.NextTrack(s.Id);

                AudioTrack at = API.CreateTrack(t, s.Id.ToString());

                BackgroundAudioPlayer.Instance.Track = at;

                BackgroundAudioPlayer.Instance.Play();

                NavigationService.Navigate(new Uri("/NowPlaying.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            if (API.IsLoggedIn())
            {
                MenuItem mi = (MenuItem)sender;
                Station station = (Station)mi.Tag;

                NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}", station.Id), UriKind.Relative));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Station s = (Station)b.Tag;

            NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}", s.Id), UriKind.Relative));
        }
    }
}