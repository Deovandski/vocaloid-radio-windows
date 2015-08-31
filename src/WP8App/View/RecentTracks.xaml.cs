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

// Developed by Deovandski Skibinski Junior
// Updated on 2/21/2014

//// Recent Track feature is no longer under development.

namespace WPAppStudio.View
{
    public partial class RecentTracks : PhoneApplicationPage
    {
        public RecentTracks()
        {
            InitializeComponent();
        }

        // Debug Diagnostics for Ad

        private void Ad1_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error);
        }

    }
}