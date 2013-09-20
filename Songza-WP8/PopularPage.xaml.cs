using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SongzaClasses;
using System.Collections.ObjectModel;
using Microsoft.Phone.BackgroundAudio;
using System.IO.IsolatedStorage;
using Songza_WP8.Resources;

namespace Songza_WP8
{
    public partial class PopularPage : PhoneApplicationPage
    {
        public PopularPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Progress.Visibility = System.Windows.Visibility.Visible;

            List<Popular> pop = new List<Popular>();
            pop.Add(new Popular(AppResources.Pop_Trending, "trending"));
            pop.Add(new Popular(AppResources.Pop_All, "all-time"));

            List<PivotWrapper> list = new List<PivotWrapper>();

            foreach (var item in pop)
            {
                list.Add(new PivotWrapper()
                {
                    TitleText = item.Name,
                    List = new ObservableCollection<object>()
                });
            }

            PopularPivot.ItemsSource = list;

            Load(pop, list);

        }

        private async void Load(List<Popular> pops, List<PivotWrapper> list)
        {
            try
            {
                for (int i = 0; i < pops.Count; i++)
                {
                    List<Station> subs = await API.PopularStations(pops[i].Tag);

                    foreach (var item in subs)
                    {
                        list[i].List.Add(item);
                    }
                }
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Progress.Visibility = System.Windows.Visibility.Visible;

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
            finally
            {
                Progress.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            Station station = (Station)mi.Tag;

            NavigationService.Navigate(new Uri(string.Format("/Favorites.xaml?station={0}", station.Id), UriKind.Relative));
        }
    }
}