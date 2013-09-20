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
using System.Threading.Tasks;

namespace Songza_WP8
{
    public partial class BrowsePage : PhoneApplicationPage
    {
        private bool loaded = false;

        public BrowsePage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!loaded)
            {
                Progress.Visibility = System.Windows.Visibility.Visible;

                List<PivotWrapper> list = new List<PivotWrapper>();

                try
                {

                    List<Category> cats = await API.Categories();

                    foreach (var item in cats)
                    {
                        list.Add(new PivotWrapper()
                        {
                            TitleText = item.Name,
                            List = new ObservableCollection<object>()
                        });
                    }

                    BrowsePivot.ItemsSource = list;

                    await Load(cats, list);

                    loaded = true;
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
        }

        private async Task Load(List<Category> cats, List<PivotWrapper> list)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                List<SubCategory> subs = await API.SubCategories(cats[i].Id);

                foreach (var item in subs)
                {
                    list[i].List.Add(item);
                }
            }

            Progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button c = (Button)sender;
            SubCategory s = (SubCategory)c.Tag;

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
}