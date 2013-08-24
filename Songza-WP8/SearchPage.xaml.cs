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
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();

            Query.Loaded += Query_Loaded;
        }

        void Query_Loaded(object sender, RoutedEventArgs e)
        {
            Query.Focus();
        }

        private void Query_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                Search();
        }

        private void Search()
        {
            this.Focus();
            string query = Query.Text;

            NavigationService.Navigate(new Uri(string.Format("/SearchResults.xaml?query={0}",query), UriKind.Relative));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}