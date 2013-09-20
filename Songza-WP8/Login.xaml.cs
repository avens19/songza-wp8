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
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoLogin();
        }

        private async void DoLogin()
        {
            try
            {
                Progress.Visibility = System.Windows.Visibility.Visible;
                Error.Text = "";
                await API.Login(Username.Text, Password.Password);

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Error.Text = "Login failed";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Progress.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private new void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                DoLogin();
        }
    }
}