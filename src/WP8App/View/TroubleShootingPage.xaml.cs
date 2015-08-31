using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Advertising;
using System.Windows.Media;

// Developed by Deovandski Skibinski Junior
// Updated on 2/22/2014

namespace WPAppStudio.View
{
    public partial class TroubleShootingPage : PhoneApplicationPage
    {
        public TroubleShootingPage()
        {
            InitializeComponent();
        }

        // Debug Diagnostics for Ad

        private void Ad1_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error);
        }

        // Go to Technical Information
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Changelog.xaml", UriKind.Relative));
        }

        // Stop Media Element player
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            MEstreamStatus.Text = "Media Element is Off";
        }

        // Start Media Element player
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
            MEstreamStatus.Text = "Media Element is On";
            MEbufferStatus.Text = mediaElement.BufferingTime.ToString() + "s";
        }

        // Change values according to Buffer Time
        private void mediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            MEbufferStatus.Text = mediaElement.BufferingTime.ToString() + "s";
           // if (MEbufferStatus.Text == "00:00:00") { MEbufferStatus.Foreground = new SolidColorBrush(Colors.Red); }
            //if (MEbufferStatus.Text == "00:00:05") { MEbufferStatus.Foreground = new SolidColorBrush(Colors.Green); }
        }

        // Close Media Element and return to Main Menu. Do not allow call to Radio Stream or else there will be a loop call between these two pages.
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mediaElement = null;
            NavigationService.Navigate(new Uri("/View/MenuSection_Menu.xaml", UriKind.Relative));
        }
    }
}