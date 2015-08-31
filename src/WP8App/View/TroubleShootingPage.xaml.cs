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
using Microsoft.Phone.Net.NetworkInformation;

// Developed by Deovandski Skibinski Junior
// Updated on 2/22/2014

namespace WPAppStudio.View
{
    public partial class TroubleShootingPage : PhoneApplicationPage
    {
        private String MEBufferTime = "";

        public TroubleShootingPage()
        {
            InitializeComponent();
            PhoneApplicationService phoneAppService = PhoneApplicationService.Current;
            phoneAppService.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        // Debug Diagnostics for Ad

        private void Ad1_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
        //     System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error);
        }

        // Go to Technical Information
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Changelog.xaml", UriKind.Relative));
        }

        // Stop Media Element player
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                mediaElement.Stop();
                MEstreamStatus.Text = "Media Element is Off";
                MEstreamStatus.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                MEstreamStatus.Text = "Check your Connection";
                MEstreamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }

        }

        // Event handler. Call updateMEStatus()
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.updateMEStatus(true);
        }

        /// <summary>
        /// Start Media Element player + Change values according to Buffer Time
        /// Start Media Element if parameter receive is true
        /// </summary>
        /// <param name="startPlayer"></param>
        private void updateMEStatus(bool startPlayer)
        {
            try
            {
                MEBufferTime = mediaElement.BufferingTime.ToString();
                if (MEBufferTime == "00:00:00") { MEBufferTime = "0s"; MEstreamStatus.Foreground = new SolidColorBrush(Colors.Orange); }
                if (MEBufferTime == "00:00:10") { MEBufferTime = "10s"; MEstreamStatus.Foreground = new SolidColorBrush(Colors.Green); }
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    if (startPlayer == true) { mediaElement.Play(); }
                    MEstreamStatus.Text = "M.E. is On || Buffer is " + MEBufferTime;
                }
                else
                {
                    MEstreamStatus.Text = "Check your Connection";
                    MEstreamStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception) { MEstreamStatus.Text = "Error (Ref 5)"; }
        }

        // Event handler. Call updateMEStatus()
        private void mediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            this.updateMEStatus(false);
        }

        // Close Media Element and return to Main Menu. Do not allow call to Radio Stream or else there will be a loop call between these two pages.
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mediaElement = null;
            NavigationService.Navigate(new Uri("/View/MenuSection_Menu.xaml", UriKind.Relative));
        }
    }
}