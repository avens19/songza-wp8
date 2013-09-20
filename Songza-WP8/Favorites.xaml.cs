using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using SongzaClasses;
using Microsoft.Phone.BackgroundAudio;
using Songza_WP8.Resources;

namespace Songza_WP8
{
    public partial class Favorites : PhoneApplicationPage
    {
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private string Station;
        private bool loaded = false;

        public Favorites()
        {
            InitializeComponent();

            ((ApplicationBarIconButton) ApplicationBar.Buttons[0]).Text = AppResources.AppBar_Add;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                if (!loaded)
                {
                    Progress.Visibility = System.Windows.Visibility.Visible;

                    string s = null;

                    NavigationContext.QueryString.TryGetValue("station", out s);

                    Station = s;

                    if (!settings.Contains("login"))
                        NavigationService.GoBack();

                    List<Favorite> favs = await API.Favorites();

                    FavoriteList.ItemsSource = favs;

                    loaded = true;
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

        private async void but_Click(object sender, RoutedEventArgs e)
        {
            if (Station != null)
            {
                try
                {
                    Button b = (Button)sender;
                    Favorite a = (Favorite)b.Tag;

                    await API.AddToFavorite(Station, a.Id.ToString());

                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Button b = (Button)sender;
                Favorite a = (Favorite)b.Tag;

                List<int> stns = a.StationIds;

                string stations = "";

                for (int i = 0; i < stns.Count; i++)
                {
                    stations += stns[i];

                    if (i < stns.Count - 1)
                        stations += ",";
                }

                NavigationService.Navigate(new Uri("/StationsPage.xaml?stations=" + stations, UriKind.Relative));
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            CustomMessageBox box = new CustomMessageBox()
            {
                Caption = AppResources.NL_Caption,
                Message = AppResources.NL_Message,
                LeftButtonContent = AppResources.NL_Left,
                RightButtonContent = AppResources.NL_Right,
                Content = new TextBox()
                {
                    Width = 300
                }
            };

            box.Dismissed += async (s, boxEventArgs) =>
                {
                    if (boxEventArgs.Result == CustomMessageBoxResult.LeftButton)
                    {
                        string title = ((TextBox)box.Content).Text;
                        Favorite f = await API.CreateFavorite(title);
                        FavoriteList.ItemsSource.Add(f);
                    }
                };

            box.Show();
        }
    }
}