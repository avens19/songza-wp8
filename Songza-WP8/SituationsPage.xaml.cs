using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Songza_WP8
{
    public partial class SituationsPage : PhoneApplicationPage
    {
        public SituationsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string json;

            if (!NavigationContext.QueryString.TryGetValue("json", out json))
            {
                NavigationService.GoBack();
                return;
            }

            Scenario s = SimpleJson.DeserializeObject<Scenario>(json);

            Description.Text = s.SelectedMessage;

            SituationsList.ItemsSource = s.Situations;
        }

        void but_Click(object sender, RoutedEventArgs e)
        {
            Button c = (Button)sender;
            Situation s = (Situation)c.Tag;

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